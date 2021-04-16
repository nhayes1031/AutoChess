using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using Server.Game.Timers;
using System;
using System.Collections.Generic;

namespace Server.Game.Systems {
    // TODO: Probably don't need to publish to the pub sub system and have custom action
    public class PlayerInitializer {
        public Action<Dictionary<NetConnection, PlayerData>> PlayerDatasCreated;

        private NetServer server;
        private Timer timeout;

        public PlayerInitializer(NetServer server) {
            this.server = server;

            timeout = new Timer(Constants.MAXIMUM_TIME_TO_WAIT_FOR_PLAYERS) {
                AutoReset = false
            };
            timeout.Elapsed += HandleTimeout;
        }

        public void Update(double time, double deltaTime) {
            timeout.Update(deltaTime);
            if (VerifyAllPlayersJoinedTheGame()) CreatePlayerDatas();
        }

        private void HandleTimeout() {
            Hub.Default.Publish(new GameFinished());
        }

        private bool VerifyAllPlayersJoinedTheGame() {
            return server.ConnectionsCount == Constants.MINIMUM_PLAYERS_FOR_A_GAME;
        }

        private void CreatePlayerDatas() {
            Dictionary<NetConnection, PlayerData> playerDatas = new Dictionary<NetConnection, PlayerData>();
            foreach (var connection in server.Connections) {
                var data = new PlayerData(connection);
                playerDatas.Add(connection, data);
            }
            PlayerDatasCreated?.Invoke(playerDatas);
            Hub.Default.Publish<PlayersConnected>();
        }
    }
}
