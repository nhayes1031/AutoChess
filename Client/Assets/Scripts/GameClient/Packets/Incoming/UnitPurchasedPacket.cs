using Lidgren.Network;

namespace Client.Game {
    public class UnitPurchasedPacket : IIncomingPacket {
        public int ShopIndex;
        public string Name;
        public ILocation Location;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            ShopIndex = message.ReadInt32();
            Name = message.ReadString();
            var locationType = message.ReadByte();
            if ((LocationTypes)locationType == LocationTypes.Bench) {
                Location = new BenchLocation {
                    seat = message.ReadInt32()
                };
            } else {
                Location = new BoardLocation() {
                    coords = new HexCoords(
                        message.ReadInt32(),
                        message.ReadInt32()
                    )
                };
            }
        }
    }
}
