using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Client.Game {
    public class GameClient {
        public Action<bool> Connected;
        public Action<List<int>> Ports;
        public Action<string[]> ShopUpdate;
        public Action<StateTransitionPacket> TransitionUpdate;
        public Action<UpdatePlayerPacket> UpdatePlayerInfo;
        public Action<string> UnitPurchased;
        public Action<UnitSoldPacket> UnitSold;
        public Action<UnitMovedPacket> UnitMoved;
        public Action<UnitAttackedPacket> UnitAttacked;
        public Action<UnitDiedPacket> UnitDied;
        public Action<CombatStartedPacket> CombatStarted;
        public Action<UnitRepositionedPacket> UnitRepositioned;
        public Action CombatEndedInDraw;
        public Action CombatEndedInVictory;
        public Action CombatEndedInLoss;
        public Action<PlayerDiedPacket> PlayerDied;
        public Action<GameOverPacket> GameOver;

        public List<int> PlayerPorts;
        public Guid PlayerId => playerId;

        private NetClient client { get; set; }

        private Guid playerId;

        public GameClient() {
            PlayerPorts = new List<int>();
        }

        public void Initialize(int port, string server, string serverName) {
            try {

            var config = new NetPeerConfiguration(serverName);

            client = new NetClient(config);
            client.RegisterReceivedCallback(new SendOrPostCallback(ReceiveMessage));
            client.Start();

            NetOutgoingMessage message = client.CreateMessage();
            new ConnectPacket().PacketToNetOutgoingMessage(message);

            client.Connect(server, port, message);
            } catch (Exception e) {
                Debug.Log(e);
            }
        }

        public void ReceiveMessage(object peer) {
            NetIncomingMessage message;

            while ((message = client.ReadMessage()) != null) {
                switch (message.MessageType) {
                    case NetIncomingMessageType.Data:
                        var packetType = (int)message.ReadByte();

                        IIncomingPacket packet;
                        switch (packetType) {
                            case (int)IncomingPacketTypes.InitialGameSetup:
                                packet = new InitialGameSetupPacket();
                                packet.NetIncomingMessageToPacket(message);
                                PlayerPorts = ((InitialGameSetupPacket)packet).PlayerPorts;
                                Ports?.Invoke(PlayerPorts);
                                break;
                            case (int)IncomingPacketTypes.UpdatePlayer:
                                packet = new UpdatePlayerPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UpdatePlayerInfo?.Invoke((UpdatePlayerPacket)packet);
                                ShopUpdate?.Invoke(((UpdatePlayerPacket)packet).Shop.ToArray());
                                break;
                            case (int)IncomingPacketTypes.StateTransition:
                                packet = new StateTransitionPacket();
                                packet.NetIncomingMessageToPacket(message);
                                TransitionUpdate?.Invoke((StateTransitionPacket)packet);
                                break;
                            case (int)IncomingPacketTypes.UnitPurchased:
                                packet = new UnitPurchasedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitPurchased?.Invoke(((UnitPurchasedPacket)packet).Name);
                                break;
                            case (int)IncomingPacketTypes.UnitSold:
                                packet = new UnitSoldPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitSold?.Invoke((UnitSoldPacket)packet);
                                break;
                            case (int)IncomingPacketTypes.UnitRepositioned:
                                packet = new UnitRepositionedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitRepositioned?.Invoke((UnitRepositionedPacket)packet);
                                break;
                            case (int)IncomingPacketTypes.UnitMoved:
                                packet = new UnitMovedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitMoved?.Invoke((UnitMovedPacket)packet);
                                break;
                            case (int)IncomingPacketTypes.UnitAttacked:
                                packet = new UnitAttackedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitAttacked?.Invoke((UnitAttackedPacket)packet);
                                break;
                            case (int)IncomingPacketTypes.CombatStarted:
                                packet = new CombatStartedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                CombatStarted?.Invoke((CombatStartedPacket)packet);
                                break;
                            case (int)IncomingPacketTypes.CombatEndedInVictory:
                                CombatEndedInVictory?.Invoke();
                                break;
                            case (int)IncomingPacketTypes.CombatEndedInLoss:
                                CombatEndedInLoss?.Invoke();
                                break;
                            case (int)IncomingPacketTypes.CombatEndedInDraw:
                                CombatEndedInDraw?.Invoke();
                                break;
                            case (int)IncomingPacketTypes.UnitDied:
                                packet = new UnitDiedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitDied?.Invoke((UnitDiedPacket)packet);
                                break;
                            case (int)IncomingPacketTypes.Connect:
                                packet = new ConnectPacket();
                                packet.NetIncomingMessageToPacket(message);
                                playerId = ((ConnectPacket)packet).playerId;
                                break;
                            case (int)IncomingPacketTypes.PlayerDied:
                                packet = new PlayerDiedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                // TODO: Disable all packets that can be sent out when the player has died
                                PlayerDied?.Invoke((PlayerDiedPacket)packet);
                                break;
                            case (int)IncomingPacketTypes.GameOver:
                                packet = new GameOverPacket();
                                packet.NetIncomingMessageToPacket(message);
                                // TODO: Clean up the game server connection
                                GameOver?.Invoke((GameOverPacket)packet);
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

        public void Disconnect() {
            SendDisconnect();
        }

        private void SendDisconnect() {
            if (client != null) {
                NetOutgoingMessage message = client.CreateMessage();
                new DisconnectPacket().PacketToNetOutgoingMessage(message);
                client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
                client.FlushSendQueue();

                client.Disconnect("Bye!");
            }
        }

        public void SendRerollRequest() {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestRerollPacket().PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void Purchase(string name, int shopIndex) {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestUnitPurchasePacket() {
                Name = name,
                ShopIndex = shopIndex
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendPurchaseXPRequest() {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestXPPurchasePacket().PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void SellUnit(int seat, string character) {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestUnitSellPacket() {
                Name = character,
                Location = new BenchLocation() {
                    seat = seat
                }
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void SellUnit(Hex hex) {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestUnitSellPacket() {
                Name = hex.unit,
                Location = new BoardLocation() {
                    coords = hex.coords
                }
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void MoveUnit(int seat, Hex hex, string character) {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestMoveUnitPacket() {
                From = new BenchLocation() {
                    seat = seat
                },
                To = new BoardLocation() {
                    coords = hex.coords
                },
                Name = character
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void MoveUnit(int fromSeat, int toSeat, string character) {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestMoveUnitPacket() {
                From = new BenchLocation() {
                    seat = fromSeat
                },
                To = new BenchLocation() {
                    seat = toSeat
                },
                Name = character
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void MoveUnit(Hex hex, int seat, string character) {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestMoveUnitPacket() {
                From = new BoardLocation() {
                    coords = hex.coords
                },
                To = new BenchLocation() {
                    seat = seat
                },
                Name = character
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void MoveUnit(Hex fromHex, Hex toHex, string character) {
            NetOutgoingMessage message = client.CreateMessage();
            new RequestMoveUnitPacket() {
                From = new BoardLocation() {
                    coords = fromHex.coords
                },
                To = new BoardLocation() {
                    coords = toHex.coords
                },
                Name = character
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
