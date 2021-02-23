using Lidgren.Network;

namespace Client.Game {
    public class TransitionUpdatePacket : Packet {
        public string Event { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Event = message.ReadString();
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.TransitionUpdate);
        }
    }
}
