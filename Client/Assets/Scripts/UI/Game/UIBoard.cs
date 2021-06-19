using Client.Game;
using System;
using UnityEngine;

namespace Client.UI {
    public class UIBoard : MonoBehaviour {
        private const int HEIGHT = 8;
        private const int WIDTH = 8;

        public Action<Hex> HexSelected;

        [SerializeField] private  GameObject hexPrefab = null;
        [SerializeField] private  GameObject attackPrefab = null;
        [SerializeField] private  GameObject movePrefab = null;

        private UIHex[,] grid;
        private string[,] snapshot;
        private bool flipCoords;

        private void Start() {
            grid = new UIHex[HEIGHT, WIDTH];
            snapshot = new string[HEIGHT, WIDTH];

            for (int r = 0; r < HEIGHT; r++) {
                int r_offset = (int)Mathf.Floor(r / 2);
                for (int q = -r_offset; q < WIDTH - r_offset; q++) {
                    var axialCoords = new HexCoords(q, r);
                    var offsetCoords = axialCoords.ToOffset();
                    var hex = CreateHex(axialCoords);

                    grid[offsetCoords.y, offsetCoords.x] = hex;
                }
            }

            Manager.GameClient.UnitSold += HandleUnitSold;
            Manager.GameClient.UnitRepositioned += HandleUnitRepositioned;
            Manager.GameClient.UnitMoved += HandleUnitMoved;
            Manager.GameClient.UnitAttacked += HandleUnitAttacked;
            Manager.GameClient.CombatStarted += HandleCombatStarted;
            Manager.GameClient.CombatEndedInDraw += ResetBoard;
            Manager.GameClient.CombatEndedInVictory += ResetBoard;
            Manager.GameClient.CombatEndedInLoss += ResetBoard;
            Manager.GameClient.UnitDied += HandleUnitDied;
        }

        private void OnDestroy() {
            Manager.GameClient.UnitSold -= HandleUnitSold;
            Manager.GameClient.UnitRepositioned -= HandleUnitRepositioned;
            Manager.GameClient.UnitMoved -= HandleUnitMoved;
            Manager.GameClient.UnitAttacked -= HandleUnitAttacked;
            Manager.GameClient.CombatStarted -= HandleCombatStarted;
            Manager.GameClient.CombatEndedInDraw -= ResetBoard;
            Manager.GameClient.CombatEndedInVictory -= ResetBoard;
            Manager.GameClient.CombatEndedInLoss -= ResetBoard;
            Manager.GameClient.UnitDied -= HandleUnitDied;
        }

        private void HandleUnitDied(UnitDiedPacket packet) {
            var offset = packet.Location.coords.ToOffset(flipCoords);
            grid[offset.y, offset.x].RemoveUnit();
        }

        private void HandleUnitMoved(UnitMovedPacket packet) {
            var fromOffset = packet.FromCoords.coords.ToOffset(flipCoords);
            var toOffset = packet.ToCoords.coords.ToOffset(flipCoords);

            var unit = grid[fromOffset.y, fromOffset.x].RemoveUnit();
            grid[toOffset.y, toOffset.x].AddUnit(unit);

            SpawnMoveSprite(grid[fromOffset.y, fromOffset.x], grid[toOffset.y, toOffset.x]);
        }

        private void SpawnMoveSprite(UIHex origin, UIHex direction) {
            var sprite = Instantiate(movePrefab, origin.transform);

            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction.transform.position - sprite.transform.position);
            sprite.transform.rotation = rotation;

            Destroy(sprite, 0.25f);
        }

        private void HandleUnitAttacked(UnitAttackedPacket packet) {
            var attackerOffset = packet.Attacker.coords.ToOffset(flipCoords);
            var defenderOffset = packet.Defender.coords.ToOffset(flipCoords);

            SpawnAttackSprite(grid[attackerOffset.y, attackerOffset.x], grid[defenderOffset.y, defenderOffset.x]);
        }

        private void SpawnAttackSprite(UIHex attacker, UIHex defender) {
            var sprite = Instantiate(attackPrefab, attacker.transform);

            var position = new Vector3();

            position.x = attacker.transform.position.x + (defender.transform.position.x - attacker.transform.position.x) / 2;
            position.y = attacker.transform.position.y + (defender.transform.position.y - attacker.transform.position.y) / 2;

            sprite.transform.position = position;

            Destroy(sprite, 0.25f);
        }

        private void HandleCombatStarted(CombatStartedPacket packet) {
            CreateSnapshot();

            ClearBoard();

            if (packet.BottomPlayer == Manager.GameClient.PlayerId) {
                flipCoords = false;
            } else {
                flipCoords = true;
            }

            foreach (var unit in packet.Units) {
                var offsetCoords = unit.Item2.coords.ToOffset(flipCoords);
                grid[offsetCoords.y, offsetCoords.x].RemoveUnit();
                grid[offsetCoords.y, offsetCoords.x].AddUnit(unit.Item1);
            }
        }

        private void ClearBoard() {
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    grid[i, j].RemoveUnit();
                }
            }
        }

        private void CreateSnapshot() {
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    snapshot[i, j] = grid[i, j].Unit;
                }
            }
        }

        private void ResetBoard() {
            ClearBoard();
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    var unit = snapshot[i, j];
                    if (unit != "") {
                        grid[i, j].AddUnit(unit);
                    }
                }
            }
        }

        private UIHex CreateHex(HexCoords coords) {
            var hex = Instantiate(hexPrefab, transform).GetComponent<UIHex>();
            hex.Initialize(coords);

            hex.HexSelected += HandleHexSelected;
            return hex;
        }

        private void HandleHexSelected(Hex hex) {
            HexSelected?.Invoke(hex);
        }

        private void HandleUnitSold(UnitSoldPacket packet) {
            if (packet.Location is BoardLocation) {
                var location = (BoardLocation)packet.Location;
                var offsetCoords = location.coords.ToOffset();
                if (offsetCoords.x < 0 || offsetCoords.x > WIDTH) return;
                if (offsetCoords.y < 0 || offsetCoords.y > HEIGHT) return;

                var hex = grid[offsetCoords.y, offsetCoords.x];
                if (hex.Coords == location.coords) {
                    hex.RemoveUnit();
                }
            }
        }

        private void HandleUnitRepositioned(UnitRepositionedPacket packet) {
            if (packet.FromLocation is BoardLocation) {
                var boardFromLocation = (BoardLocation)packet.FromLocation;
                var offsetCoords = boardFromLocation.coords.ToOffset();
                if (offsetCoords.x < 0 || offsetCoords.x > WIDTH) return;
                if (offsetCoords.y < 0 || offsetCoords.y > HEIGHT) return;

                var hex = grid[offsetCoords.y, offsetCoords.x];
                if (hex.Unit == packet.Name)
                    hex.RemoveUnit();
            }

            if (packet.ToLocation is BoardLocation) {
                var boardToLocation = (BoardLocation)packet.ToLocation;
                var offsetCoords = boardToLocation.coords.ToOffset();
                if (offsetCoords.x < 0 || offsetCoords.x > WIDTH) return;
                if (offsetCoords.y < 0 || offsetCoords.y > HEIGHT) return;

                var hex = grid[offsetCoords.y, offsetCoords.x];
                if (String.IsNullOrEmpty(hex.Unit))
                    hex.AddUnit(packet.Name);
            }
        }
    }
}
