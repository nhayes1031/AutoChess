using Lidgren.Network;

namespace Client.Game {
    public class RequestUnitPurchasePacket : IOutgoingPacket {
        public string Name;
        public int ShopIndex;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.RequestUnitPurchase);
            message.Write(Name);
            message.Write(ShopIndex);
        }
    }
}
