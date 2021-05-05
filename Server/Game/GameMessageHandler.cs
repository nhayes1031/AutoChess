using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using System.Threading;

namespace Server.Game {
    public class GameMessageHandler {
        private readonly Hub hub;
        private readonly NetServer server;
        private bool isAllowedToProcessMessages = true;

        public GameMessageHandler(NetServer server) {
            this.server = server;
            hub = Hub.Default;

            hub.Subscribe<LockMessageHandler>(this, SetLock);
        }

        public void Update() {
            NetIncomingMessage message;
            while ((message = server.ReadMessage()) != null) {
                byte type = message.ReadByte();
                switch (message.MessageType) {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (type == (byte)IncomingPacketTypes.Connect) {
                            Logger.Info("Player " + message.SenderEndPoint + " connected to game server");
                            message.SenderConnection.Approve();

                            var playerData = new PlayerData(message.SenderConnection);
                            NetOutgoingMessage response = server.CreateMessage();
                            new ConnectPacket() {
                                playerId = playerData.Id
                            }.PacketToNetOutgoingMessage(response);

                            Thread.Sleep(50);
                            server.SendMessage(response, message.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                        } else {
                            message.SenderConnection.Deny();
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        IIncomingPacket packet;
                        switch (type) {
                            case ((byte)IncomingPacketTypes.RequestReroll):
                                Logger.Info("Reroll was purchased by " + message.SenderConnection);
                                packet = new RequestRerollPacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new PurchaseRerollRequested() { client = message.SenderConnection });
                                break;
                            case ((byte)IncomingPacketTypes.RequestXPPurchase):
                                Logger.Info("XP was purchased by " + message.SenderConnection);
                                packet = new RequestXPPurchasePacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new PurchaseXPRequested() { client = message.SenderConnection });
                                break;
                            case ((byte)IncomingPacketTypes.RequestUnitPurchase):
                                Logger.Info("Unit purchase requested by " + message.SenderConnection);
                                packet = new RequestUnitPurchasePacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new PurchaseUnitRequested() { client = message.SenderConnection, packet = (RequestUnitPurchasePacket)packet });
                                break;
                            case ((byte)IncomingPacketTypes.RequestMoveUnit):
                                if (isAllowedToProcessMessages) {
                                    Logger.Info(message.SenderConnection + " tried to move a unit to the bench from the board");
                                    packet = new RequestMoveUnitPacket();
                                    packet.NetIncomingMessageToPacket(message);
                                    hub.Publish(new MoveUnitRequested() { client = message.SenderConnection, packet = (RequestMoveUnitPacket)packet });
                                }
                                break;
                            case ((byte)IncomingPacketTypes.RequestUnitSell):
                                Logger.Info(message.SenderConnection + " tried to sell a unit from their bench");
                                packet = new RequestUnitSellPacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new SellUnitRequested() { client = message.SenderConnection, packet = (RequestUnitSellPacket)packet });
                                break;
                            default:
                                Logger.Error("Unhandled date / packet type: " + type);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)message.ReadByte();
                        switch (status) {
                            case NetConnectionStatus.Disconnecting:
                                Logger.Warn("Client disconnecting");
                                break;
                            case NetConnectionStatus.Disconnected:
                                Logger.Warn("Client disconnected");
                                break;
                            default:
                                break;
                        }
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        Logger.Debug("Debug Message: " + message.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Logger.Warn("Warning Message: " + message.ReadString());
                        break;
                    default:
                        Logger.Error("Unhandled message with type: " + message.MessageType);
                        break;
                }
            }
            server.Recycle(message);
        }

        private void SetLock(LockMessageHandler e) {
            isAllowedToProcessMessages = e.status;
        }
    }
}
