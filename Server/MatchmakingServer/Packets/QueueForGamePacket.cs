using Lidgren.Network;

namespace Server.Matchmaking {
    public class QueueForGamePacket : Packet {
        public bool Status { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.QueueForGame);
            message.Write(Status);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}
