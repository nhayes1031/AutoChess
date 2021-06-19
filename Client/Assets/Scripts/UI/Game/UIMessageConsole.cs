using Client.Game;
using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UIMessageConsole : MonoBehaviour {
        [SerializeField] private  TextMeshProUGUI text = null;

        private void Start() {
            Manager.GameClient.TransitionUpdate += HandleTransitionUpdate;
            Manager.GameClient.UpdatePlayerInfo += HandleUpdatePlayerInfo;
            Manager.GameClient.UnitPurchased += HandleUnitPurchased;
            Manager.GameClient.UnitMoved += HandleUnitMoved;

            text.text = "";
        }

        private void OnDestory() {
            Manager.GameClient.TransitionUpdate -= HandleTransitionUpdate;
            Manager.GameClient.UpdatePlayerInfo -= HandleUpdatePlayerInfo;
            Manager.GameClient.UnitPurchased -= HandleUnitPurchased;
            Manager.GameClient.UnitMoved -= HandleUnitMoved;
        }

        private void HandleTransitionUpdate(StateTransitionPacket packet) {
            text.text += packet.Event + "\n";
        }

        private void HandleUpdatePlayerInfo(UpdatePlayerPacket packet) {
            text.text += "Player Shop: ";
            for (int i = 0; i < 5; i++) {
                text.text += "\n" + packet.Shop[i];
            }
            text.text += "\n";
        }

        private void HandleUnitPurchased(string name) {
            text.text += "Unit Purchased: " + name + "\n";
        }

        private void HandleUnitMoved(UnitMovedPacket packet) {
            text.text += $"Unit moved from {packet.FromCoords.coords} to {packet.ToCoords.coords}\n";
        }
    }
}
