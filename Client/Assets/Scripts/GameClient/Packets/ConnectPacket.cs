using Lidgren.Network;
using System;

namespace Client.Game {
    public class ConnectPacket : Packet {
        public Guid playerId;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            playerId = Guid.Parse(message.ReadString());
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.Connect);
        }
    }
}
