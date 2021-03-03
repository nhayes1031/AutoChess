using Lidgren.Network;
using Server.Game.Timeline;
using System;
using System.Collections.Generic;

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
        private Battle[] battles;

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

            // TODO: Battles will be instantiated when the timeline says we are battling.
            // TODO: Battles will be deallocated when the timeline says we aren't battling.
            battles = new Battle[4];
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
                    foreach (var battle in battles) {
                        //battle.Update(time, deltaTime);
                    }
                    break;
            }
        }

        public void CleanUp() {
            state = GameState.Cleaning;

            startingRound = true;
            timeline.Reset();

            state = GameState.Idle;
        }

        public void PurchaseReroll(NetConnection client) {
            var playerData = playerDatas[client];
            var status = playerData.PayForReroll();
            if (status) {
                var newShop = shop.RequestReroll(playerData);
                playerData.UpdateShop(newShop);
            }
            SendUpdatePlayerInfoPacket(client, playerData);
        }

        public void PurchaseUnit(PurchaseUnitPacket packet, NetConnection connection) {
            var status = playerDatas[connection].Purchase(packet.Name);
            if (status) {
                SendPurchaseUnitPacket(connection, packet.Name);
            }
        }

        public void PurchaseXP(NetConnection connection) {
            var playerData = playerDatas[connection];
            playerData.PurchaseXP();
            SendUpdatePlayerInfoPacket(connection, playerData);
        }

        public void MoveUnit(MoveToBenchFromBoardPacket packet, NetConnection connection) {
            var status = playerDatas[connection].MoveUnit(
                CharacterFactory.CreateFromName(packet.Character), 
                packet.FromCoords, 
                packet.ToSeat
            );
            if (status) {
                SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
            }
        }

        public void MoveUnit(RepositionOnBoardPacket packet, NetConnection connection) {
            var status = playerDatas[connection].MoveUnit(
                CharacterFactory.CreateFromName(packet.Character), 
                packet.FromCoords, 
                packet.ToCoords
            );
            if (status) {
                SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
            }
        }

        public void MoveUnit(MoveToBoardFromBenchPacket packet, NetConnection connection) {
            var status = playerDatas[connection].MoveUnit(
                CharacterFactory.CreateFromName(packet.Character), 
                packet.FromSeat, 
                packet.ToCoords
            );
            if (status) {
                SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
            }
        }

        public void MoveUnit(RepositionOnBenchPacket packet, NetConnection connection) {
            var status = playerDatas[connection].MoveUnit(
                CharacterFactory.CreateFromName(packet.Character), 
                packet.FromSeat, 
                packet.ToSeat
            );
            if (status) {
                SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
            }
        }

        public void SellUnit(SellUnitFromBenchPacket packet, NetConnection connection) {
            var status = playerDatas[connection].SellUnit(
                CharacterFactory.CreateFromName(packet.Name),
                packet.Seat
            );
            if (status) {
                // This is temporary. This will be changed when the board is added to the client
                SendSellUnitFromBenchPacket(connection, packet);
                //SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
            }
        }

        public void SellUnit(SellUnitFromBoardPacket packet, NetConnection connection) {
            var status = playerDatas[connection].SellUnit(
                CharacterFactory.CreateFromName(packet.Name),
                packet.Coords
            );
            if (status) {
                SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
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
        // This is temporary. Clients should always be updated with the playerInfoPacket.
        // This will be changed when the board is added to the client.
        private void SendSellUnitFromBenchPacket(NetConnection connection, SellUnitFromBenchPacket packet) {
            NetOutgoingMessage message = server.CreateMessage();
            packet.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

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
