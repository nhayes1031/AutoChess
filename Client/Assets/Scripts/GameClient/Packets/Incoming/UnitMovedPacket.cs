using Lidgren.Network;

namespace Client.Game {
    public class UnitMovedPacket : IIncomingPacket {
        public BoardLocation FromCoords;
        public BoardLocation ToCoords;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            FromCoords = new BoardLocation() {
                coords = new HexCoords(
                    message.ReadInt32(),
                    message.ReadInt32()
                )
            };
            ToCoords = new BoardLocation() {
                coords = new HexCoords(
                    message.ReadInt32(),
                    message.ReadInt32()
                )
            };
        }
    }
}
