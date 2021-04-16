using Lidgren.Network;

namespace Client.Matchmaking {
    public class CancelQueueForGamePacket : Packet {
        public bool Status { get; set; }
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.CancelQueueForGame);
        }
    }
}
