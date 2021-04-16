using Lidgren.Network;

namespace Server.Game {
    public class SimulationEndedInVictoryPacket : Packet {
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SimulationEndedInVictory);
        }
    }

    public class SimulationEndedInLossPacket : Packet {
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SimulationEndedInLoss);
        }
    }
}
