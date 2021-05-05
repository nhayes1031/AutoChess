using Lidgren.Network;

namespace Server.Game {
    public class CombatEndedInDrawPacket : IOutgoingPacket {
        public int Damage;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.CombatEndedInDraw);
            message.Write(Damage);
        }
    }
}