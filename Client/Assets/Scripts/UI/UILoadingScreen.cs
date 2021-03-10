using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.UI {
    public class UILoadingScreen : MonoBehaviour {

        private void Start() {
            StaticManager.GameClient.Ports += TransitionToGame;
        }

        private void OnApplicationQuit() {
            StaticManager.GameClient.Ports -= TransitionToGame;
        }

        private void TransitionToGame(List<int> ports) {
            SceneManager.LoadScene("Game");
        }
    }
}
