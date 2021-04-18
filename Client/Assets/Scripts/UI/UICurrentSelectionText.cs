using Client.Game;
using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UICurrentSelectionText : MonoBehaviour {
        [SerializeField] private  TextMeshProUGUI text = null;

        private int? seat = null;
        private Hex hex = null;

        private void Start() {
            FindObjectOfType<UIBench>().SeatSelected += HandleSeatSelected;
            FindObjectOfType<UIBoard>().HexSelected += HandleHexSelected;

            Manager.GameClient.UnitSoldFromBench += HandleUnitSold;
            Manager.GameClient.UnitSoldFromBoard += HandleUnitSold;
            Manager.GameClient.UnitMovedFromBenchToBoard += HandleUnitMovedFromBenchToBoard;
        }

        private void HandleSeatSelected(int seat, string character) {
            text.text = character;
            this.seat = seat;
            hex = null;
        }

        private void HandleHexSelected(Hex hex) {
            if (!seat.HasValue) {
                text.text = hex.unit;
                this.hex = hex;
            }
            if (seat.HasValue && hex.unit == "") {
                Manager.GameClient.MoveUnit(seat.GetValueOrDefault(), hex, text.text);
            }
        }

        public void SellCurrentSelection() {
            if (!(seat is null)) {
                Manager.GameClient.SellUnit(seat.GetValueOrDefault(), text.text);
            } else if (!(hex is null)) {
                Manager.GameClient.SellUnit(hex);
            }
        }

        private void HandleUnitSold(SellUnitFromBenchPacket packet) {
            if (packet.Seat == seat) {
                seat = null;
                hex = null;
                text.text = "";
            }
        }

        private void HandleUnitSold(SellUnitFromBoardPacket packet) {
            if (packet.Coords == hex.coords) {
                hex = null;
                seat = null;
                text.text = "";
            }
        }

        private void HandleUnitMovedFromBenchToBoard(MoveToBoardFromBenchPacket packet) {
            hex = null;
            seat = null;
            text.text = "";
        }
    }
}
