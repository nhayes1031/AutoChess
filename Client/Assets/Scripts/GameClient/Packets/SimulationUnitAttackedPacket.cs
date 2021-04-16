using Lidgren.Network;

namespace Client.Game {
    public class SimulationUnitAttackedPacket : Packet {
        public HexCoords Attacker;
        public HexCoords Defender;
        public int Damage;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Attacker = new HexCoords(message.ReadInt32(), message.ReadInt32());
            Defender = new HexCoords(message.ReadInt32(), message.ReadInt32());
            Damage = message.ReadInt32();
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
        }
    }
}
