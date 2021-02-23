using Lidgren.Network;

namespace Client.Matchmaking {
    public class ConnectPacket : Packet {
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.Connect);
        }
    }
}
