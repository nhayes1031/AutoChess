using UnityEngine;
using Client;
using Client.Game;
using TMPro;

namespace Client.UI {
    public class UIXpDisplay : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI xpText = null;

        private void Start() {
            StaticManager.GameClient.UpdatePlayerInfo += DisplayXPLevel;
        }

        private void DisplayXPLevel(UpdatePlayerInfoPacket packet) {
            xpText.text = "XP: " + packet.XP + "/" + DisplayExperienceToLevel(packet.Level);
        }

        private int DisplayExperienceToLevel(int currentLevel) {
            return currentLevel * 4; ;
        }
    }
}
