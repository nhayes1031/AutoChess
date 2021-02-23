using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI {
    public class UIFindMatch : MonoBehaviour {
        [SerializeField] private Button button = null;
        [SerializeField] private TextMeshProUGUI text = null;
        private bool isQueued;

        private void Start() {
            StaticManager.MatchmakingClient.Queued += HandleQueued;
            StaticManager.GameClient.Connected += HandleGameClientConnected;
        }

        private void OnApplicationQuit() {
            StaticManager.MatchmakingClient.Queued -= HandleQueued;
            StaticManager.GameClient.Connected -= HandleGameClientConnected;
        }

        public void FindMatch() {
            if (isQueued) {
                StaticManager.MatchmakingClient.CancelMatchmakingRequest();
            } else {
                StaticManager.MatchmakingClient.SendMatchmakingRequest();
            }
        }

        private void HandleGameClientConnected(bool status) {
            if (status) {
                isQueued = false;
                text.text = "Find Match";
            }

            button.gameObject.SetActive(!status);
        }

        private void HandleQueued(bool queued) {
            isQueued = queued;
            UpdateDisplay();
        }

        private void UpdateDisplay() {
            if (isQueued) {
                text.text = "Cancel";
            } else {
                text.text = "Find Match";
            }
        }
    }
}
