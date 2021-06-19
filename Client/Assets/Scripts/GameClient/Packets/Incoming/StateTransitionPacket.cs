using Lidgren.Network;

namespace Client.Game {
    public class StateTransitionPacket : IIncomingPacket {
        public string Event;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Event = message.ReadString();
        }
    }
}
