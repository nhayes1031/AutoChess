using Lidgren.Network;

namespace Client.Game {
    public class RequestUnitSellPacket : IOutgoingPacket {
        public string Name;
        public ILocation Location;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.RequestUnitSell);
            message.Write(Name);
            if (Location is BenchLocation bench) {
                message.Write((byte)LocationTypes.Bench);
                message.Write(bench.seat);
            }
            else if (Location is BoardLocation board) {
                message.Write((byte)LocationTypes.Board);
                message.Write(board.coords.q);
                message.Write(board.coords.r);
            }
        }
    }
}

