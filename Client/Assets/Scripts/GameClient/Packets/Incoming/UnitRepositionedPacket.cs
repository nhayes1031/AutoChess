using Lidgren.Network;

namespace Client.Game {
    public class UnitRepositionedPacket : IIncomingPacket {
        public string Name;
        public ILocation FromLocation;
        public ILocation ToLocation;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
            var fromLocationType = message.ReadByte();
            if ((LocationTypes)fromLocationType == LocationTypes.Bench) {
                FromLocation = new BenchLocation {
                    seat = message.ReadInt32()
                };
            } else {
                FromLocation = new BoardLocation() {
                    coords = new HexCoords(
                        message.ReadInt32(),
                        message.ReadInt32()
                    )
                };
            }

            var toLocationType = message.ReadByte();
            if ((LocationTypes)toLocationType == LocationTypes.Bench) {
                ToLocation = new BenchLocation {
                    seat = message.ReadInt32()
                };
            } else {
                ToLocation = new BoardLocation() {
                    coords = new HexCoords(
                        message.ReadInt32(),
                        message.ReadInt32()
                    )
                };
            }
        }
    }
}
