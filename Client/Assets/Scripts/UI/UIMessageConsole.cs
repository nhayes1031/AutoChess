using Client.Game;
using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UIMessageConsole : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI text = null;

        private void Start() {
            StaticManager.GameClient.TransitionUpdate += HandleTransitionUpdate;
            StaticManager.GameClient.UpdatePlayerInfo += HandleUpdatePlayerInfo;
            StaticManager.GameClient.UnitPurchased += HandleUnitPurchased;

            text.text = "";
        }

        private void OnApplicationQuit() {
            StaticManager.GameClient.TransitionUpdate -= HandleTransitionUpdate;
            StaticManager.GameClient.UpdatePlayerInfo -= HandleUpdatePlayerInfo;
            StaticManager.GameClient.UnitPurchased -= HandleUnitPurchased;
        }

        private void HandleTransitionUpdate(TransitionUpdatePacket packet) {
            text.text += packet.Event + "\n";
        }

        private void HandleUpdatePlayerInfo(UpdatePlayerInfoPacket packet) {
            text.text += "Player Level: " + packet.Level + "\n";
            text.text += "Player Gold: " + packet.Gold + "\n";
            text.text += "Player XP: " + packet.XP + "\n";
            text.text += "Player Shop: ";
            for (int i = 0; i < 5; i++) {
                text.text += "\n" + packet.Shop[i];
            }
            text.text += "\n";
        }

        private void HandleUnitPurchased(string name) {
            text.text += "Unit Purchased: " + name + "\n";
        }
    }
}
