using Lidgren.Network;
using Server.Game.Systems;

namespace Server.Game {
    public class RequestMoveUnitPacket : IIncomingPacket {
        public string Name;
        public ILocation From;
        public ILocation To;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
            var fromLocationType = message.ReadByte();
            if ((LocationTypes)fromLocationType == LocationTypes.Bench) {
                From = new BenchLocation() {
                    seat = message.ReadInt32()
                };
            }
            else {
                From = new BoardLocation() {
                    coords = new HexCoords(
                        message.ReadInt32(),
                        message.ReadInt32()
                    )
                };
            }

            var toLocationType = message.ReadByte();
            if ((LocationTypes)toLocationType == LocationTypes.Bench) {
                To = new BenchLocation() {
                    seat = message.ReadInt32()
                };
            }
            else {
                To = new BoardLocation() {
                    coords = new HexCoords(
                        message.ReadInt32(),
                        message.ReadInt32()
                    )
                };
            }
        }
    }
}
