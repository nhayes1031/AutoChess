using Server.Game.EC;

namespace Server.Game.Systems {
    public class Hex {
        public HexCoords coords;
        public Entity unit;
        public bool locked;

        public Hex (HexCoords coords) {
            this.coords = coords;
        }

        public void RemoveEntity() {
            unit = null;
        }

        public void AddEntity(Entity entity) {
            unit = entity;
        }

        public int Distance(Hex other) {
            return HexCoords.Distance(coords, other.coords);
        }

        public bool Equals(Hex other) => coords == other.coords;
        public override bool Equals(object obj) => (obj is Hex other) && this.Equals(other);
        public override int GetHashCode() => coords.GetHashCode();
        public static bool operator ==(Hex a, Hex b) => a.Equals(b);
        public static bool operator !=(Hex a, Hex b) => !a.Equals(b);
    }
}
