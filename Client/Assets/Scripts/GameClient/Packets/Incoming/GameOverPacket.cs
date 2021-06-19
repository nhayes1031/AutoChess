using Lidgren.Network;
using System;

namespace Client.Game {
    public class GameOverPacket : IIncomingPacket {
        public Guid Winner;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Winner = Guid.Parse(message.ReadString());
        }
    }
}

