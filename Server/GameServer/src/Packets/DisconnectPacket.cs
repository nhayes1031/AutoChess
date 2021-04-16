﻿using Lidgren.Network;

namespace Server.Game {
    public class DisconnectPacket : Packet {
        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.Disconnect);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }
    }
}
