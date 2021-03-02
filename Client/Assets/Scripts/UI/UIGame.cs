using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UIGame : MonoBehaviour {
        [SerializeField] private GameObject canvas = null;
        [SerializeField] private TextMeshProUGUI playerPorts = null;

        private void Start() {
            StaticManager.GameClient.Ports += HandlePorts;
            StaticManager.GameClient.Connected += HandleConnected;

            canvas.SetActive(false);
        }

        private void OnApplicationQuit() {
            StaticManager.GameClient.Ports -= HandlePorts;
            StaticManager.GameClient.Connected -= HandleConnected;
        }

        private void HandlePorts(List<int> ports) {
            string text = "";
            foreach (var port in ports) {
                text += "port " + port + "\n";
            }
            playerPorts.text = text;
        }

        private void HandleConnected(bool status) {
            canvas.SetActive(status);
        }
    }
}
