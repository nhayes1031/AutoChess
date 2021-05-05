using Lidgren.Network;
using Server.Game.Systems;

namespace Server.Game {
    public class RequestUnitSellPacket : IIncomingPacket {
        public string Name;
        public ILocation Location;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
            var locationType = message.ReadByte();
            if ((LocationTypes)locationType == LocationTypes.Bench) {
                Location = new BenchLocation() {
                    seat = message.ReadInt32()
                };
            }
            else {
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
