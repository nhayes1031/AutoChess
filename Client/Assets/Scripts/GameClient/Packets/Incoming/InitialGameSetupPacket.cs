using Lidgren.Network;
using System.Collections.Generic;

namespace Client.Game {
    public class InitialGameSetupPacket : IIncomingPacket {
        public List<int> PlayerPorts;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            PlayerPorts = new List<int>();
            var count = message.ReadInt32();
            for (int i = 0; i < count; i++)
                PlayerPorts.Add(message.ReadInt32());
        }
    }
}
