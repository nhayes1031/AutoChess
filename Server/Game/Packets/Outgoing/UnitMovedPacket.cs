using Lidgren.Network;

namespace Server.Game {
    // NOTE: This is for battle simulation only
    public class UnitMovedPacket : IOutgoingPacket {
        public BoardLocation FromCoords;
        public BoardLocation ToCoords;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.UnitMoved);
            message.Write(FromCoords.coords.q);
            message.Write(FromCoords.coords.r);
            message.Write(ToCoords.coords.q);
            message.Write(ToCoords.coords.r);
        }
    }
}