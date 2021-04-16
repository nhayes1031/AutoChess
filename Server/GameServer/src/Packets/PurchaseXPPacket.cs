using Lidgren.Network;

namespace Server.Game {
    public class PurchaseXPPacket : Packet {

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.PurchaseXP);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}