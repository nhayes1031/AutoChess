using UnityEngine;
using Client.Game;
using TMPro;

namespace Client.UI {
    public class UILevelDisplay : MonoBehaviour {
        [SerializeField] private  TextMeshProUGUI levelText = null;

        private void Start() {
            Manager.GameClient.UpdatePlayerInfo += DisplayPlayerLevel;
        }

        private void OnDestroy() {
            Manager.GameClient.UpdatePlayerInfo -= DisplayPlayerLevel;
        }

        private void DisplayPlayerLevel(UpdatePlayerInfoPacket packet) {
            levelText.text = "Level: " + packet.Level;
        }
    }
}