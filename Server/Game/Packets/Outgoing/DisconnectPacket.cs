using Lidgren.Network;

namespace Server.Game {
    public partial class DisconnectPacket : IOutgoingPacket {
        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.Disconnect);
        }
    }
}