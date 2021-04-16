using Lidgren.Network;
using System.Collections.Generic;

namespace Server.Game {
    public class UpdatePlayerInfoPacket : Packet {
        public int Level;
        public IEnumerable<string> Shop;
        public int Gold;
        public int XP;
        public int Health;

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.UpdatePlayerInfo);
            message.Write(Level);
            foreach (var character in Shop) {
                message.Write(character);
            }
            message.Write(Gold);
            message.Write(XP);
            message.Write(Health);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}
