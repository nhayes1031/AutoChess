using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using Server.Game.Timers;
using System;
using System.Collections.Generic;

namespace Server.Game.Systems {
    /// <summary>
    /// Wait until all players have joined the game and then create a playerData instance for each of them
    /// </summary>
    public class PlayerInitializer {
        public Action<Dictionary<NetConnection, PlayerData>> PlayerDatasCreated;

        private readonly NetServer server;
        private readonly Timer timeout;

        private readonly Dictionary<NetConnection, PlayerData> playerDatas;

        public PlayerInitializer(NetServer server) {
            this.server = server;
            playerDatas = new Dictionary<NetConnection, PlayerData>();
            PlayerData.PlayerCreated += HandlePlayerCreated;

            timeout = new Timer(Constants.MAXIMUM_TIME_TO_WAIT_FOR_PLAYERS) {
                AutoReset = false
            };
            timeout.Elapsed += HandleTimeout;
        }

        private void HandlePlayerCreated(PlayerData player) {
            playerDatas[player.Connection] = player;
        }

        public void Update(double time, double deltaTime) {
            timeout.Update(deltaTime);
            if (VerifyAllPlayersJoinedTheGame()) CreatePlayerDatas();
        }

        private void HandleTimeout() {
            Hub.Default.Publish(new GameFinished());
        }

        private bool VerifyAllPlayersJoinedTheGame() {
            return playerDatas.Count == Constants.MINIMUM_PLAYERS_FOR_A_GAME;
        }

        private void CreatePlayerDatas() {
            PlayerDatasCreated?.Invoke(playerDatas);
        }
    }
}
