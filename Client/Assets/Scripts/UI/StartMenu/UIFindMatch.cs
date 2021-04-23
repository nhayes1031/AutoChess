using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI {
    public class UIFindMatch : MonoBehaviour {
        [SerializeField] private  Button button = null;
        [SerializeField] private  TextMeshProUGUI text = null;
        private bool isQueued;

        private void Start() {
            Manager.MatchmakingClient.Queued += HandleQueued;
            Manager.GameClient.Connected += HandleGameClientConnected;
        }

        private void OnDestroy() {
            Manager.MatchmakingClient.Queued -= HandleQueued;
            Manager.GameClient.Connected -= HandleGameClientConnected;
        }

        public void FindMatch() {
            if (isQueued) {
                Manager.MatchmakingClient.CancelMatchmakingRequest();
            } else {
                Manager.MatchmakingClient.SendMatchmakingRequest();
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
            if (isQueued) {
                text.text = "Cancel";
            } else {
                text.text = "Find Match";
            }
        }
    }
}
