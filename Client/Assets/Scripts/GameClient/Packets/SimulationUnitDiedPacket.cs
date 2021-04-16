using Lidgren.Network;

namespace Client.Game {
    public class SimulationUnitDiedPacket : Packet {
        public HexCoords Unit;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Unit = new HexCoords(message.ReadInt32(), message.ReadInt32());
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) { }
    }
}
