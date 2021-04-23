using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.UI {
    public class UIReturnToStartMenu : MonoBehaviour {
        public void ReturnToStartMenu() {
            Manager.DisconnectGameClient();
            SceneManager.LoadScene("StartMenu");
        }
    }
}
