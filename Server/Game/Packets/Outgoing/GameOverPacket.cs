using Lidgren.Network;
using System;

namespace Server.Game {
    public class GameOverPacket : IOutgoingPacket {
        public Guid Winner;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.GameOver);
            message.Write(Winner.ToString());
        }
    }
}