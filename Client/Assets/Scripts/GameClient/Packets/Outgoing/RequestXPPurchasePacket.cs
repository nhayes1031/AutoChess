using Lidgren.Network;

namespace Client.Game {
    public class RequestXPPurchasePacket : IOutgoingPacket {
        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.RequestXPPurchase);
        }
    }
}
