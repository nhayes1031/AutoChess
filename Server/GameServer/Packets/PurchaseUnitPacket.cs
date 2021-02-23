using Lidgren.Network;

namespace Server.Game {
    public class PurchaseUnitPacket : Packet {
        public string Name { get; set; }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.PurchaseUnit);
            message.Write(Name);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
        }
    }
}
