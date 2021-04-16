using Lidgren.Network;
using PubSub;
using Server.Game.Messages;

namespace Server.Game {
    public class MessageHub {
        private NetServer server;
        private Hub hub = Hub.Default;

        public MessageHub(NetServer server) {
            this.server = server;

            hub.Subscribe<SimulationUnitMoved>(this, SendUnitMovedPacket);
        }

        private void SendUnitMovedPacket(SimulationUnitMoved e) {
            NetOutgoingMessage message;
            foreach (var connection in e.connections) {
                message = server.CreateMessage();
                new SimulationUnitMovedPacket() {
                    FromCoords = e.fromCoords,
                    ToCoords = e.toCoords
                }.PacketToNetOutgoingMessage(message);

                server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
            }
        }
    }
}
