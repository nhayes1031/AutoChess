using Lidgren.Network;

namespace Client.Game {
    public class SimulationEndedInVictoryPacket : Packet {
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }
        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) { }
    }
}
