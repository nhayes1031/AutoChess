using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Client.Game {
    public class UnitLeveledUpPacket : IIncomingPacket {
        public List<Tuple<string, ILocation>> UnitsToRemove;
        public string Name;
        public ILocation Location;
        public int StarLevel;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
            StarLevel = message.ReadInt32();

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

            var count = message.ReadInt32();
            for (int i = 0; i < count; i++) {
                var name = message.ReadString();
                locationType = message.ReadByte();
                ILocation location = null;
                if ((LocationTypes)locationType == LocationTypes.Bench) {
                    location = new BenchLocation {
                        seat = message.ReadInt32()
                    };
                } else {
                    location = new BoardLocation() {
                        coords = new HexCoords(
                            message.ReadInt32(),
                            message.ReadInt32()
                        )
                    };
                }
                UnitsToRemove.Add(
                    new Tuple<string, ILocation>(
                        name,
                        location
                    )
                );
            }
        }
    }
}
