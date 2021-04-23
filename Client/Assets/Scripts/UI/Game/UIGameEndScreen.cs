using Client.Game;
using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UIGameEndScreen : MonoBehaviour {
        [SerializeField] private GameObject mask = null;
        [SerializeField] private TextMeshProUGUI text = null;

        private void Awake() {
            mask.SetActive(false);
        }

        private void Start() {
            Manager.GameClient.GameOver += HandleGameOver;
            Manager.GameClient.PlayerDied += HandlePlayerDied;
        }

        private void HandleGameOver(GameOverPacket packet) {
            mask.SetActive(true);
            text.text = packet.Winner == Manager.GameClient.PlayerId ? "Victory" : "Defeat";
        }

        private void HandlePlayerDied(PlayerDiedPacket packet) {
            if (packet.Who == Manager.GameClient.PlayerId) {
                mask.SetActive(true);
                text.text = "Defeat";
            }
        }
    }
}
