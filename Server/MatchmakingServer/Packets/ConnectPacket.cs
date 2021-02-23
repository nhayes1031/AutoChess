using Lidgren.Network;

namespace Server.Matchmaking {
    public class ConnectPacket : Packet {
        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.Connect);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}
