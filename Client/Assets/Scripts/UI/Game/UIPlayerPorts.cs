using UnityEngine;
using TMPro;

namespace Client.UI {
    public class UIPlayerPorts : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI portsText = null;

        private DataStore dataStore;

        private void Awake() {
            dataStore = FindObjectOfType<DataStore>();
            var playerPorts = dataStore.playerPorts;
            string text = "";
            foreach (var port in playerPorts)
            {
                text += "port " + port + "\n";
            }
            portsText.text = text;
        }
    }
}
