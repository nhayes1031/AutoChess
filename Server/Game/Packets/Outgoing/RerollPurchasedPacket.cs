using Lidgren.Network;
using System.Collections.Generic;

namespace Server.Game {
    public class RerollPurchasedPacket : IOutgoingPacket {
        public List<string> Shop;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.RerollPurchased);
            message.Write(Shop.Count);
            foreach (var name in Shop) {
                message.Write(name);
            }
        }
    }
}