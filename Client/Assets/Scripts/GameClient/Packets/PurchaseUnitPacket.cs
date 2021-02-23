using Lidgren.Network;

namespace Client.Game {
    public class PurchaseUnitPacket : Packet {
        public string Name;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Name = message.ReadString();
        }

        public override void PacketToNetOutgoingMessage(NetOutgoingMessage message) {
            message.Write((byte)PacketTypes.PurchaseUnit);
            message.Write(Name);
        }
    }
}
