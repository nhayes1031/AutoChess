using Lidgren.Network;

namespace Server.Game {
    public class RepositionOnBenchPacket : Packet {
        public string Character { get; set; }
        public int FromSeat { get; set; }
        public int ToSeat { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Character = message.ReadString();
            FromSeat = message.ReadInt32();
            ToSeat = message.ReadInt32();
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.RepositionOnBench);
        }
    }
}
