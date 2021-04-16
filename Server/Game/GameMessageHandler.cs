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
                        if (type == (byte)PacketTypes.Connect) {
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
                        Packet packet;
                        switch (type) {
                            case ((byte)PacketTypes.PurchaseReroll):
                                Logger.Info("Reroll was purchased by " + message.SenderConnection);
                                packet = new PurchaseRerollPacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new PurchaseRerollRequested() { client = message.SenderConnection });
                                break;
                            case ((byte)PacketTypes.PurchaseXP):
                                Logger.Info("XP was purchased by " + message.SenderConnection);
                                packet = new PurchaseXPPacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new PurchaseXPRequested() { client = message.SenderConnection });
                                break;
                            case ((byte)PacketTypes.PurchaseUnit):
                                Logger.Info("Unit purchase requested by " + message.SenderConnection);
                                packet = new PurchaseUnitPacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new PurchaseUnitRequested() { client = message.SenderConnection, packet = (PurchaseUnitPacket)packet });
                                break;
                            case ((byte)PacketTypes.MoveToBenchFromBoard):
                                if (isAllowedToProcessMessages) {
                                    Logger.Info(message.SenderConnection + " tried to move a unit to the bench from the board");
                                    packet = new MoveToBenchFromBoardPacket();
                                    packet.NetIncomingMessageToPacket(message);
                                    hub.Publish(new MoveToBenchFromBoardRequested() { client = message.SenderConnection, packet = (MoveToBenchFromBoardPacket)packet });
                                }
                                break;
                            case ((byte)PacketTypes.MoveToBoardFromBench):
                                if (isAllowedToProcessMessages) {
                                    Logger.Info(message.SenderConnection + " tried to move a unit to the board from bench");
                                    packet = new MoveToBoardFromBenchPacket();
                                    packet.NetIncomingMessageToPacket(message);
                                    hub.Publish(new MoveToBoardFromBenchRequested() { client = message.SenderConnection, packet = (MoveToBoardFromBenchPacket)packet });
                                }
                                break;
                            case ((byte)PacketTypes.RepositionOnBoard):
                                if (isAllowedToProcessMessages) {
                                    Logger.Info(message.SenderConnection + " tried to reposition a unit on the board");
                                    packet = new RepositionOnBoardPacket();
                                    packet.NetIncomingMessageToPacket(message);
                                    hub.Publish(new RepositionOnBoardRequested() { client = message.SenderConnection, packet = (RepositionOnBoardPacket)packet });
                                }
                                break;
                            case ((byte)PacketTypes.RepositionOnBench):
                                Logger.Info(message.SenderConnection + " tried to reposition a unit on their bench");
                                packet = new RepositionOnBenchPacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new RepositionOnBenchRequested() { client = message.SenderConnection, packet = (RepositionOnBenchPacket)packet });
                                break;
                            case ((byte)PacketTypes.SellUnitFromBench):
                                Logger.Info(message.SenderConnection + " tried to sell a unit from their bench");
                                packet = new SellUnitFromBenchPacket();
                                packet.NetIncomingMessageToPacket(message);
                                hub.Publish(new SellUnitFromBenchRequested() { client = message.SenderConnection, packet = (SellUnitFromBenchPacket)packet });
                                break;
                            case ((byte)PacketTypes.SellUnitFromBoard):
                                if (isAllowedToProcessMessages) {
                                    Logger.Info(message.SenderConnection + " tried to sell a unit from their board");
                                    packet = new SellUnitFromBoardPacket();
                                    packet.NetIncomingMessageToPacket(message);
                                    hub.Publish(new SellUnitFromBoardRequested() { client = message.SenderConnection, packet = (SellUnitFromBoardPacket)packet });
                                }
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
