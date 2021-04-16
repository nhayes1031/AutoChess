using Lidgren.Network;
using OpenMatch;
using System.Threading.Tasks;

namespace Frontend {
    public class MessageHandler {
        private NetServer server;
        private OpenMatchService openMatchService;

        public MessageHandler(NetServer server) {
            this.server = server;
            openMatchService = new OpenMatchService();
            openMatchService.connectionAssigned += HandleConnectionAssigned;
        }

        public void Update() {
            NetIncomingMessage message;
            while ((message = server.ReadMessage()) != null) {
                byte type = message.ReadByte();
                switch (message.MessageType) {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (type == (byte)PacketTypes.Connect) {
                            Logger.Info("Received Connection from " + message.SenderConnection);
                            message.SenderConnection.Approve();
                        } else {
                            message.SenderConnection.Deny();
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        switch (type) {
                            case (byte)PacketTypes.Disconnect:
                                Logger.Info("Disconnecting " + message.SenderConnection);
                                message.SenderConnection.Disconnect("Bye");
                                CleanUpDisconnectedPlayer(message.SenderConnection);
                                break;
                            case (byte)PacketTypes.QueueForGame:
                                Logger.Info(message.SenderConnection + " queued for a game");
                                _ = AddPlayerToQueueAsync(message.SenderConnection);
                                break;
                            case (byte)PacketTypes.CancelQueueForGame:
                                Logger.Info(message.SenderConnection + " stop queueing for a game");
                                _ = RemovePlayerFromQueueAsync(message.SenderConnection);
                                break;
                            default:
                                Logger.Error("Unhandled data / packet type: " + type);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
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

        private void CleanUpDisconnectedPlayer(NetConnection connection) {
            _ = RemovePlayerFromQueueAsync(connection);
        }

        private async Task AddPlayerToQueueAsync(NetConnection connection) {
            var ticket = await openMatchService.CreateTicket(connection);

            var message = server.CreateMessage();
            new QueueForGamePacket() {
                Status = ticket != null
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        private async Task RemovePlayerFromQueueAsync(NetConnection connection) {
            var status = await openMatchService.DeleteTicket(connection);

            var message = server.CreateMessage();
            new CancelQueueForGamePacket() {
                Status = status
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void HandleConnectionAssigned(NetConnection connection, Assignment assignment) {
            _ = openMatchService.DeleteTicket(connection);

            var message = server.CreateMessage();
            new GameFoundPacket() {
                Endpoint = assignment.Connection
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
