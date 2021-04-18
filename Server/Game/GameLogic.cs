using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using Server.Game.Systems;
using Server.Game.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Server.Game {
    public class GameLogic {
        private readonly NetServer server;
        private readonly Hub hub;

        // TODO: These might be defined at the wrong level...
        private Dictionary<NetConnection, PlayerData> playerDatas;
        private readonly Shop shop;

        private readonly GameTimeline timeline;
        private readonly PlayerInitializer playerIntializer;
        private GameState state;

        private bool startingRound = true;

        public GameLogic(NetServer server) {
            this.server = server;
            hub = Hub.Default;

            hub.Subscribe<PurchaseRerollRequested>(this, PurchaseReroll);
            hub.Subscribe<PurchaseUnitRequested>(this, PurchaseUnit);
            hub.Subscribe<MoveToBoardFromBenchRequested>(this, MoveToBoardFromBench);
            hub.Subscribe<MoveToBenchFromBoardRequested>(this, MoveToBenchFromBoard);
            hub.Subscribe<RepositionOnBoardRequested>(this, RepositionOnBoard);
            hub.Subscribe<RepositionOnBenchRequested>(this, RepositionOnBench);
            hub.Subscribe<SellUnitFromBenchRequested>(this, SellUnitFromBench);
            hub.Subscribe<SellUnitFromBoardRequested>(this, SellUnitFromBoard);
            hub.Subscribe<PurchaseXPRequested>(this, PurchaseXP);

            state = GameState.Idle;

            shop = new Shop();

            playerIntializer = new PlayerInitializer(server);
            playerIntializer.PlayerDatasCreated += HandlePlayerDatasCreated;

            timeline = new GameTimeline();
            timeline.Finished += () => hub.Publish(new GameFinished());
            timeline.Changed += HandleTimelineChanged;
            timeline.RoundStart += HandleRoundStart;
        }

        public void Initialize() {
            state = GameState.Initializing;
        }

        public void Update(double time, double deltaTime) {
            switch (state) {
                case GameState.Initializing:
                    playerIntializer.Update(time, deltaTime);
                    break;
                case GameState.Running:
                    timeline.Update(time, deltaTime, playerDatas);
                    break;
            }
        }

        public void CleanUp() {
            state = GameState.Cleaning;

            startingRound = true;
            timeline.Reset();

            state = GameState.Idle;
        }

        #region Event Handlers
        private void PurchaseReroll(PurchaseRerollRequested e) {
            var playerData = playerDatas[e.client];
            var status = playerData.PayForReroll();
            if (status) {
                var newShop = shop.RequestReroll(playerData);
                playerData.UpdateShop(newShop);
            }
            SendUpdatePlayerInfoPacket(e.client, playerData);
        }

        private void PurchaseUnit(PurchaseUnitRequested e) {
            var status = playerDatas[e.client].Purchase(e.packet.Name);
            if (status) {
                SendPurchaseUnitPacket(e.client, e.packet.Name);
            }
        }

        private void PurchaseXP(PurchaseXPRequested e) {
            var playerData = playerDatas[e.client];
            playerData.PurchaseXP();
            SendUpdatePlayerInfoPacket(e.client, playerData);
        }

        private void MoveToBenchFromBoard(MoveToBenchFromBoardRequested e) {
            var status = playerDatas[e.client].MoveUnit(
                CharacterFactory.CreateFromName(e.packet.Character), 
                e.packet.FromCoords, 
                e.packet.ToSeat
            );
            if (status) {
                SendUpdatePlayerInfoPacket(e.client, playerDatas[e.client]);
            }
        }

        private void RepositionOnBoard(RepositionOnBoardRequested e) {
            var status = playerDatas[e.client].MoveUnit(
                CharacterFactory.CreateFromName(e.packet.Character), 
                e.packet.FromCoords, 
                e.packet.ToCoords
            );
            if (status) {
                SendUpdatePlayerInfoPacket(e.client, playerDatas[e.client]);
            }
        }

        private void MoveToBoardFromBench(MoveToBoardFromBenchRequested e) {
            var status = playerDatas[e.client].MoveUnit(
                CharacterFactory.CreateFromName(e.packet.Character), 
                e.packet.FromSeat, 
                e.packet.ToCoords
            );
            if (status) {
                // This is temporary
                SendMoveToBenchFromBoardPacket(e.client, e.packet);
                // SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
            }
        }

        private void RepositionOnBench(RepositionOnBenchRequested e) {
            var status = playerDatas[e.client].MoveUnit(
                CharacterFactory.CreateFromName(e.packet.Character), 
                e.packet.FromSeat, 
                e.packet.ToSeat
            );
            if (status) {
                SendUpdatePlayerInfoPacket(e.client, playerDatas[e.client]);
            }
        }

        private void SellUnitFromBench(SellUnitFromBenchRequested e) {
            var status = playerDatas[e.client].SellUnit(
                CharacterFactory.CreateFromName(e.packet.Name),
                e.packet.Seat
            );
            if (status) {
                // This is temporary. This will be changed when the board is added to the client
                SendSellUnitFromBenchPacket(e.client, e.packet);
                // SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
            }
        }

        private void SellUnitFromBoard(SellUnitFromBoardRequested e) {
            var status = playerDatas[e.client].SellUnit(
                CharacterFactory.CreateFromName(e.packet.Name),
                e.packet.Coords
            );
            if (status) {
                // This is temporary.
                SendSellUnitFromBoardPacket(e.client, e.packet);
                // SendUpdatePlayerInfoPacket(connection, playerDatas[connection]);
            }
        }

        private void HandlePlayerDatasCreated(Dictionary<NetConnection, PlayerData> playerDatas) {
            this.playerDatas = playerDatas;
            SendInitialGameSetupPacket();

            Thread.Sleep(100);

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
            if (timeline.CurrentEvent is Shopping) {
                hub.Publish(new LockMessageHandler() { status = true });
            } else {
                hub.Publish(new LockMessageHandler() { status = false });
            }
            SendTransitionPacketToAllUsers();
        }
        #endregion

        #region Messages
        // This is temporary
        private void SendMoveToBenchFromBoardPacket(NetConnection connection, MoveToBoardFromBenchPacket packet) {
            NetOutgoingMessage message = server.CreateMessage();
            packet.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        // This is temporary. Clients should always be updated with the playerInfoPacket.
        // This will be changed when the board is added to the client.
        private void SendSellUnitFromBenchPacket(NetConnection connection, SellUnitFromBenchPacket packet) {
            NetOutgoingMessage message = server.CreateMessage();
            packet.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        // This is temporary
        private void SendSellUnitFromBoardPacket(NetConnection connection, SellUnitFromBoardPacket packet) {
            NetOutgoingMessage message = server.CreateMessage();
            packet.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendInitialGameSetupPacket() {
            NetOutgoingMessage message = server.CreateMessage();
            new InitialGameSetupPacket() {
                PlayerPorts = playerDatas.Keys.Select( x => x.RemoteEndPoint.Port )
            }.PacketToNetOutgoingMessage(message);
            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendTransitionPacketToAllUsers() {
            NetOutgoingMessage message = server.CreateMessage();
            new TransitionUpdatePacket() {
                Event = timeline.CurrentEvent.GetType().ToString()
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
                var reward = RewardSystem.GetRewardsFor(entry.Value);
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
                Gold = playerData.Gold,
                Health = playerData.Health
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
