using Lidgren.Network;

namespace Server.Game {
    public interface IIncomingPacket {
        void NetIncomingMessageToPacket(NetIncomingMessage message);
    }

   
    public interface IOutgoingPacket {
        void PacketToNetOutgoingMessage(NetOutgoingMessage message);
    }
}
