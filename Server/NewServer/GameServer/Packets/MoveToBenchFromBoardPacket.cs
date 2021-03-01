using Lidgren.Network;

namespace Server.Game {
    public class MoveToBenchFromBoardPacket : Packet {
        public string Character { get; set; }
        public HexCoords FromCoords { get; set; }
        public int ToSeat { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Character = message.ReadString();
            FromCoords = new HexCoords(
                message.ReadInt32(),
                message.ReadInt32()
            );
            ToSeat = message.ReadInt32();
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.MoveToBenchFromBoard);
        }
    }
}
