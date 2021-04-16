using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Client.Game {
    public class SimulationCombatStartedPacket : Packet {
        public Guid bottomPlayer;
        public Guid topPlayer;
        public List<Entity> units;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            bottomPlayer = Guid.Parse(message.ReadString());
            topPlayer = Guid.Parse(message.ReadString());
            units = new List<Entity>();
            var numberOfUnits = message.ReadInt32();
            for (int i = 0; i < numberOfUnits; i++) {
                units.Add(new Entity() {
                    position = new HexCoords(message.ReadInt32(), message.ReadInt32()),
                    name = message.ReadString()
                });
            }
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) { }
    }
}
