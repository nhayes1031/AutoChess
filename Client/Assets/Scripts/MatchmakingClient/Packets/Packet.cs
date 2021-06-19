using Lidgren.Network;

namespace Client {

    public interface IPacket {
        void PacketToNetOutgoingMessage(NetOutgoingMessage message);
        void NetIncomingMessageToPacket(NetIncomingMessage message);
    }

    public abstract class Packet : IPacket {
        public abstract void PacketToNetOutgoingMessage(NetOutgoingMessage message);
        public abstract void NetIncomingMessageToPacket(NetIncomingMessage message);
    }
}
