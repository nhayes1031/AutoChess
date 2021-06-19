using Lidgren.Network;
using System;

namespace Client.Game {
    public class PlayerDiedPacket : IIncomingPacket {
        public Guid Player;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Player = Guid.Parse(message.ReadString());
        }
    }
}
