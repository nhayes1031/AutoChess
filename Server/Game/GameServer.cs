using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Server.Game {
    public class GameServer {
        private readonly NetServer server;
        private readonly GameLoop gameLoop;
        private readonly Hub hub;

        private ServerState currentState;
        private Thread thread;
        //private Timer checkForServerUseTimer;

        public bool InUse => currentState != ServerState.ReadyForNewGame;
        public int Port => server.Port;

        public GameServer(int port) {
            hub = Hub.Default;
            hub.Subscribe<GameFinished>(this, HandleGameFinished);

            currentState = ServerState.ReadyForNewGame;
            //checkForServerUseTimer = new Timer(Constants.SERVER_IN_USE_TIMER) {
            //    AutoReset = true,
            //    Enabled = true,
            //};
            //checkForServerUseTimer.Elapsed += CheckForServerUse;

            var config = new NetPeerConfiguration(Constants.LIDGREN_SERVER_NAME) {
                Port = port,
                MaximumConnections = Constants.MAXIMUM_CONNECTIONS
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            server = new NetServer(config);
            server.Start();

            gameLoop = new GameLoop(server);

            Logger.Debug("Host: " + server.Configuration.LocalAddress + " Port: " + server.Configuration.Port);
        }

        public void Initialize() {
            currentState = ServerState.InGame;

            Logger.Info("Initializing Game Server");
            gameLoop.Initialize();

            thread = new Thread(UpdateLoop);
            thread.Start();

            //checkForServerUseTimer.Start();
        }

        private void UpdateLoop() {
            while (currentState != ServerState.ReadyForNewGame) {
                gameLoop.Update();
            }
        }

        public void CleanUp() {
            Logger.Debug("Cleaning up Game Server");
            // TODO: Agones should dispose of the game server now

            currentState = ServerState.ReadyForNewGame;
            server.Connections.Clear();
            thread = null;
            //checkForServerUseTimer.Stop();
        }

        //private void CheckForServerUse(object source, ElapsedEventArgs e) {
        //    if (server.ConnectionsCount == 0 && currentState != ServerState.ReadyForNewGame)
        //        CleanUp();
        //}

        private void HandleGameFinished(GameFinished e) {
            CleanUp();
        }

        private enum ServerState {
            ReadyForNewGame,
            InGame
        }
    }
}
