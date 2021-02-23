using Lidgren.Network;

namespace Client.Game {
    public class UpdatePlayerInfoPacket : Packet {
        public int Level;
        public string[] Shop;
        public int Gold;
        public int XP;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Level = message.ReadInt32();
            Shop = new string[5];
            for (int i = 0; i < 5; i++) {
                Shop[i] = message.ReadString();
            }
            Gold = message.ReadInt32();
            XP = message.ReadInt32();
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.UpdatePlayerInfo);
        }
    }
}
