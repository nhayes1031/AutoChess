using Lidgren.Network;

namespace Client.Game {
    public class MoveToBenchFromBenchPacket : Packet {
        public string Character { get; set; }
        public int FromSeat { get; set; }
        public HexCoords ToCoords { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {

        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.MoveToBoardFromBench);
        }
    }
}
