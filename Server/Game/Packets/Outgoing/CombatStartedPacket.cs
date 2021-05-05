using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Server.Game {
    public class CombatStartedPacket : IOutgoingPacket {
        public Guid BottomPlayer;
        public Guid TopPlayer;
        public List<Tuple<string, BoardLocation>> Units;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.CombatStarted);
            message.Write(BottomPlayer.ToString());
            message.Write(TopPlayer.ToString());
            foreach (var tuple in Units) {
                message.Write(tuple.Item1);
                message.Write(tuple.Item2.coords.q);
                message.Write(tuple.Item2.coords.r);
            }
        }
    }
}