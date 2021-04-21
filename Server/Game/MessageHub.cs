using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
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
            new SimulationUnitMovedPacket() {
                FromCoords = e.fromCoords,
                ToCoords = e.toCoords
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new SimulationUnitMovedPacket() {
                FromCoords = e.fromCoords,
                ToCoords = e.toCoords
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.connections.First(), NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.connections.Last(), NetDeliveryMethod.ReliableOrdered);
        }

        private void SendUnitAttackedPacket(SimulationUnitAttacked e) {
            NetOutgoingMessage message = server.CreateMessage();
            new SimulationUnitAttackedPacket() {
                Attacker = e.attacker,
                Defender = e.defender,
                Damage = e.damage
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new SimulationUnitAttackedPacket() {
                Attacker = e.attacker,
                Defender = e.defender,
                Damage = e.damage
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.connections.First(), NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.connections.Last(), NetDeliveryMethod.ReliableOrdered);
        }

        private void SendCombatStartedPacket(SimulationCombatStarted e) {
            NetOutgoingMessage message = server.CreateMessage();
            new SimulationCombatStartedPacket() {
                bottomPlayer = e.bottom.Id,
                topPlayer = e.top.Id,
                units = e.units
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new SimulationCombatStartedPacket() {
                bottomPlayer = e.bottom.Id,
                topPlayer = e.top.Id,
                units = e.units
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.bottom.Connection, NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.top.Connection, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendSimulationUnitDiedPacket(SimulationUnitDied e) {
            NetOutgoingMessage message = server.CreateMessage();
            new SimulationUnitDiedPacket() {
                Unit = e.unit
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new SimulationUnitDiedPacket() {
                Unit = e.unit
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.connections.First(), NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.connections.Last(), NetDeliveryMethod.ReliableOrdered);
        }

        private void SendSimulationVictoryPacket(SimulationEndedInVictory e) {
            NetOutgoingMessage message = server.CreateMessage();
            new SimulationEndedInVictoryPacket().PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new SimulationEndedInLossPacket().PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.winner, NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.loser, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendSimulationDrawPacket(SimulationEndedInDraw e) {
            NetOutgoingMessage message = server.CreateMessage();
            new SimulationEndedInDrawPacket() {
                Damage = e.participant1Damage
            }.PacketToNetOutgoingMessage(message);

            NetOutgoingMessage message2 = server.CreateMessage();
            new SimulationEndedInDrawPacket() {
                Damage = e.participant2Damage
            }.PacketToNetOutgoingMessage(message2);

            server.SendMessage(message, e.participant1, NetDeliveryMethod.ReliableOrdered);
            server.SendMessage(message2, e.participant2, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendPlayerDiedPacket(PlayerDied e) {
            NetOutgoingMessage message = server.CreateMessage();
            new PlayerDiedPacket() {
                Who = e.who.RemoteEndPoint.Port
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
