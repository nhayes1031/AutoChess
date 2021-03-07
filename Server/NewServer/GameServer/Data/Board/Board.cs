using System;

namespace Server.Game {
    /// <summary>
    /// Represents the 4 x 8 board a player has
    /// </summary>
    public class Board {
        private const int HEIGHT = 4;
        private const int WIDTH = 8;

        private Hex[,] grid;

        public Board() {
            grid = new Hex[HEIGHT, WIDTH];
            for (int r = 0; r < HEIGHT; r++) {
                int r_offset = (int) MathF.Floor(r / 2);
                for (int q = -r_offset; q < WIDTH - r_offset; q++) {
                    var axialCoords = new HexCoords(q, r);
                    var offsetCoords = axialCoords.ToOffset();
                    var hex = new Hex(axialCoords);
                    grid[offsetCoords.y, offsetCoords.x] = hex;
                }
            }
        }

        public bool Contains(Character character, HexCoords coords) {
            var offset = coords.ToOffset();
            if (!Contains(coords))
                return false;

            if (grid[offset.y, offset.x].unit == character) {
                return true;
            }
            return false;
        }

        public bool Contains(HexCoords coords) {
            var offset = coords.ToOffset();
            if (MathF.Abs(offset.x) >= WIDTH || MathF.Abs(offset.y) >= HEIGHT)
                return false;

            return true;
        }

        public Character RemoveUnit(HexCoords coords) {
            var offset = coords.ToOffset();
            if (!Contains(coords))
                return null;

            var character = grid[offset.y, offset.x].unit;
            if (!(character is null)) {
                grid[offset.y, offset.x].unit = null;
            }
            return character;
        }

        public bool IsHexEmpty(HexCoords coords) {
            var offset = coords.ToOffset();
            if (!Contains(coords))
                return false;

            if (grid[offset.y, offset.x].unit is null) {
                return true;
            }

            return false;
        }

        public void MoveUnit(HexCoords from, HexCoords to) {
            if (!Contains(from) && !Contains(to))
                return;

            if (IsHexEmpty(from) || !IsHexEmpty(to)) {
                return;
            }

            var character = RemoveUnit(from);
            AddUnit(character, to);
        }

        public void AddUnit(Character unit, HexCoords coords) {
            var offset = coords.ToOffset();
            if (!Contains(coords))
                return;

            if (!IsHexEmpty(coords))
                return;

            grid[offset.y, offset.x].unit = unit;
        }
    }
}
