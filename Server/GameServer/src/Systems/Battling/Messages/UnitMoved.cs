using Server.Game.EC;

namespace Server.Game.Systems {
    public struct UnitMoved {
        public HexCoords fromCoords;
        public HexCoords toCoords;
        public Entity entity;
    }
}
