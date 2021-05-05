using Lidgren.Network;

namespace Server.Game {
    public class UnitAttackPacket : IOutgoingPacket {
        public BoardLocation Attacker;
        public BoardLocation Defender;
        public int Damage;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.UnitAttacked);
            message.Write(Attacker.coords.q);
            message.Write(Attacker.coords.r);
            message.Write(Defender.coords.q);
            message.Write(Defender.coords.r);
            message.Write(Damage);
        }
    }
}