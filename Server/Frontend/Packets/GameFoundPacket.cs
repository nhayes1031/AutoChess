using Lidgren.Network;

namespace Frontend {
    public class GameFoundPacket : Packet {
        public string Endpoint { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.GameFound);
            message.Write(Endpoint);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }
    }
}
