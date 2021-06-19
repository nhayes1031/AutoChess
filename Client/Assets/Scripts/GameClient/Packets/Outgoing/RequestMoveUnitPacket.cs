using Lidgren.Network;

namespace Client.Game {
    public class RequestMoveUnitPacket : IOutgoingPacket {
        public string Name;
        public ILocation From;
        public ILocation To;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.RequestMoveUnit);
            message.Write(Name);
            if (From is BenchLocation fromBench) {
                message.Write((byte)LocationTypes.Bench);
                message.Write(fromBench.seat);
            }
            else if (From is BoardLocation fromBoard) {
                message.Write((byte)LocationTypes.Board);
                message.Write(fromBoard.coords.q);
                message.Write(fromBoard.coords.r);
            }

            if (To is BenchLocation toBench) {
                message.Write((byte)LocationTypes.Bench);
                message.Write(toBench.seat);
            }
            else if (To is BoardLocation toBoard) {
                message.Write((byte)LocationTypes.Board);
                message.Write(toBoard.coords.q);
                message.Write(toBoard.coords.r);
            }
        }
    }
}
