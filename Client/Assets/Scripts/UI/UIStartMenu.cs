using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.UI {
    public class UIStartMenu : MonoBehaviour {

        private void Start() {
            Manager.MatchmakingClient.GameFound += TransitionToLoading;
        }

        private void OnDestroy() {
            Manager.MatchmakingClient.GameFound -= TransitionToLoading;
        }

        private void TransitionToLoading() {
            SceneManager.LoadScene("LoadingScreen");
        }
    }
}
