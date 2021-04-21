using Lidgren.Network;
using System;

namespace Server.Game {
    public class GameOverPacket : Packet {
        public Guid Winner { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.GameOver);
            message.Write(Winner.ToString());
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }
    }
}
