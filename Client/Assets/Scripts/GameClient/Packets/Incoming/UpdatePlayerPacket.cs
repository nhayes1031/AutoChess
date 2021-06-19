using Lidgren.Network;
using System.Collections.Generic;

namespace Client.Game {
    public class UpdatePlayerPacket : IIncomingPacket {
        public int Level;
        public List<string> Shop;
        public int Gold;
        public int XP;
        public int Health;
        
        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Level = message.ReadInt32();
            Shop = new List<string>();
            var count = message.ReadInt32();
            for (int i = 0; i < count; i++) {
                Shop.Add(message.ReadString());
            }
            Gold = message.ReadInt32();
            XP = message.ReadInt32();
            Health = message.ReadInt32();
        }
    }
}
