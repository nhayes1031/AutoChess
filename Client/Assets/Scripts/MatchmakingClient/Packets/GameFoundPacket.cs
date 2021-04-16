using Lidgren.Network;

namespace Client.Matchmaking {
    public class GameFoundPacket : Packet {
        public string Endpoint { get; set; }
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Endpoint = message.ReadString();
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.GameFound);
        }
    }
}
