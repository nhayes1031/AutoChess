using Client.Game;
using Client.Matchmaking;

namespace Client {
    public class StaticManager {
        public static MatchmakingClient MatchmakingClient { get; set; }
        public static GameClient GameClient { get; set; }

        public static void InitializeMatchmakingClient(int port, string server, string game) {
            MatchmakingClient = new MatchmakingClient(port, server, game);

            GameClient = new GameClient();
        }

        public static void InitializeGameClient(int port, string server, string game) {
            GameClient.Init(port, server, game);
        }

        public static void Disconnect() {
            MatchmakingClient.SendDisconnect();
            GameClient.SendDisconnect();
        }
    }
}
