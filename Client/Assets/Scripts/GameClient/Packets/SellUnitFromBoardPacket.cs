using Lidgren.Network;

namespace Client.Game {
    public class SellUnitFromBoardPacket : Packet {
        public string Name { get; set; }
        public HexCoords Coords { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SellUnitFromBoard);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {

        }
    }
}
