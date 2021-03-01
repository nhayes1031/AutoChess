using Lidgren.Network;

namespace Server.Matchmaking {
    public class MatchmakingMessageHandler {
        private NetServer server;
        private MatchmakingLogic matchmakingLogic;

        public MatchmakingMessageHandler(NetServer server) {
            this.server = server;

            matchmakingLogic = new MatchmakingLogic();
        }

        public void Update() {
            NetIncomingMessage message;
            while ((message = server.ReadMessage()) != null) {
                byte type = message.ReadByte();
                switch (message.MessageType) {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (type == (byte)PacketTypes.Connect) {
                            Logger.Info("Received Connection from " + message.SenderEndPoint);
                            message.SenderConnection.Approve();
                        } else {
                            message.SenderConnection.Deny();
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        switch (type) {
                            case (byte)PacketTypes.Disconnect:
                                Logger.Info("Disconnecting " + message.SenderEndPoint);
                                message.SenderConnection.Disconnect("Bye");
                                break;
                            case (byte)PacketTypes.QueueForGame:
                                Logger.Info(message.SenderEndPoint + " queued for a game");
                                AddPlayerToGameQueue(message.SenderConnection);
                                break;
                            case (byte)PacketTypes.CancelQueueForGame:
                                Logger.Info(message.SenderEndPoint + " stop queueing for a game");
                                RemovePlayerFromGameQueue(message.SenderConnection);
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

        public void AddPlayerToGameQueue(NetConnection connection) {
            var status = matchmakingLogic.AddToQueue(connection);

            // TODO: Handle the status packet on the front end
            NetOutgoingMessage message = server.CreateMessage();
            new QueueForGamePacket() {
                Status = status
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);

            if (status && matchmakingLogic.IsThereEnoughPlayersForAGame() && ServerManager.instance.GameServerAvailable()) {
                StartGame();
            }
        }

        public void RemovePlayerFromGameQueue(NetConnection connection) {
            var status = matchmakingLogic.RemoveFromQueue(connection);

            // TODO: Handle the status packet on the front end
            NetOutgoingMessage message = server.CreateMessage();
            new CancelQueueForGamePacket() {
                Status = status
            }.PacketToNetOutgoingMessage(message);
            server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void StartGame() {
            var gameServer = ServerManager.instance.GetGameServer();
            var players = matchmakingLogic.GetPlayersForAGame();
            if (players != null) {
                gameServer.Initialize();
                NetOutgoingMessage message = server.CreateMessage();
                new GameFoundPacket() {
                    Port = gameServer.Port
                }.PacketToNetOutgoingMessage(message);
                server.SendMessage(message, players, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }
    }
}
