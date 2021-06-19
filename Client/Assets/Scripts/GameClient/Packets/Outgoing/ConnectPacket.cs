using Lidgren.Network;

namespace Client.Game {
    public partial class ConnectPacket : IOutgoingPacket {
        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.Connect);
        }
    }
}
