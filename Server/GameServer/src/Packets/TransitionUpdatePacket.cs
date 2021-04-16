using Lidgren.Network;

namespace Server.Game {
    public class TransitionUpdatePacket : Packet {
        public string Event { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.TransitionUpdate);
            message.Write(Event);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}
