using Lidgren.Network;

namespace Client.Game {
    public class RepositionOnBoardPacket : Packet {
        public string Character { get; set; }
        public HexCoords FromCoords { get; set; }
        public HexCoords ToCoords { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {

        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.RepositionOnBoard);
        }
    }
}
