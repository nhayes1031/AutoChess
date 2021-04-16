using Lidgren.Network;
using Server.Game.Systems;

namespace Server.Game {
    public class SimulationUnitAttackedPacket : Packet {
        public HexCoords Attacker;
        public HexCoords Defender;
        public int Damage;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SimulationUnitAttacked);
            message.Write(Attacker.q);
            message.Write(Attacker.r);
            message.Write(Defender.q);
            message.Write(Defender.r);
            message.Write(Damage);
        }
    }
}
