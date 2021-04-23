using UnityEngine;
using TMPro;

namespace Client.UI {
    public class UILoadingStatus : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI loadStatus = null;

        private void Start() {
            Manager.GameClient.Connected += DisplayLoadingStatus;
        }

        private void OnDestroy() {
            Manager.GameClient.Connected -= DisplayLoadingStatus;
        }

        private void DisplayLoadingStatus(bool status) {
            loadStatus.text = "Loaded: " + status.ToString();
        }
    }
}