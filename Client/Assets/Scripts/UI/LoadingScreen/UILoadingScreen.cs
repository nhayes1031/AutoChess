using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.UI {
    public class UILoadingScreen : MonoBehaviour {
        private DataStore dataStore;

        private void Awake() {
            dataStore = FindObjectOfType<DataStore>();
        }

        private void Start() {
            Manager.GameClient.Ports += TransitionToGame;
        }

        private void OnDestory() {
            Manager.GameClient.Ports -= TransitionToGame;
        }

        private void TransitionToGame(List<int> ports) {
            dataStore.playerPorts = ports;
            SceneManager.LoadScene("Game");
        }
    }
}
