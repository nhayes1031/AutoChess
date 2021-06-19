using Lidgren.Network;

namespace Client.Game {
    public class UnitAttackedPacket : IIncomingPacket {
        public BoardLocation Attacker;
        public BoardLocation Defender;
        public int Damage;

        public void NetIncomingMessageToPacket(NetIncomingMessage message) {
            Attacker = new BoardLocation() {
                coords = new HexCoords(
                    message.ReadInt32(),
                    message.ReadInt32()
                )
            };
            Defender = new BoardLocation() {
                coords = new HexCoords(
                    message.ReadInt32(),
                    message.ReadInt32()
                )
            };
            Damage = message.ReadInt32();
        }
    }
}
