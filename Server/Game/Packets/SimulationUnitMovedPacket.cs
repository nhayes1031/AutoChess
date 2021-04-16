using Lidgren.Network;
using Server.Game.Systems;

namespace Server.Game {
    public class SimulationUnitMovedPacket : Packet {
        public HexCoords FromCoords;
        public HexCoords ToCoords;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SimulationUnitMoved);
            message.Write(FromCoords.q);
            message.Write(FromCoords.r);
            message.Write(ToCoords.q);
            message.Write(ToCoords.r);
        }
    }
}
