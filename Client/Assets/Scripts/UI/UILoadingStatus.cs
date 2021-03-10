using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Client.UI {
    public class UILoadingStatus : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI loadStatus = null;

        void Start() {
            StaticManager.GameClient.Connected += DisplayLoadingStatus;
        }

        private void OnApplicationQuit() {
            StaticManager.GameClient.Connected -= DisplayLoadingStatus;
        }

        private void DisplayLoadingStatus(bool status) {
            loadStatus.text = "Loaded: " + status.ToString();
        }
    }
}