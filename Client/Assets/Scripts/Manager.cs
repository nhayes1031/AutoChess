using Client.Game;
using Client.Matchmaking;
using UnityEngine;

namespace Client {
    public class Manager : MonoSingleton<Manager> {
        [Header("Build Info")]
        [SerializeField] private BuildTypes build = BuildTypes.Local;

        [Header("Matchmaking Network Info")]
        [SerializeField] private int port = 34560;
        [SerializeField] private string server = "127.0.0.1";
        [SerializeField] private string gameName = "Autochess_frontend";

        public static IMatchmakingClient MatchmakingClient { get; private set; }
        public static GameClient GameClient { get; private set; }

        private void Awake() {
            DontDestroyOnLoad(this);

            switch (build) {
                case BuildTypes.Cloud:
                    InitializeRealClients();
                    break;
                case BuildTypes.Local:
                    InitializeLocalClients();
                    break;
                case BuildTypes.GameOnly:
                    InitializeGameClient();
                    break;
            }
        }

        private void OnApplicationQuit() {
            if (GameClient != null)
                GameClient.Disconnect();

            if (MatchmakingClient != null)
                MatchmakingClient.Disconnect();
        }

        public static void Reconnect() {
            switch (instance.build) {
                case BuildTypes.Cloud:
                    MatchmakingClient = new MatchmakingClient(
                        instance.port,
                        instance.server,
                        instance.gameName
                   );
                    break;
                case BuildTypes.Local:
                    MatchmakingClient = new LocalMatchmakingClient();
                    break;
            }
        }

        private void InitializeRealClients() {
            MatchmakingClient = new MatchmakingClient(port, server, gameName);
            GameClient = new GameClient();
        }

        private void InitializeLocalClients() {
            MatchmakingClient = new LocalMatchmakingClient();
            GameClient = new GameClient();
        }

        private void InitializeGameClient() {
            GameClient = new GameClient();
            GameClient.Initialize(34561, "127.0.0.1", "AutoChess Game");
        }

        private enum BuildTypes {
            Cloud,
            Local,
            GameOnly
        }
    }
}
