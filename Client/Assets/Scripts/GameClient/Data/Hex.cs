namespace Client.Game {
    public class Hex {
        public HexCoords coords;
        public string unit;

        public Hex (HexCoords coords) {
            this.coords = coords;
            unit = "";
        }

        public bool Equals(Hex other) => coords == other.coords;
        public override bool Equals(object obj) => (obj is Hex other) && this.Equals(other);
        public override int GetHashCode() => coords.GetHashCode();
        public static bool operator ==(Hex a, Hex b) => a.Equals(b);
        public static bool operator !=(Hex a, Hex b) => !a.Equals(b);
    }
}
