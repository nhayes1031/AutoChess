using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.UI {
    public class UIStartMenu : MonoBehaviour {

        private void Start() {
            StaticManager.MatchmakingClient.GameFound += TransitionToLoading;
        }

        private void OnApplicationQuit() {
            StaticManager.MatchmakingClient.GameFound -= TransitionToLoading;
        }

        private void TransitionToLoading() {
            SceneManager.LoadScene("LoadingScreen");
        }
    }
}
