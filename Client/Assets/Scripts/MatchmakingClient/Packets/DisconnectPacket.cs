using Lidgren.Network;

namespace Client.Matchmaking {
    public class DisconnectPacket : Packet {
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.Disconnect);
        }
    }
}
