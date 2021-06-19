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

            Manager.GameClient.UnitSold += HandleUnitSold;
            Manager.GameClient.UnitRepositioned += HandleUnitRepositioned;
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

        private void HandleUnitSold(UnitSoldPacket packet) {
            if (packet.Location is BenchLocation) {
                var location = (BenchLocation)packet.Location;
                if (location.seat == seat) {
                    seat = null;
                    hex = null;
                    text.text = "";
                }
            }
            else if (packet.Location is BoardLocation) {
                var location = (BoardLocation)packet.Location;
                if (location.coords == hex.coords) {
                    hex = null;
                    seat = null;
                    text.text = "";
                }
            }
        }

        private void HandleUnitRepositioned(UnitRepositionedPacket packet) {
            hex = null;
            seat = null;
            text.text = "";
        }
    }
}
