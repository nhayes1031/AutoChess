using Lidgren.Network;

namespace Client.Game {
    public class RequestRerollPacket : IOutgoingPacket {
        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.RequestReroll);
        }
    }
}
