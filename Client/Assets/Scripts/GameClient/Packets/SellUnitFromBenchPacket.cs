﻿using Lidgren.Network;

namespace Client.Game {
    public class SellUnitFromBenchPacket : Packet {
        public string Name { get; set; }
        public int Seat { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.SellUnitFromBench);
            message.Write(Name);
            message.Write(Seat);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
            Seat = message.ReadInt32();
        }
    }
}