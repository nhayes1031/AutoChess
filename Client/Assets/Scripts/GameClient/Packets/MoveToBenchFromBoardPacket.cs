using Lidgren.Network;

namespace Client.Game {
    public class MoveToBenchFromBoardPacket : Packet {
        public string Character { get; set; }
        public HexCoords FromCoords { get; set; }
        public int ToSeat { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {

        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.MoveToBenchFromBoard);
        }
    }
}
