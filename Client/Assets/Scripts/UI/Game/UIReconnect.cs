using UnityEngine;

namespace Client.UI {
    public class UIReconnect : MonoBehaviour {
        [SerializeField] private GameObject button = null;

        private void Start() {
            Manager.MatchmakingClient.Connected += HandleConnection;
        }

        private void OnDestroy() {
            Manager.MatchmakingClient.Connected -= HandleConnection;
        }

        private void HandleConnection(bool connectionStatus) {
            if (connectionStatus) {
                button.SetActive(false);
            } else {
                button.SetActive(true);
            }
        }

        public void Reconnect() {
            Manager.Reconnect();
            button.SetActive(false);
        }
    }
}
