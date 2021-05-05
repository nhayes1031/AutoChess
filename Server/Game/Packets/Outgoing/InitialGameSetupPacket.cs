using Lidgren.Network;
using System.Collections.Generic;

namespace Server.Game {
    public class InitialGameSetupPacket : IOutgoingPacket {
        public List<int> PlayerPorts;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.InitialGameSetup);
            message.Write(PlayerPorts.Count);
            foreach (var port in PlayerPorts) {
                message.Write(port);
            }
        }
    }
}