using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Server.Game {
    public class GameServer {
        public Action Reserve;
        public Action Shutdown;
        public Action Healthy;

        private NetServer server;
        private GameLoop gameLoop;
        private Hub hub;

        private Timer healthCheckTimer;

        public GameServer(int port) {
            // Should initialize the pub sub system
            hub = Hub.Default;
            hub.Subscribe<GameFinished>(this, HandleGameFinished);
            hub.Subscribe<PlayersConnected>(this, HandlePlayersConnected);

            // Should create a net server on the port
            var config = new NetPeerConfiguration(Constants.LIDGREN_SERVER_NAME) {
                Port = port,
                MaximumConnections = Constants.MAXIMUM_CONNECTIONS
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            server = new NetServer(config);
            server.Start();

            // Should initialize the game loop but not start it
            gameLoop = new GameLoop(server);

            healthCheckTimer = new Timer(Constants.HEALTH_CHECK_PERIOD) {
                AutoReset = true,
                Enabled = true
            };
            healthCheckTimer.Elapsed += (object source, ElapsedEventArgs e) => Healthy?.Invoke();
        }

        public bool Initialize() {
            try {
                gameLoop = new GameLoop(server);
                return true;
            } catch {
                return false;
            }
        }

        public async Task StartGameLoop() {
            await Task.Run(UpdateLoop);
        }

        private void UpdateLoop() {
            gameLoop.Update();
        }

        private void CleanUp() {
            gameLoop.CleanUp();
            server.Connections.Clear();
            healthCheckTimer.Stop();

            Shutdown?.Invoke();
        }

        private void HandleGameFinished(GameFinished e) {
            CleanUp();
        }

        private void HandlePlayersConnected(PlayersConnected e) {
            Reserve?.Invoke();
            healthCheckTimer.Start();
        }
    }
}
