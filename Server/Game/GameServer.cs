using Agones;
using Grpc.Core;
using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Server.Game {
    public class GameServer {
        private IAgonesSDK agones;
        private Timer checkForServerUseTimer;
        private GameLoop gameLoop;
        private NetServer server;

        public GameServer() {
            SubscribeToHub();
            InitializeCheckForServerUseTimer();
            InitializeVariables();
        }

        public async Task ConnectToAgonesAsync() {
            bool ok = await agones.ConnectAsync();
            if (!ok)
                throw new Exception("Server - Failed to connect to Agones");
        }

        public async Task ReadyLidgrenServer() {
            var gameServer = await agones.GetGameServerAsync();
            var port = gameServer.Status.Ports[0].Port_;
            Logger.Info("Assigned this port: " + port);

            StartLidgrenServer(port);

            var status = await agones.ReadyAsync();
            if (status.StatusCode != StatusCode.OK) {
                throw new Exception("Server - Ready Failed");
            }

            new Thread(HealthChecks).Start();
        }

        private void SubscribeToHub() {
            Hub.Default.Subscribe<GameFinished>(this, HandleGameFinished);
        }

        private void InitializeCheckForServerUseTimer() {
            checkForServerUseTimer = new Timer(Constants.SERVER_IN_USE_TIMER) {
                AutoReset = false
            };
            checkForServerUseTimer.Elapsed += CheckForServerUse;
        }

        private void StartLidgrenServer(int port) {
            var config = new NetPeerConfiguration(Constants.LIDGREN_SERVER_NAME) {
                Port = port,
                MaximumConnections = Constants.MAXIMUM_CONNECTIONS
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            server = new NetServer(config);
            server.Start();
        }

        private void InitializeVariables() {
            if (Environment.GetEnvironmentVariable("deployment") == "local") {
                agones = new LocalAgonesSDK();
            } else {
                agones = new AgonesSDK();
            }
        }
        
        public async Task Start() {
            gameLoop = new GameLoop(server);
            gameLoop.Initialize();

            new Thread(UpdateLoop).Start();

            checkForServerUseTimer.Start();

            await agones.AllocateAsync();
        }

        private void UpdateLoop() {
            while(true) {
                gameLoop.Update();
            }
        }

        private void HealthChecks() {
            while(true) {
                Thread.Sleep(500);
                agones.HealthAsync();
            }
        }

        private void CleanUp() {
            agones.ShutDownAsync();
            agones.Dispose();
            server.Shutdown("bye");
        }

        private void CheckForServerUse(object source, ElapsedEventArgs e) {
            if (server.ConnectionsCount == 0)
                CleanUp();
        }

        private void HandleGameFinished(GameFinished e) {
            CleanUp();
        }
    }
}
