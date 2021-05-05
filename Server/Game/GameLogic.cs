using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using Server.Game.Systems;
using Server.Game.Timeline;
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
            hub.Subscribe<MoveUnitRequested>(this, MoveUnit);
            hub.Subscribe<PurchaseXPRequested>(this, PurchaseXP);
            hub.Subscribe<UnitPurchased>(this, UnitPurchased);
            hub.Subscribe<UnitLeveledUp>(this, UnitLeveledUp);

            state = GameState.Idle;

            shop = new Shop();

            playerIntializer = new PlayerInitializer();
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
            if (!playerData.IsAlive)
                return;

            var status = playerData.PayForReroll();
            if (status) {
                var newShop = shop.RequestReroll(playerData.Level, playerData.Shop);
                playerData.UpdateShop(newShop);
            }
            SendUpdatePlayerInfoPacket(e.client, playerData);
        }

        private void PurchaseUnit(PurchaseUnitRequested e) {
            var playerData = playerDatas[e.client];
            if (!playerData.IsAlive)
                return;

            playerData.Purchase(e.packet.ShopIndex);
        }

        private void PurchaseXP(PurchaseXPRequested e) {
            var playerData = playerDatas[e.client];
            if (!playerData.IsAlive)
                return;

            playerData.PurchaseXP();
            SendUpdatePlayerInfoPacket(e.client, playerData);
        }

        private void MoveUnit(MoveUnitRequested e) {
            var playerData = playerDatas[e.client];
            if (!playerData.IsAlive)
                return;

            var status = playerData.MoveUnit(
                e.packet.Name,
                e.packet.From,
                e.packet.To
            );

            if (status) {
                SendUnitMovedPacket(e.client, e.packet);
            }
        }

        private void SellUnit(SellUnitRequested e) {
            var playerData = playerDatas[e.client];
            if (!playerData.IsAlive)
                return;

            var status = playerData.SellUnit(
                e.packet.Name,
                e.packet.Location
            );
            if (status) {
                SendSellUnitPacket(e.client, e.packet);
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
        private void SendUnitMovedPacket(NetConnection connection, RequestMoveUnitPacket packet) {
            NetOutgoingMessage message = server.CreateMessage();
            new UnitRepositionedPacket() {
                Name = packet.Name,
                FromLocation = packet.From,
                ToLocation = packet.To
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        // Clients should always be updated with the playerInfoPacket.
        // This will be changed when the board is added to the client.
        private void SendSellUnitPacket(NetConnection connection, RequestUnitSellPacket packet) {
            NetOutgoingMessage message = server.CreateMessage();
            new UnitSoldPacket() {
                Name = packet.Name,
                Location = packet.Location
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendInitialGameSetupPacket() {
            NetOutgoingMessage message = server.CreateMessage();
            new InitialGameSetupPacket() {
                PlayerPorts = playerDatas.Keys.Select( x => x.RemoteEndPoint.Port ).ToList()
            }.PacketToNetOutgoingMessage(message);
            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendTransitionPacketToAllUsers() {
            NetOutgoingMessage message = server.CreateMessage();
            new StateTransitionPacket() {
                Event = timeline.CurrentEvent.ToString()
            }.PacketToNetOutgoingMessage(message);
            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
        }

        private void UpdateAllPlayerInfos() {
            foreach (var entry in playerDatas) {
                var newShop = shop.RequestReroll(entry.Value.Level, entry.Value.Shop);
                entry.Value.UpdateShop(newShop);
                SendUpdatePlayerInfoPacket(entry.Key, entry.Value);
            }
        }

        private void UpdateAllPlayerInfosWithRewards() {
            foreach (var entry in playerDatas) {
                var reward = RewardSystem.GetRewardsFor(entry.Value);
                var newShop = shop.RequestReroll(entry.Value.Level, entry.Value.Shop);
                entry.Value.AddReward(reward);
                entry.Value.UpdateShop(newShop);
                SendUpdatePlayerInfoPacket(entry.Key, entry.Value);
            }
        }

        private void SendUpdatePlayerInfoPacket(NetConnection connection, PlayerData playerData) {
            NetOutgoingMessage message = server.CreateMessage();
            new UpdatePlayerPacket() {
                Level = playerData.Level,
                Shop = playerData.GetShopAsStringArray().ToList(),
                XP = playerData.XP,
                Gold = playerData.Gold,
                Health = playerData.Health
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendPurchaseUnitPacket(NetConnection connection, int shopIndex) {
            NetOutgoingMessage message = server.CreateMessage();
            new UnitPurchasedPacket() {
                ShopIndex = shopIndex
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void UnitPurchased(UnitPurchased e) {
            NetOutgoingMessage message = server.CreateMessage();
            new UnitPurchasedPacket() {
                ShopIndex = e.shopIndex
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, e.connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void UnitLeveledUp(UnitLeveledUp e) {
            NetOutgoingMessage message = server.CreateMessage();
            new UnitLeveledUpPacket() {
                UnitsToRemove = e.units,
                Name = e.name,
                Location = e.location,
                StarLevel = e.starLevel
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, e.connection, NetDeliveryMethod.ReliableOrdered);
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
