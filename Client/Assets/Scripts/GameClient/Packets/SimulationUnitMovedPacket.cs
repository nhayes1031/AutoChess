using Lidgren.Network;

namespace Client.Game {
    public class SimulationUnitMovedPacket : Packet {
        public HexCoords FromCoords;
        public HexCoords ToCoords;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            FromCoords = new HexCoords(message.ReadInt32(), message.ReadInt32());
            ToCoords = new HexCoords(message.ReadInt32(), message.ReadInt32());
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) { }
    }
}
