using Lidgren.Network;

namespace Server.Game {
    public class CombatEndedInVictoryPacket : IOutgoingPacket {
        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.CombatEndedInVictory);
        }
    }
}