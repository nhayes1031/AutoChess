using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Client.UI {
    public class UIPlayerPorts : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI portsText = null;
        DataStore dataStore;

        void Awake() {
            dataStore = FindObjectOfType<DataStore>();
            List<int> playerPorts = dataStore.playerPorts;
            string text = "";
            foreach (var port in playerPorts)
            {
                text += "port " + port + "\n";
            }
            portsText.text = text;
        }
    }
}
