using Lidgren.Network;

namespace Server.Game {
    public class RequestUnitPurchasePacket : IIncomingPacket {
        public string Name;
        public int ShopIndex;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
            ShopIndex = message.ReadInt32();
        }
    }
}
