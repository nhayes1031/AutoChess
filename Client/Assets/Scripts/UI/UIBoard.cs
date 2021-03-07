using Client.Game;
using System;
using UnityEngine;

namespace Client.UI {
    public class UIBoard : MonoBehaviour {
        private const int HEIGHT = 4;
        private const int WIDTH = 8;

        public Action<Hex> HexSelected;

        [SerializeField] private GameObject hexPrefab = null;

        private UIHex[,] grid;

        public void Start() {
            grid = new UIHex[HEIGHT, WIDTH];

            for (int r = 0; r < HEIGHT; r++) {
                int r_offset = (int)Mathf.Floor(r / 2);
                for (int q = -r_offset; q < WIDTH - r_offset; q++) {
                    var axialCoords = new HexCoords(q, r);
                    var offsetCoords = axialCoords.ToOffset();
                    var hex = CreateHex(axialCoords);

                    grid[offsetCoords.y, offsetCoords.x] = hex;
                }
            }

            StaticManager.GameClient.UnitSoldFromBoard += HandleUnitSold;
            StaticManager.GameClient.UnitMovedFromBenchToBoard += HandleUnitMoved;
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
