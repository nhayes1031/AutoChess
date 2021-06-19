using Lidgren.Network;

namespace Client.Game {
    public interface IIncomingPacket {
        void NetIncomingMessageToPacket(NetIncomingMessage message);
    }

    public interface IOutgoingPacket {
        void PacketToNetOutgoingMessage(NetOutgoingMessage message);
    }
}
