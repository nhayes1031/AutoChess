using Lidgren.Network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Game {
    public class GameClient {
        public Action<bool> Connected;
        public Action<List<int>> Ports;
        public Action<string[]> ShopUpdate;
        public Action<TransitionUpdatePacket> TransitionUpdate;
        public Action<UpdatePlayerInfoPacket> UpdatePlayerInfo;
        public Action<string> UnitPurchased;
        public Action<SellUnitFromBenchPacket> UnitSold;

        public List<int> PlayerPorts;

        private NetClient client { get; set; }

        public GameClient() {
            PlayerPorts = new List<int>();
        }

        public void Init(int port, string server, string serverName) {
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
                            case (int)PacketTypes.InitialGameSetup:
                                packet = new InitialGameSetupPacket();
                                packet.NetIncomingMessageToPacket(message);
                                PlayerPorts = ((InitialGameSetupPacket)packet).PlayerPorts;
                                Ports?.Invoke(PlayerPorts);
                                break;
                            case (int)PacketTypes.UpdatePlayerInfo:
                                packet = new UpdatePlayerInfoPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UpdatePlayerInfo?.Invoke((UpdatePlayerInfoPacket)packet);
                                ShopUpdate?.Invoke(((UpdatePlayerInfoPacket)packet).Shop);
                                break;
                            case (int)PacketTypes.TransitionUpdate:
                                packet = new TransitionUpdatePacket();
                                packet.NetIncomingMessageToPacket(message);
                                TransitionUpdate?.Invoke((TransitionUpdatePacket)packet);
                                break;
                            case (int)PacketTypes.PurchaseUnit:
                                packet = new PurchaseUnitPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitPurchased?.Invoke(((PurchaseUnitPacket)packet).Name);
                                break;
                            case (int)PacketTypes.SellUnitFromBench:
                                packet = new SellUnitFromBenchPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitSold?.Invoke((SellUnitFromBenchPacket)packet);
                                break;
                            default:
                                Debug.Log("Game server received a packet");
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

        public void SendDisconnect() {
            NetOutgoingMessage message = client.CreateMessage();
            new DisconnectPacket().PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();

            client.Disconnect("Bye!");
        }

        public void SendRerollRequest() {
            NetOutgoingMessage message = client.CreateMessage();
            new PurchaseRerollPacket().PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void Purchase(string name) {
            NetOutgoingMessage message = client.CreateMessage();
            new PurchaseUnitPacket() {
                Name = name
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendPurchaseXPRequest() {
            NetOutgoingMessage message = client.CreateMessage();
            new PurchaseXPPacket().PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void SellUnit(int seat, string character) {
            NetOutgoingMessage message = client.CreateMessage();
            new SellUnitFromBenchPacket() {
                Name = character,
                Seat = seat
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
