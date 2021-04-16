using Server.Game.EC;
using Server.Game.EC.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Systems {
    public class Board {
        private Hex[,] grid;

        private int height;
        private int width;

        public Board(List<Entity> entities1, List<Entity> entities2) {
            InitializeGrid(Constants.BOARD_HEIGHT * 2, Constants.BOARD_WIDTH);

            foreach (var entity in entities1) {
                var location = entity.GetComponent<LocationComponent>();
                var offsetCoords = location.StartingCoords.ToOffset();
                grid[offsetCoords.y, offsetCoords.x].unit = entity;
                entity.GetComponent<MovementComponent>().position = grid[offsetCoords.y, offsetCoords.x];
            }

            foreach (var entity in entities2) {
                var location = entity.GetComponent<LocationComponent>();
                var offsetCoords = location.StartingCoords.ToOffset();
                var y = Constants.BOARD_HEIGHT * 2 - 1 - offsetCoords.y;
                var x = Constants.BOARD_WIDTH - offsetCoords.x - 1;
                grid[y, x].unit = entity;
                entity.GetComponent<MovementComponent>().position = grid[y, x];
            }
        }

        private void InitializeGrid(int height, int width) {
            this.height = height;
            this.width = width;
            grid = new Hex[height, width];
            for (int r = 0; r < height; r++) {
                int r_offset = (int)MathF.Floor(r / 2);
                for (int q = -r_offset; q < width - r_offset; q++) {
                    var axialCoords = new HexCoords(q, r);
                    var offsetCoords = axialCoords.ToOffset();
                    var hex = new Hex(axialCoords);
                    grid[offsetCoords.y, offsetCoords.x] = hex;
                }
            }
        }

        public bool Contains(Entity entity, HexCoords coords) {
            var offset = coords.ToOffset();
            if (!Contains(coords))
                return false;

            if (grid[offset.y, offset.x].unit == entity) {
                return true;
            }
            return false;
        }

        public bool Contains(HexCoords coords) {
            return AllowedCoordinates.Contains(coords);
        }

        public Entity RemoveUnit(HexCoords coords) {
            var offset = coords.ToOffset();
            if (!Contains(coords))
                return null;

            var entity = grid[offset.y, offset.x].unit;
            if (!(entity is null)) {
                grid[offset.y, offset.x].unit = null;
            }
            return entity;
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

        public bool IsHexLocked(HexCoords coords) {
            var offset = coords.ToOffset();
            if (!Contains(coords))
                return false;

            return grid[offset.y, offset.x].locked;
        }

        public void MoveUnit(HexCoords from, HexCoords to) {
            if (!Contains(from) && !Contains(to))
                return;

            if (IsHexEmpty(from) || !IsHexEmpty(to)) {
                return;
            }

            var entity = RemoveUnit(from);
            AddUnit(entity, to);
        }

        public void AddUnit(Entity entity, HexCoords coords) {
            var offset = coords.ToOffset();
            if (!Contains(coords))
                return;

            if (!IsHexEmpty(coords))
                return;

            grid[offset.y, offset.x].unit = entity;
        }

        public List<Entity> GetUnits() {
            var units = new List<Entity>();
            foreach (var hex in grid) {
                if (!(hex.unit is null)) {
                    units.Add(hex.unit);
                }
            }
            return units;
        }

        public Hex GetHexInDirectionOfClosestEnemy(Entity entity) {
            var team = entity.GetComponent<TeamComponent>().Team;
            var location = entity.GetComponent<MovementComponent>().position;
            var enemyUnits = GetUnits().Where(x => x.GetComponent<TeamComponent>().Team != team);

            Hex enemyHex = null;
            var minDistance = 99;
            foreach (var unit in enemyUnits) {
                var hex = unit.GetComponent<MovementComponent>().position;
                var newDistance = location.Distance(hex);
                if (newDistance < minDistance) {
                    minDistance = newDistance;
                    enemyHex = hex;
                }
            }

            return GetHexInDirectionOf(location, enemyHex);
        }

        public Hex GetHexInDirectionOf(Hex from, Hex to) {
            HexCoords closetCoords = null;
            var minDistance = 99;
            foreach (var direction in HexCoords.directions) {
                var dirCoords = from.coords + direction;
                if (IsHexEmpty(dirCoords) && !IsHexLocked(dirCoords)) {
                    var newDistance = HexCoords.Direction(dirCoords, to.coords);
                    if (newDistance < minDistance) {
                        minDistance = newDistance;
                        closetCoords = dirCoords;
                    }
                }
            }
            if (!(closetCoords is null)) {
                var offsetCoords = closetCoords.ToOffset();
                return grid[offsetCoords.y, offsetCoords.x];
            }
            return null;
        }

        public override string ToString() {
            var str = "";
            var ctr = 0;
            foreach (var hex in grid) {
                var offsetCoords = hex.coords.ToOffset();
                str += $"Axial: [{hex.coords.q}, {hex.coords.r}] Offset: [{offsetCoords.x}, {offsetCoords.y}] Unit: {(hex.unit is null ? "" : hex.unit)}      ";
                ctr++;
                if (ctr % 8 == 0) {
                    str += "\n";
                }
            }
            return str;
        }

        private readonly HashSet<HexCoords> AllowedCoordinates = new HashSet<HexCoords>() {
            new HexCoords(0, 0),
            new HexCoords(1, 0),
            new HexCoords(2, 0),
            new HexCoords(3, 0),
            new HexCoords(4, 0),
            new HexCoords(5, 0),
            new HexCoords(6, 0),
            new HexCoords(7, 0),
            new HexCoords(0, 1),
            new HexCoords(1, 1),
            new HexCoords(2, 1),
            new HexCoords(3, 1),
            new HexCoords(4, 1),
            new HexCoords(5, 1),
            new HexCoords(6, 1),
            new HexCoords(7, 1),
            new HexCoords(-1, 2),
            new HexCoords(0, 2),
            new HexCoords(1, 2),
            new HexCoords(2, 2),
            new HexCoords(3, 2),
            new HexCoords(4, 2),
            new HexCoords(5, 2),
            new HexCoords(6, 2),
            new HexCoords(-1, 3),
            new HexCoords(0, 3),
            new HexCoords(1, 3),
            new HexCoords(2, 3),
            new HexCoords(3, 3),
            new HexCoords(4, 3),
            new HexCoords(5, 3),
            new HexCoords(6, 3),
            new HexCoords(-2, 4),
            new HexCoords(-1, 4),
            new HexCoords(0, 4),
            new HexCoords(1, 4),
            new HexCoords(2, 4),
            new HexCoords(3, 4),
            new HexCoords(4, 4),
            new HexCoords(5, 4),
            new HexCoords(-2, 5),
            new HexCoords(-1, 5),
            new HexCoords(0, 5),
            new HexCoords(1, 5),
            new HexCoords(2, 5),
            new HexCoords(3, 5),
            new HexCoords(4, 5),
            new HexCoords(5, 5),
            new HexCoords(-3, 6),
            new HexCoords(-2, 6),
            new HexCoords(-1, 6),
            new HexCoords(0, 6),
            new HexCoords(1, 6),
            new HexCoords(2, 6),
            new HexCoords(3, 6),
            new HexCoords(4, 6),
            new HexCoords(-3, 7),
            new HexCoords(-2, 7),
            new HexCoords(-1, 7),
            new HexCoords(0, 7),
            new HexCoords(1, 7),
            new HexCoords(2, 7),
            new HexCoords(3, 7),
            new HexCoords(4, 7)
        };
    }
}