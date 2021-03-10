using UnityEngine;

namespace Client {
    public class Manager : MonoBehaviour {
        // TODO: Get this info with server discovery
        [Header("Network Info")]
        public int port = 34560;
        public string server = "127.0.0.1";
        public string gameName = "AutoChess";

        private void Awake() {
            DontDestroyOnLoad(this.gameObject);

            Debug.Log("Starting game manager");
            StaticManager.InitializeMatchmakingClient(port, server, gameName);
        }

        private void OnApplicationQuit() {
            StaticManager.Disconnect();
        }
    }
}
