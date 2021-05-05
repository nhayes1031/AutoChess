using Lidgren.Network;

namespace Server.Game {
    public class StateTransitionPacket : IOutgoingPacket {
        public string Event;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.StateTransition);
            message.Write(Event);
        }
    }
}