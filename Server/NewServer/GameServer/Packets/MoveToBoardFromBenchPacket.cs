using Lidgren.Network;

namespace Server.Game {
    public class MoveToBoardFromBenchPacket : Packet {
        public string Character { get; set; }
        public int FromSeat { get; set; }
        public HexCoords ToCoords { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Character = message.ReadString();
            FromSeat = message.ReadInt32();
            ToCoords = new HexCoords(
                message.ReadInt32(),
                message.ReadInt32()
            );
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.MoveToBoardFromBench);
        }
    }
}
