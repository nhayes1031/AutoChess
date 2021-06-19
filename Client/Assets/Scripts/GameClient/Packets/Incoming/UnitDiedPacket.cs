using Lidgren.Network;

namespace Client.Game {
    public class UnitDiedPacket : IIncomingPacket {
        public BoardLocation Location;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Location = new BoardLocation() {
                coords = new HexCoords(
                    message.ReadInt32(),
                    message.ReadInt32()
                )
            };
        }
    }
}
