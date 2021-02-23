using Lidgren.Network;
using Server.Game.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game {
    public class GameLogic {
        public Action Finished;
        public Action<bool> Lock;

        private NetServer server;

        // TODO: These might be defined at the wrong level...
        private Dictionary<NetConnection, PlayerData> playerDatas;
        private Shop shop;
        private RewardSystem rewardSystem;

        private GameTimeline timeline;
        private PlayerInitializer playerIntializer;
        private GameState state;

        private bool startingRound = true;

        public GameLogic(NetServer server) {
            this.server = server;
            state = GameState.Idle;

            shop = new Shop();
            rewardSystem = new RewardSystem();

            playerIntializer = new PlayerInitializer(server);
            playerIntializer.PlayerDatasCreated += HandlePlayerDatasCreated;
            playerIntializer.GameCouldNotBeStarted += () => Finished?.Invoke();

            timeline = new GameTimeline();
            timeline.Finished += () => Finished?.Invoke();
            timeline.Changed += HandleTimelineChanged;
            timeline.RoundStart += HandleRoundStart;
        }

        public void Initialize() {
            state = GameState.Initializing;
        }

        public void Update(double time, double deltaTime) {
            switch(state) {
                case GameState.Initializing:
                    playerIntializer.Update(time, deltaTime);
                    break;
                case GameState.Running:
                    timeline.Update(time, deltaTime);
                    break;
            }
        }

        public void CleanUp() {
            state = GameState.Cleaning;

            startingRound = true;
            timeline.Reset();

            state = GameState.Idle;
        }

        public void Reroll(NetConnection client) {
            var playerData = playerDatas[client];
            var newShop = shop.RequestReroll(playerData);
            playerData.UpdateShop(newShop);
            SendUpdatePlayerInfoPacket(client, playerData);
        }

        public void PurchaseUnit(PurchaseUnitRequest request) {
            var status = playerDatas[request.connection].Purchase(request.Name);
            if (status) {
                SendPurchaseUnitPacket(request.connection, request.Name);
            }
        }

        #region Event Handlers
        private void HandlePlayerDatasCreated(Dictionary<NetConnection, PlayerData> playerDatas) {
            this.playerDatas = playerDatas;
            state = GameState.Running;
            timeline.Start();
        }

        private void HandleRoundStart() {
            if (startingRound) {
                UpdateAllPlayerInfos();
                startingRound = false;
            } else {
                UpdateAllPlayerInfosWithRewards();
            }
        }

        private void HandleTimelineChanged() {
            if (timeline.CurrentEvent == "Shopping") {
                Lock?.Invoke(false);
            } else {
                Lock?.Invoke(true);
            }
            SendTransitionPacketToAllUsers();
        }
        #endregion

        #region Messages
        private void SendTransitionPacketToAllUsers() {
            NetOutgoingMessage message = server.CreateMessage();
            new TransitionUpdatePacket() {
                Event = timeline.CurrentEvent
            }.PacketToNetOutgoingMessage(message);
            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
        }

        private void UpdateAllPlayerInfos() {
            foreach (var entry in playerDatas) {
                var newShop = shop.RequestReroll(entry.Value);
                entry.Value.UpdateShop(newShop);
                SendUpdatePlayerInfoPacket(entry.Key, entry.Value);
            }
        }

        private void UpdateAllPlayerInfosWithRewards() {
            foreach (var entry in playerDatas) {
                var reward = rewardSystem.GetRewardsFor(entry.Value);
                var newShop = shop.RequestReroll(entry.Value);
                entry.Value.AddReward(reward);
                entry.Value.UpdateShop(newShop);
                SendUpdatePlayerInfoPacket(entry.Key, entry.Value);
            }
        }

        private void SendUpdatePlayerInfoPacket(NetConnection connection, PlayerData playerData) {
            NetOutgoingMessage message = server.CreateMessage();
            new UpdatePlayerInfoPacket() {
                Level = playerData.Level,
                Shop = playerData.GetShopAsStringArray(),
                XP = playerData.XP,
                Gold = playerData.Gold
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendPurchaseUnitPacket(NetConnection connection, string name) {
            NetOutgoingMessage message = server.CreateMessage();
            new PurchaseUnitPacket() {
                Name = name
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }
        #endregion

        public enum GameState {
            Idle,
            Initializing,
            Running,
            Cleaning
        }
    }
}
