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

            Manager.GameClient.UnitSoldFromBoard += HandleUnitSold;
            Manager.GameClient.UnitMovedFromBenchToBoard += HandleUnitMoved;
            Manager.GameClient.SimulationUnitMoved += HandleSimulationUnitMoved;
            Manager.GameClient.SimulationUnitAttacked += HandleSimulationUnitAttacked;
            Manager.GameClient.SimulationCombatStarted += HandleSimulationCombatStarted;
            Manager.GameClient.SimulationEndedInDraw += ResetBoard;
            Manager.GameClient.SimulationEndedInVictory += ResetBoard;
            Manager.GameClient.SimulationEndedInLoss += ResetBoard;
            Manager.GameClient.SimulationUnitDied += HandleSimulationUnitDied;
        }

        private void OnDestroy() {
            Manager.GameClient.UnitSoldFromBoard -= HandleUnitSold;
            Manager.GameClient.UnitMovedFromBenchToBoard -= HandleUnitMoved;
            Manager.GameClient.SimulationUnitMoved -= HandleSimulationUnitMoved;
            Manager.GameClient.SimulationUnitAttacked -= HandleSimulationUnitAttacked;
            Manager.GameClient.SimulationCombatStarted -= HandleSimulationCombatStarted;
            Manager.GameClient.SimulationEndedInDraw -= ResetBoard;
            Manager.GameClient.SimulationEndedInVictory -= ResetBoard;
            Manager.GameClient.SimulationEndedInLoss -= ResetBoard;
            Manager.GameClient.SimulationUnitDied -= HandleSimulationUnitDied;
        }

        private void HandleSimulationUnitDied(SimulationUnitDiedPacket packet) {
            var offset = packet.Unit.ToOffset(flipCoords);
            grid[offset.y, offset.x].RemoveUnit();
        }

        private void HandleSimulationUnitMoved(SimulationUnitMovedPacket packet) {
            var fromOffset = packet.FromCoords.ToOffset(flipCoords);
            var toOffset = packet.ToCoords.ToOffset(flipCoords);

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

        private void HandleSimulationUnitAttacked(SimulationUnitAttackedPacket packet) {
            var attackerOffset = packet.Attacker.ToOffset(flipCoords);
            var defenderOffset = packet.Defender.ToOffset(flipCoords);

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

        private void HandleSimulationCombatStarted(SimulationCombatStartedPacket packet) {
            CreateSnapshot();

            ClearBoard();

            if (packet.bottomPlayer == Manager.GameClient.PlayerId) {
                flipCoords = false;
            } else {
                flipCoords = true;
            }

            foreach (var unit in packet.units) {
                var offsetCoords = unit.position.ToOffset(flipCoords);
                grid[offsetCoords.y, offsetCoords.x].RemoveUnit();
                grid[offsetCoords.y, offsetCoords.x].AddUnit(unit.name);
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

        private void HandleUnitSold(SellUnitFromBoardPacket packet) {
            var offsetCoords = packet.Coords.ToOffset();
            if (offsetCoords.x < 0 || offsetCoords.x > WIDTH) return;
            if (offsetCoords.y < 0 || offsetCoords.y > HEIGHT) return;

            var hex = grid[offsetCoords.y, offsetCoords.x];
            if (hex.Coords == packet.Coords) {
                hex.RemoveUnit();
            }
        }

        private void HandleUnitMoved(MoveToBoardFromBenchPacket packet) {
            var offsetCoords = packet.ToCoords.ToOffset();
            if (offsetCoords.x < 0 || offsetCoords.x > WIDTH) return;
            if (offsetCoords.y < 0 || offsetCoords.y > HEIGHT) return;

            var hex = grid[offsetCoords.y, offsetCoords.x];
            hex.AddUnit(packet.Character);
        }
    }
}
