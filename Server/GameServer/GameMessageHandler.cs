using Lidgren.Network;
using System;

namespace Server.Game {
    public class GameMessageHandler {
        public Action<NetConnection> RerollRequested;
        public Action<PurchaseUnitRequest> UnitPurchaseRequested;

        private NetServer server;
        private bool isAllowedToProcessMessages = false;

        public GameMessageHandler(NetServer server) {
            this.server = server;
        }

        public void Update() {
            NetIncomingMessage message;
            while ((message = server.ReadMessage()) != null) {
                byte type = message.ReadByte();
                switch (message.MessageType) {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (type == (byte)PacketTypes.Connect) {
                            Logger.Info("Player " + message.SenderEndPoint + " connected to game server");
                            message.SenderConnection.Approve();
                        } else {
                            message.SenderConnection.Deny();
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        Packet packet;
                        switch (type) {
                            //case ((byte)PacketTypes.ShopUpdate):
                            //    Logger.Info("Shop Update requested by " + message.SenderConnection);
                            //    RerollRequested?.Invoke(message.SenderConnection);
                            //    break;
                            case ((byte)PacketTypes.PurchaseUnit):
                                Logger.Info("Unit purchase requested by " + message.SenderConnection);
                                packet = new PurchaseUnitPacket();
                                packet.NetIncomingMessageToPacket(message);
                                var request = new PurchaseUnitRequest() {
                                    Name = ((PurchaseUnitPacket)packet).Name,
                                    connection = message.SenderConnection
                                };
                                UnitPurchaseRequested(request);
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

        public void SetLock(bool status) {
            isAllowedToProcessMessages = status;
        }
    }
}
