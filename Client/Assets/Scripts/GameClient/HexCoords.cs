using UnityEngine;

namespace Client.Game {
    /// <summary>
    /// Axial based tile coordinates
    /// </summary>
    public struct HexCoords {
        private static readonly HexCoords[] directions = new HexCoords[] {
            new HexCoords(1, -1),
            new HexCoords(1, 0),
            new HexCoords(0, 1),
            new HexCoords(-1, 1),
            new HexCoords(-1, 0),
            new HexCoords(0, -1)
        };
        public static HexCoords Zero => new HexCoords(0, 0);

        public readonly int q;
        public readonly int r;

        public HexCoords(int q, int r) {
            this.q = q;
            this.r = r;
        }

        public HexCoords Direction(HexDirection direction) {
            return directions[(int)direction];
        }

        public HexCoords Neighbor(HexDirection direction) {
            return this + Direction(direction);
        }

        public OffsetCoords ToOffset(bool flip = false) {
            if (flip) {
                var x = 7 - (q + (r - (r & 1)) / 2);
                var y = 7 - r;
                return new OffsetCoords(x, y);
            } else {
                var x = q + (r - (r & 1)) / 2;
                var y = r;
                return new OffsetCoords(x, y);
            }
        }

        public static bool operator == (HexCoords a, HexCoords b) => a.Equals(b);
        public static bool operator != (HexCoords a, HexCoords b) => !a.Equals(b);
        public static HexCoords operator + (HexCoords a, HexCoords b) => new HexCoords(a.q + b.q, a.r + b.r);
        public static HexCoords operator + (HexCoords a, int offset) => new HexCoords(a.q + offset, a.r + offset);
        public static HexCoords operator - (HexCoords a, HexCoords b) => new HexCoords(a.q - b.q, a.r - b.r);
        public static HexCoords operator - (HexCoords a, int offset) => new HexCoords(a.q - offset, a.r - offset);
        public static HexCoords operator * (HexCoords a, int offset) => new HexCoords(a.q * offset, a.r * offset);
        public override bool Equals(object other) => other is HexCoords hex && Equals(hex);
        public bool Equals(HexCoords other) => (q, r) == (other.q, other.r);
        public bool Equals(HexCoords a, HexCoords b) => a.Equals(b);
        public override int GetHashCode() => (q, r).GetHashCode();
        public int GetHashCode(HexCoords hex) => hex.GetHashCode();
        public override string ToString() => $"[{q},{r}]";
    }
}
