using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Server.Game {
    public class UnitLeveledUpPacket : IOutgoingPacket {
        public List<Tuple<string, ILocation>> UnitsToRemove;
        public string Name;
        public ILocation Location;
        public int StarLevel;

        public void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)OutgoingPacketTypes.UnitLeveledUp);
            message.Write(Name);
            message.Write(StarLevel);
            if (Location is BenchLocation benchLocation) {
                message.Write((byte)LocationTypes.Bench);
                message.Write(benchLocation.seat);
            } else if (Location is BoardLocation boardLocation) {
                message.Write((byte)LocationTypes.Board);
                message.Write(boardLocation.coords.q);
                message.Write(boardLocation.coords.r);
            }
            message.Write(UnitsToRemove.Count);
            foreach (var tuple in UnitsToRemove) {
                message.Write(tuple.Item1);
                if (tuple.Item2 is BenchLocation benchLoc) {
                    message.Write((byte)LocationTypes.Bench);
                    message.Write(benchLoc.seat);
                } else if (tuple.Item2 is BoardLocation boardLoc) {
                    message.Write((byte)LocationTypes.Board);
                    message.Write(boardLoc.coords.q);
                    message.Write(boardLoc.coords.r);
                }
            }
        }
    }
}