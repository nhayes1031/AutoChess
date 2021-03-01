using Lidgren.Network;

namespace Server.Matchmaking {
    public class GameFoundPacket : Packet {
        public int Port { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.GameFound);
            message.Write(Port);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Port = message.ReadInt32();
        }
    }
}
