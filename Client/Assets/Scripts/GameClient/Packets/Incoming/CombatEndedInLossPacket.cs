using Lidgren.Network;

namespace Client.Game {
    public class CombatEndedInLossPacket : IIncomingPacket {
        public int Damage;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Damage = message.ReadInt32();
        }
    }
}
