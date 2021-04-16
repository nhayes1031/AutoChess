using Lidgren.Network;

namespace Client.Game {
    public class SimulationEndedInDrawPacket : Packet {
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }
        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) { }
    }
}
