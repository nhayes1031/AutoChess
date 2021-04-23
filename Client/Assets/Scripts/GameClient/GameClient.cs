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
        public Action<TransitionUpdatePacket> TransitionUpdate;
        public Action<UpdatePlayerInfoPacket> UpdatePlayerInfo;
        public Action<string> UnitPurchased;
        public Action<SellUnitFromBenchPacket> UnitSoldFromBench;
        public Action<SellUnitFromBoardPacket> UnitSoldFromBoard;
        public Action<MoveToBoardFromBenchPacket> UnitMovedFromBenchToBoard;
        public Action<SimulationUnitMovedPacket> SimulationUnitMoved;
        public Action<SimulationUnitAttackedPacket> SimulationUnitAttacked;
        public Action<SimulationCombatStartedPacket> SimulationCombatStarted;
        public Action SimulationEndedInDraw;
        public Action SimulationEndedInVictory;
        public Action SimulationEndedInLoss;
        public Action<SimulationUnitDiedPacket> SimulationUnitDied;
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
                                UnitSoldFromBench?.Invoke((SellUnitFromBenchPacket)packet);
                                break;
                            case (int)PacketTypes.SellUnitFromBoard:
                                packet = new SellUnitFromBoardPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitSoldFromBoard?.Invoke((SellUnitFromBoardPacket)packet);
                                break;
                            case (int)PacketTypes.MoveToBoardFromBench:
                                packet = new MoveToBoardFromBenchPacket();
                                packet.NetIncomingMessageToPacket(message);
                                UnitMovedFromBenchToBoard?.Invoke((MoveToBoardFromBenchPacket)packet);
                                break;
                            case (int)PacketTypes.SimulationUnitMoved:
                                packet = new SimulationUnitMovedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                SimulationUnitMoved?.Invoke((SimulationUnitMovedPacket)packet);
                                break;
                            case (int)PacketTypes.SimulationUnitAttacked:
                                packet = new SimulationUnitAttackedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                SimulationUnitAttacked?.Invoke((SimulationUnitAttackedPacket)packet);
                                break;
                            case (int)PacketTypes.SimulationCombatStarted:
                                packet = new SimulationCombatStartedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                SimulationCombatStarted?.Invoke((SimulationCombatStartedPacket)packet);
                                break;
                            case (int)PacketTypes.SimulationEndedInVictory:
                                SimulationEndedInVictory?.Invoke();
                                break;
                            case (int)PacketTypes.SimulationEndedInLoss:
                                SimulationEndedInLoss?.Invoke();
                                break;
                            case (int)PacketTypes.SimulationEndedInDraw:
                                SimulationEndedInDraw?.Invoke();
                                break;
                            case (int)PacketTypes.SimulationUnitDied:
                                packet = new SimulationUnitDiedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                SimulationUnitDied?.Invoke((SimulationUnitDiedPacket)packet);
                                break;
                            case (int)PacketTypes.Connect:
                                packet = new ConnectPacket();
                                packet.NetIncomingMessageToPacket(message);
                                playerId = ((ConnectPacket)packet).playerId;
                                break;
                            case (int)PacketTypes.PlayerDied:
                                packet = new PlayerDiedPacket();
                                packet.NetIncomingMessageToPacket(message);
                                // TODO: Disable all packets that can be sent out when the player has died
                                PlayerDied?.Invoke((PlayerDiedPacket)packet);
                                break;
                            case (int)PacketTypes.GameOver:
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

        public void SellUnit(Hex hex) {
            NetOutgoingMessage message = client.CreateMessage();
            new SellUnitFromBoardPacket() {
                Coords = hex.coords,
                Name = hex.unit
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void MoveUnit(int seat, Hex hex, string character) {
            NetOutgoingMessage message = client.CreateMessage();
            new MoveToBoardFromBenchPacket() {
                FromSeat = seat,
                ToCoords = hex.coords,
                Character = character
            }.PacketToNetOutgoingMessage(message);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
