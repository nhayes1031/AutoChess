using Lidgren.Network;
using System;

namespace Server.Game {
    public class PlayerDiedPacket : IOutgoingPacket {
        public Guid Player;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.PlayerDied);
            message.Write(Player.ToString());
        }
    }
}