using Lidgren.Network;
using System;

namespace Server.Game {
    public class GameMessageHandler {
        public Action<NetConnection> RerollRequested;
        public Action<NetConnection> XPPurchaseRequested;
        public Action<PurchaseUnitPacket, NetConnection> UnitPurchaseRequested;
        public Action<MoveToBenchFromBoardPacket, NetConnection> MoveToBench;
        public Action<MoveToBoardFromBenchPacket, NetConnection> MoveToBoard;
        public Action<RepositionOnBoardPacket, NetConnection> RepositionOnBoard;
        public Action<RepositionOnBenchPacket, NetConnection> RepositionOnBench;

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
                            case ((byte)PacketTypes.PurchaseReroll):
                                Logger.Info("Reroll was purchased by " + message.SenderConnection);
                                packet = new PurchaseRerollPacket();
                                packet.NetIncomingMessageToPacket(message);
                                RerollRequested?.Invoke(message.SenderConnection);
                                break;
                            case ((byte)PacketTypes.PurchaseXP):
                                Logger.Info("XP was purchased by " + message.SenderConnection);
                                packet = new PurchaseXPPacket();
                                packet.NetIncomingMessageToPacket(message);
                                XPPurchaseRequested?.Invoke(message.SenderConnection);
                                break;
                            case ((byte)PacketTypes.PurchaseUnit):
                                Logger.Info("Unit purchase requested by " + message.SenderConnection);
                                packet = new PurchaseUnitPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitPurchaseRequested?.Invoke((PurchaseUnitPacket)packet, message.SenderConnection);
                                break;
                            case ((byte)PacketTypes.MoveToBenchFromBoard):
                                Logger.Info(message.SenderConnection + " tried to move a unit to the bench from the board");
                                packet = new MoveToBenchFromBoardPacket();
                                packet.NetIncomingMessageToPacket(message);
                                MoveToBench?.Invoke((MoveToBenchFromBoardPacket)packet, message.SenderConnection);
                                break;
                            case ((byte)PacketTypes.MoveToBoardFromBench):
                                if (isAllowedToProcessMessages) {
                                    Logger.Info(message.SenderConnection + " tried to move a unit to the board from bench");
                                    packet = new MoveToBoardFromBenchPacket();
                                    packet.NetIncomingMessageToPacket(message);
                                    MoveToBoard?.Invoke((MoveToBoardFromBenchPacket)packet, message.SenderConnection);
                                }
                                break;
                            case ((byte)PacketTypes.RepositionOnBoard):
                                if (isAllowedToProcessMessages) {
                                    Logger.Info(message.SenderConnection + " tried to reposition a unit on the board");
                                    packet = new RepositionOnBoardPacket();
                                    packet.NetIncomingMessageToPacket(message);
                                    RepositionOnBoard?.Invoke((RepositionOnBoardPacket)packet, message.SenderConnection);
                                }
                                break;
                            case ((byte)PacketTypes.RepositionOnBench):
                                Logger.Info(message.SenderConnection + " tried to reposition a unit on their bench");
                                packet = new RepositionOnBenchPacket();
                                packet.NetIncomingMessageToPacket(message);
                                RepositionOnBench?.Invoke((RepositionOnBenchPacket)packet, message.SenderConnection);
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
