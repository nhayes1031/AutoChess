using UnityEngine;
using Client;
using Client.Game;
using TMPro;

namespace Client.UI {
    public class UILevelDisplay : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI levelText = null;

        void Start() {
            StaticManager.GameClient.UpdatePlayerInfo += DisplayPlayerLevel;
        }

        private void DisplayPlayerLevel(UpdatePlayerInfoPacket packet) {
            levelText.text = "Level: " + packet.Level;
        }
    }
}