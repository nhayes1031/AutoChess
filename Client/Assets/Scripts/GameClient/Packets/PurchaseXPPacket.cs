﻿using Lidgren.Network;

namespace Client.Game {
    public class PurchaseXPPacket : Packet {
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.PurchaseXP);
        }
    }
}
