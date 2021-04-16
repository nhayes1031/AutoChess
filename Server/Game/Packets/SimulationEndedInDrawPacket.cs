using Lidgren.Network;

namespace Server.Game {
    public class SimulationEndedInDrawPacket : Packet {
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SimulationEndedInDraw);
        }
    }
}
