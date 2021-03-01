using Lidgren.Network;
using System.Threading;

namespace Server.Game {
    public class GameServer {
        private NetServer server;
        private GameLoop gameLoop;

        private ServerState currentState;
        private Thread thread;
        private System.Timers.Timer checkForServerUseTimer;

        public bool InUse => currentState != ServerState.ReadyForNewGame;
        public int Port => server.Port;

        public GameServer(int port) {
            currentState = ServerState.ReadyForNewGame;
            checkForServerUseTimer = new System.Timers.Timer(Constants.SERVER_IN_USE_TIMER) {
                AutoReset = true,
                Enabled = true,
            };
            checkForServerUseTimer.Elapsed += CheckForServerUse;

            var config = new NetPeerConfiguration(Constants.LIDGREN_SERVER_NAME) {
                Port = port,
                MaximumConnections = Constants.MAXIMUM_CONNECTIONS
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            server = new NetServer(config);
            server.Start();

            gameLoop = new GameLoop(server);

            gameLoop.Finished += HandleGameFinished;
        }

        public void Initialize() {
            currentState = ServerState.InGame;

            Logger.Info("Initializing Game Server");
            gameLoop.Initialize();

            thread = new Thread(UpdateLoop);
            thread.Start();

            checkForServerUseTimer.Start();
        }

        private void UpdateLoop() {
            while (currentState != ServerState.ReadyForNewGame) {
                gameLoop.Update();
            }
        }

        public void CleanUp() {
            Logger.Debug("Cleaning up Game Server");
            gameLoop.CleanUp();
            currentState = ServerState.ReadyForNewGame;
            server.Connections.Clear();
            thread = null;
            checkForServerUseTimer.Stop();
        }

        private void CheckForServerUse(object source, System.Timers.ElapsedEventArgs e) {
            if (server.ConnectionsCount == 0 && currentState != ServerState.ReadyForNewGame)
                CleanUp();
        }

        private void HandleGameFinished() {
            CleanUp();
        }

        private enum ServerState {
            ReadyForNewGame,
            InGame
        }
    }
}
