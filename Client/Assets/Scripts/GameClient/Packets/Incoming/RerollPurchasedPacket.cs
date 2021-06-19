using Lidgren.Network;
using System.Collections.Generic;

namespace Client.Game {
    public class RerollPurchasedPacket : IIncomingPacket {
        public List<string> Shop;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Shop = new List<string>();
            var count = message.ReadInt32();
            for (int i = 0; i < count; i++)
                Shop.Add(message.ReadString());
        }
    }
}
