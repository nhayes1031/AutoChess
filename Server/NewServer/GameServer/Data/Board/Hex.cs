namespace Server.Game {
    public class Hex {
        public HexCoords coords;
        public Character unit;

        public Hex (HexCoords coords) {
            this.coords = coords;
        }

        public bool Equals(Hex other) => coords == other.coords;
        public override bool Equals(object obj) => (obj is Hex other) && this.Equals(other);
        public override int GetHashCode() => coords.GetHashCode();
        public static bool operator ==(Hex a, Hex b) => a.Equals(b);
        public static bool operator !=(Hex a, Hex b) => !a.Equals(b);
    }
}
