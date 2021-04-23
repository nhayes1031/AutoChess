using UnityEngine;
using Client.Game;
using TMPro;

namespace Client.UI {
    public class UIGoldDisplay : MonoBehaviour {
        [SerializeField] private  TextMeshProUGUI goldText = null;

        private void Start() {
            Manager.GameClient.UpdatePlayerInfo += DisplayPlayerGold;
        }

        private void OnDestory() {
            Manager.GameClient.UpdatePlayerInfo -= DisplayPlayerGold;
        }

        private void DisplayPlayerGold(UpdatePlayerInfoPacket packet) {
            goldText.text = "Gold: " + packet.Gold;
        }
    }
}