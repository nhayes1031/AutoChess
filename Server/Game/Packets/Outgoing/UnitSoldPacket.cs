using Lidgren.Network;

namespace Server.Game {
    public class UnitSoldPacket : IOutgoingPacket {
        public string Name;
        public ILocation Location;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.UnitSold);
            message.Write(Name);
            if (Location is BenchLocation benchLocation) {
                message.Write((byte)LocationTypes.Bench);
                message.Write(benchLocation.seat);
            } else if (Location is BoardLocation boardLocation) {
                message.Write((byte)LocationTypes.Board);
                message.Write(boardLocation.coords.q);
                message.Write(boardLocation.coords.r);
            }
        }
    }
}