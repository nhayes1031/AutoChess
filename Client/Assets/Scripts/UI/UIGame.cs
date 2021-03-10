using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UIGame : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI playerPorts = null;

        private void Start() {
            StaticManager.GameClient.Ports += HandlePorts;
        }

        private void OnApplicationQuit() {
            StaticManager.GameClient.Ports -= HandlePorts;
        }

        private void HandlePorts(List<int> ports) {
            string text = "";
            foreach (var port in ports) {
                text += "port " + port + "\n";
            }
            playerPorts.text = text;
        }
    }
}
