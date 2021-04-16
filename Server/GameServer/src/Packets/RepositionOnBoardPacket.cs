using Lidgren.Network;
using Server.Game.Systems;

namespace Server.Game {
    public class RepositionOnBoardPacket : Packet {
        public string Character { get; set; }
        public HexCoords FromCoords { get; set; }
        public HexCoords ToCoords { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Character = message.ReadString();
            FromCoords = new HexCoords(
                message.ReadInt32(),
                message.ReadInt32()
            );
            ToCoords = new HexCoords(
                message.ReadInt32(),
                message.ReadInt32()
            );
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.RepositionOnBoard);
        }
    }
}
