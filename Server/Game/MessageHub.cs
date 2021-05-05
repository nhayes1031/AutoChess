using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using System;
using System.Linq;

namespace Server.Game {
    public class MessageHub {
        private readonly NetServer server;
        private readonly Hub hub = Hub.Default;

        public MessageHub(NetServer server) {
            this.server = server;

            hub.Subscribe<SimulationUnitMoved>(this, SendUnitMovedPacket);
            hub.Subscribe<SimulationUnitAttacked>(this, SendUnitAttackedPacket);
            hub.Subscribe<SimulationCombatStarted>(this, SendCombatStartedPacket);
            hub.Subscribe<SimulationEndedInVictory>(this, SendSimulationVictoryPacket);
            hub.Subscribe<SimulationEndedInDraw>(this, SendSimulationDrawPacket);
            hub.Subscribe<SimulationUnitDied>(this, SendSimulationUnitDiedPacket);
            hub.Subscribe<PlayerDied>(this, SendPlayerDiedPacket);
            hub.Subscribe<GameOver>(this, SendGameOverPacket);
        }

        private void SendUnitMovedPacket(SimulationUnitMoved e) {
            NetOutgoingMessage message = server.CreateMessage();
            new UnitMovedPacket() {
                FromCoords = new BoardLocation() { coords = e.fromCoords },
                ToCoords = new BoardLocation() { coords = e.toCoords }
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new UnitMovedPacket() {
                FromCoords = new BoardLocation() { coords = e.fromCoords },
                ToCoords = new BoardLocation() { coords = e.toCoords }
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.connections.First(), NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.connections.Last(), NetDeliveryMethod.ReliableOrdered);
        }

        private void SendUnitAttackedPacket(SimulationUnitAttacked e) {
            NetOutgoingMessage message = server.CreateMessage();
            new UnitAttackPacket() {
                Attacker = new BoardLocation { coords = e.attacker },
                Defender = new BoardLocation { coords = e.defender },
                Damage = e.damage
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new UnitAttackPacket() {
                Attacker = new BoardLocation { coords = e.attacker },
                Defender = new BoardLocation { coords = e.defender },
                Damage = e.damage
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.connections.First(), NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.connections.Last(), NetDeliveryMethod.ReliableOrdered);
        }

        private void SendCombatStartedPacket(SimulationCombatStarted e) {
            NetOutgoingMessage message = server.CreateMessage();
            new CombatStartedPacket() {
                BottomPlayer = e.bottom.Id,
                TopPlayer = e.top.Id,
                Units = e.units
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new CombatStartedPacket() {
                BottomPlayer = e.bottom.Id,
                TopPlayer = e.top.Id,
                Units = e.units
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.bottom.Connection, NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.top.Connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendSimulationUnitDiedPacket(SimulationUnitDied e) {
            NetOutgoingMessage message = server.CreateMessage();
            new UnitDiedPacket() {
                Location = new BoardLocation() { coords = e.unit }
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new UnitDiedPacket() {
                Location = new BoardLocation() { coords = e.unit }
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.connections.First(), NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.connections.Last(), NetDeliveryMethod.ReliableOrdered);
        }

        private void SendSimulationVictoryPacket(SimulationEndedInVictory e) {
            NetOutgoingMessage message = server.CreateMessage();
            new CombatEndedInVictoryPacket().PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new CombatEndedInLossPacket().PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.winner, NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.loser, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendSimulationDrawPacket(SimulationEndedInDraw e) {
            NetOutgoingMessage message = server.CreateMessage();
            new CombatEndedInDrawPacket() {
                Damage = e.participant1Damage
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new CombatEndedInDrawPacket() {
                Damage = e.participant2Damage
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.participant1, NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.participant2, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendPlayerDiedPacket(PlayerDied e) {
            NetOutgoingMessage message = server.CreateMessage();
            new PlayerDiedPacket() {
                Player = e.who
            }.PacketToNetOutgoingMessage(message);

            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendGameOverPacket(GameOver e) {
            NetOutgoingMessage message = server.CreateMessage();
            new GameOverPacket() {
                Winner = e.Winner
            }.PacketToNetOutgoingMessage(message);

            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
