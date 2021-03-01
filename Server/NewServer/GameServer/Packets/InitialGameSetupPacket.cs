using Lidgren.Network;
using System.Collections.Generic;

namespace Server.Game {
    public class InitialGameSetupPacket : Packet {
        public IEnumerable<int> PlayerPorts { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.InitialGameSetup);
            foreach (var port in PlayerPorts) {
                message.Write(port);
            }
        }
    }

}
