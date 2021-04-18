using Lidgren.Network;
using System;
using UnityEngine;

namespace Client.Matchmaking {
    public class MatchmakingClient : IMatchmakingClient {
        public Action<bool> Connected { get; set; }
        public Action<bool> Queued { get; set; }
        public Action GameFound { get; set; }

        public NetClient Client { get; set; }

        public MatchmakingClient(int port, string server, string serverName) {
            Connect(port, server, serverName);
        }

        private void Connect(int port, string server, string serverName) {
            var config = new NetPeerConfiguration("Autochess_frontend");

            Client = new NetClient(config);
            Client.RegisterReceivedCallback(new System.Threading.SendOrPostCallback(ReceiveMessage));
            Client.Start();

            NetOutgoingMessage message = Client.CreateMessage();
            new ConnectPacket().PacketToNetOutgoingMessage(message);

            Client.Connect("127.0.0.1", 34560, message);
        }

        public void ReceiveMessage(object peer) {
            NetIncomingMessage message;

            while ((message = Client.ReadMessage()) != null) {
                switch (message.MessageType) {
                    case NetIncomingMessageType.Data:
                        var packetType = (int)message.ReadByte();

                        Packet packet;
                        switch (packetType) {
                            case (int)PacketTypes.Disconnect:
                                packet = new DisconnectPacket();
                                packet.NetIncomingMessageToPacket(message);
                                Connected?.Invoke(false);
                                break;
                            case (int)PacketTypes.QueueForGame:
                                packet = new QueueForGamePacket();
                                packet.NetIncomingMessageToPacket(message);
                                Queued?.Invoke(true);
                                break;
                            case (int)PacketTypes.CancelQueueForGame:
                                packet = new CancelQueueForGamePacket();
                                packet.NetIncomingMessageToPacket(message);
                                Queued?.Invoke(false);
                                break;
                            case (int)PacketTypes.GameFound:
                                packet = new GameFoundPacket();
                                packet.NetIncomingMessageToPacket(message);
                                ConnectToGameServer((GameFoundPacket)packet);
                                break;
                            default:
                                break;
                        }
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        Debug.Log("Debug Message: " + message.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Debug.Log("Warning Message: " + message.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)message.ReadByte();
                        switch (status) {
                            case NetConnectionStatus.InitiatedConnect:
                                Debug.Log("Initiated contact with server");
                                break;
                            case NetConnectionStatus.Connected:
                                Connected?.Invoke(true);
                                break;
                            case NetConnectionStatus.Disconnecting:
                            case NetConnectionStatus.Disconnected:
                                Connected?.Invoke(false);
                                break;
                            default:
                                Debug.Log("Unhandled Status Changed type: " + status);
                                break;
                        }
                        break;
                    default:
                        Debug.Log("Unhandled type: " + message.MessageType + " " + message.LengthBytes + "bytes");
                        break;
                }
                Client.Recycle(message);
            }
        }

        public void ConnectToGameServer(GameFoundPacket packet) {
            Debug.Log("Trying to connect to game server at " + packet.Endpoint);
            // Manager.InitializeGameClient(packet.Port, "127.0.0.1", "AutoChess Game");
        }

        public void SendMatchmakingRequest() {
            NetOutgoingMessage message = Client.CreateMessage();
            new QueueForGamePacket().PacketToNetOutgoingMessage(message);
            Client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void CancelMatchmakingRequest() {
            NetOutgoingMessage message = Client.CreateMessage();
            new CancelQueueForGamePacket().PacketToNetOutgoingMessage(message);
            Client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void Disconnect() {
            SendDisconnect();
        }

        private void SendDisconnect() {
            NetOutgoingMessage message = Client.CreateMessage();
            new DisconnectPacket().PacketToNetOutgoingMessage(message);
            Client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            Client.FlushSendQueue();

            Client.Disconnect("Bye!");
        }
    }
}
