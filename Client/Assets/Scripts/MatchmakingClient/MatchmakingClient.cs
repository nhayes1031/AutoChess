using Lidgren.Network;
using System;
using UnityEngine;

namespace Client.Matchmaking {
    public class MatchmakingClient {
        public Action<bool> Connected;
        public Action<bool> Queued;
        public Action GameFound;

        public NetClient client { get; set; }

        public MatchmakingClient(int port, string server, string serverName) {
            var config = new NetPeerConfiguration(serverName);

            client = new NetClient(config);
            client.RegisterReceivedCallback(new System.Threading.SendOrPostCallback(ReceiveMessage));
            client.Start();

            NetOutgoingMessage message = client.CreateMessage();
            new ConnectPacket().PacketToNetOutgoingMessage(message);

            client.Connect(server, port, message);
        }

        public void ReceiveMessage(object peer) {
            NetIncomingMessage message;

            while ((message = client.ReadMessage()) != null) {
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
                                GameFound?.Invoke();
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
                client.Recycle(message);
            }
        }

        public void ConnectToGameServer(GameFoundPacket packet) {
            Debug.Log("Trying to connect to game server on port " + packet.Port);
            StaticManager.InitializeGameClient(packet.Port, "127.0.0.1", "AutoChess Game");
        }

        public void SendMatchmakingRequest() {
            NetOutgoingMessage message = client.CreateMessage();
            new QueueForGamePacket().PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void CancelMatchmakingRequest() {
            NetOutgoingMessage message = client.CreateMessage();
            new CancelQueueForGamePacket().PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendDisconnect() {
            NetOutgoingMessage message = client.CreateMessage();
            new DisconnectPacket().PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();

            client.Disconnect("Bye!");
        }

        public void Reconnect() {
            var config = new NetPeerConfiguration("AutoChess");

            client = new NetClient(config);
            client.RegisterReceivedCallback(new System.Threading.SendOrPostCallback(ReceiveMessage));
            client.Start();

            NetOutgoingMessage message = client.CreateMessage();
            new ConnectPacket().PacketToNetOutgoingMessage(message);

            client.Connect("127.0.0.1", 34560, message);
        }
    }
}
