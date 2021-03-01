using Lidgren.Network;
using System.Threading;

namespace Server.Matchmaking {
    public class MatchmakingServer {
        private const int MAXIMUM_CONNECTIONS = 100;
        private const string LIDGREN_SERVER_NAME = "AutoChess";

        private NetServer server;
        private MatchmakingMessageHandler matchmakingMessageHandler;

        private Thread thread;

        public MatchmakingServer(int port) {
            var config = new NetPeerConfiguration(LIDGREN_SERVER_NAME) {
                Port = port,
                MaximumConnections = MAXIMUM_CONNECTIONS
            };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            server = new NetServer(config);
            server.Start();

            matchmakingMessageHandler = new MatchmakingMessageHandler(server);

            thread = new Thread(UpdateLoop);
            thread.Start();
        }

        private void UpdateLoop() {
            while(true) {
                matchmakingMessageHandler.Update();
            }
        }
    }
}
