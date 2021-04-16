using Client.Game;
using UnityEngine;

namespace Client {
    public class ServerTester : MonoBehaviour {
        [Header("Network Info")]
        public int port = 34561;
        public string server = "0.0.0.0";
        public string gameName = "AutoChess";

        private void Awake() {
            Debug.Log("Connecting to Game Server");
            StaticManager.GameClient = new GameClient();
            StaticManager.InitializeGameClient(port, server, gameName);
        }

        private void OnApplicationQuit() {
            StaticManager.Disconnect();
        }
    }
}
