using Client.Game;
using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UICurrentSelectionText : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI text = null;

        private int? seat = null;

        private void Start() {
            FindObjectOfType<UIBench>().SeatSelected += HandleSeatSelected;
            StaticManager.GameClient.UnitSold += HandleUnitSold;
        }

        private void HandleSeatSelected(int seat, string character) {
            text.text = character;
            this.seat = seat;
        }

        public void SellCurrentSelection() {
            if (seat != null) {
                StaticManager.GameClient.SellUnit(seat.GetValueOrDefault(), text.text);
            }
        }

        public void HandleUnitSold(SellUnitFromBenchPacket packet) {
            if (packet.Seat == seat) {
                seat = null;
                text.text = "";
            }
        }
    }
}
