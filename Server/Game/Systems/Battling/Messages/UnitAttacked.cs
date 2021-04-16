namespace Server.Game.Systems {
    public struct UnitAttacked {
        public HexCoords attacker;
        public HexCoords defender;
        public int damage;
    }
}
