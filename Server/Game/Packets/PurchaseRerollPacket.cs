using Lidgren.Network;

namespace Server.Game {
    public class PurchaseRerollPacket : Packet {
        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.PurchaseReroll);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}
