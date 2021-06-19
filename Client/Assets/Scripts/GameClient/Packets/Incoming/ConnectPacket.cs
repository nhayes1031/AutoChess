using Lidgren.Network;
using System;

namespace Client.Game {
    public partial class ConnectPacket : IIncomingPacket {
        public Guid playerId;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            playerId = Guid.Parse(message.ReadString());
        }
    }
}
