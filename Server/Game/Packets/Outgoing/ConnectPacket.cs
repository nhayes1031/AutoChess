using Lidgren.Network;
using System;

namespace Server.Game {
    public partial class ConnectPacket : IOutgoingPacket {
        public Guid playerId;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.Connect);
            message.Write(playerId.ToString());
        }
    }
}