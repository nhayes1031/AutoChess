using UnityEngine;

namespace Client.UI {
    public class UIReconnect : MonoBehaviour {
        private void Start() {
            StaticManager.MatchmakingClient.Connected += HandleConnection;
        }

        private void OnApplicationQuit() {
            StaticManager.MatchmakingClient.Connected -= HandleConnection;
        }

        private void HandleConnection(bool connectionStatus) {
            if (connectionStatus)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }

        public void Reconnect() {
            StaticManager.MatchmakingClient.Reconnect();
            gameObject.SetActive(false);
        }
    }
}
