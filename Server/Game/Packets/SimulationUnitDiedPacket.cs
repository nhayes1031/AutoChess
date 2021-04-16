using Lidgren.Network;
using Server.Game.Systems;

namespace Server.Game {
    public class SimulationUnitDiedPacket : Packet {
        public HexCoords Unit;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) { }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SimulationUnitDied);
            message.Write(Unit.q);
            message.Write(Unit.r);
        }
    }
}
