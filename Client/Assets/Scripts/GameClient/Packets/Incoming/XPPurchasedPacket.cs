using Lidgren.Network;

namespace Client.Game {
    public class XPPurchasedPacket : IIncomingPacket {
        public int XP;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            XP = message.ReadInt32();
        }
    }
}
