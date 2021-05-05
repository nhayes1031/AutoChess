using Lidgren.Network;

namespace Server.Game {
    public class UnitRepositionedPacket : IOutgoingPacket {
        public string Name;
        public ILocation FromLocation;
        public ILocation ToLocation;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.UnitRepositioned);
            message.Write(Name);
            if (FromLocation is BenchLocation fromBenchLocation) {
                message.Write((byte)LocationTypes.Bench);
                message.Write(fromBenchLocation.seat);
            }
            else if (FromLocation is BoardLocation fromBoardLocation) {
                message.Write((byte)LocationTypes.Board);
                message.Write(fromBoardLocation.coords.q);
                message.Write(fromBoardLocation.coords.r);
            }

            if (ToLocation is BenchLocation toBenchLocation) {
                message.Write((byte)LocationTypes.Bench);
                message.Write(toBenchLocation.seat);
            } else if (ToLocation is BoardLocation toBoardLocation) {
                message.Write((byte)LocationTypes.Board);
                message.Write(toBoardLocation.coords.q);
                message.Write(toBoardLocation.coords.r);
            }
        }
    }
}