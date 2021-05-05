using Lidgren.Network;

namespace Server.Game {
    public class XPPurchasedPacket : IOutgoingPacket {
        public int XP;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.XPPurchased);
            message.Write(XP);
        }
    }
} 