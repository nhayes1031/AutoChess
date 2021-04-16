using Lidgren.Network;
using Server.Game.Systems;

namespace Server.Game {
    public class SellUnitFromBoardPacket : Packet {
        public string Name { get; set; }
        public HexCoords Coords { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SellUnitFromBoard);
            message.Write(Name);
            message.Write(Coords.q);
            message.Write(Coords.r);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
            Coords = new HexCoords(
                message.ReadInt32(),
                message.ReadInt32()
            );
        }
    }
}
