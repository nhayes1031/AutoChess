using UnityEngine;
using Client;
using Client.Game;
using TMPro;

namespace Client.UI {
    public class UIGoldDisplay : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI goldText = null;

        void Start() {
            StaticManager.GameClient.UpdatePlayerInfo += DisplayPlayerGold;
        }

        private void DisplayPlayerGold(UpdatePlayerInfoPacket packet) {
            goldText.text = "Gold: " + packet.Gold;
        }
    }
}