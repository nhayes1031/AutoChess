using Lidgren.Network;
using System.Collections.Generic;

namespace Client.Game {
    public class InitialGameSetupPacket : Packet {
        public List<int> PlayerPorts { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            PlayerPorts = new List<int>();
            for (int i = 0; i < 2; i++)
                PlayerPorts.Add(message.ReadInt32());
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.InitialGameSetup);
        }
    }
}
