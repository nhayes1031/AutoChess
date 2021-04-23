using Lidgren.Network;
using System;

namespace Client.Game {
    public class PlayerDiedPacket : Packet {
        public Guid Who { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) { }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Who = Guid.Parse(message.ReadString());
        }
    }
}
