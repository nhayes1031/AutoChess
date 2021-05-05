using Lidgren.Network;
using System.Collections.Generic;

namespace Server.Game {
    public class UpdatePlayerPacket : IOutgoingPacket {
        public int Level;
        public List<string> Shop;
        public int Gold;
        public int XP;
        public int Health;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.UpdatePlayer);
            message.Write(Level);
            message.Write(Shop.Count);
            foreach (var unit in Shop) {
                message.Write(unit);
            }
            message.Write(Gold);
            message.Write(XP);
            message.Write(Health);
        }
    }
}