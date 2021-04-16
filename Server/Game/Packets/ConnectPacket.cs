using Lidgren.Network;
using System;

namespace Server.Game {
    public class ConnectPacket : Packet {
        public Guid playerId;

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.Connect);
            message.Write(playerId.ToString());
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}
