using Lidgren.Network;

namespace Server.Game {
    public class UnitDiedPacket : IOutgoingPacket {
        public BoardLocation Location;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.UnitDied);
            message.Write(Location.coords.q);
            message.Write(Location.coords.r);
        }
    }
}