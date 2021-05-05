using Lidgren.Network;

namespace Server.Game {
    public class CombatEndedInLossPacket : IOutgoingPacket {
        public int Damage;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.CombatEndedInLoss);
            message.Write(Damage);
        }
    }
}