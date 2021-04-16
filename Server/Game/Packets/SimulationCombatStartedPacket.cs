using Lidgren.Network;
using Server.Game.EC;
using Server.Game.EC.Components;
using System;
using System.Collections.Generic;

namespace Server.Game {
    public class SimulationCombatStartedPacket : Packet {
        public Guid bottomPlayer;
        public Guid topPlayer;
        public List<Entity> units;
        
        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {}

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SimulationCombatStarted);
            message.Write(bottomPlayer.ToString());
            message.Write(topPlayer.ToString());
            message.Write(units.Count);
            foreach (var unit in units) {
                var location = unit.GetComponent<MovementComponent>().position.coords;
                message.Write(location.q);
                message.Write(location.r);

                var name = unit.GetComponent<StatsComponent>().Name;
                message.Write(name);
            }
        }
    }
}
