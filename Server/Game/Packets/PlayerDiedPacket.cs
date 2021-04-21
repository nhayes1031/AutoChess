using Lidgren.Network;

namespace Server.Game {
    public class PlayerDiedPacket : Packet {
        public int Who { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.PlayerDied);
            message.Write(Who);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }
    }
}
