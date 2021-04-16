using Lidgren.Network;
using OpenMatch;

namespace Frontend {
    public class CancelQueueForGamePacket : Packet {
        public bool Status { get; set; }
        public Ticket Ticket { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.CancelQueueForGame);
            message.Write(Status);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}
