using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Client.Game {
    public class CombatStartedPacket : IIncomingPacket {
        public Guid BottomPlayer;
        public Guid TopPlayer;
        public List<Tuple<string, BoardLocation>> Units;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            BottomPlayer = Guid.Parse(message.ReadString());
            TopPlayer = Guid.Parse(message.ReadString());
            Units = new List<Tuple<string, BoardLocation>>();
            var count = message.ReadInt32();
            for (int i = 0; i < count; i++)
                Units.Add(
                    new Tuple<string, BoardLocation>(
                        message.ReadString(),
                        new BoardLocation() {
                            coords = new HexCoords(
                                message.ReadInt32(),
                                message.ReadInt32()
                            )
                        }
                    )
                );
        }
    }
}
