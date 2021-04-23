using Lidgren.Network;
using System;

namespace Client.Game {
    public class GameOverPacket : Packet {
        public Guid Winner { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) { }
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Winner = Guid.Parse(message.ReadString());
        }
    }
}
