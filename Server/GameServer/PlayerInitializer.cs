using Lidgren.Network;
using Server.Game.Timers;
using System;
using System.Collections.Generic;

namespace Server.Game {
    /// <summary>
    /// Wait until all players have joined the game and then create a playerData instance for each of them
    /// </summary>
    public class PlayerInitializer {
        public Action<Dictionary<NetConnection, PlayerData>> PlayerDatasCreated;
        public Action GameCouldNotBeStarted;

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
            GameCouldNotBeStarted?.Invoke();
        }

        private bool VerifyAllPlayersJoinedTheGame() {
            return server.ConnectionsCount == Constants.MINIMUM_PLAYERS_FOR_A_GAME;
        }

        private void CreatePlayerDatas() {
            Dictionary<NetConnection, PlayerData> playerDatas = new Dictionary<NetConnection, PlayerData>();
            foreach (var connection in server.Connections) {
                var data = new PlayerData();
                playerDatas.Add(connection, data);
            }
            PlayerDatasCreated?.Invoke(playerDatas);
        }
    }
}
