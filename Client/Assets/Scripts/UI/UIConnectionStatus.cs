using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UIConnectionStatus : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI text = null;

        private void Start() {
            text.text = "Connecting";
            Manager.MatchmakingClient.Connected += HandleConnection;
        }

        private void OnDestroy() {
            Manager.MatchmakingClient.Connected -= HandleConnection;
        }

        private void HandleConnection(bool connectionStatus) {
            text.text = "Connected: " + connectionStatus;
        }
    }
}
