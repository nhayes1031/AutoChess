using Lidgren.Network;
using System;

namespace Server.Game {
    public class PlayerDiedPacket : Packet {
        public Guid Who { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.PlayerDied);
            message.Write(Who.ToString());
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }
    }
}
