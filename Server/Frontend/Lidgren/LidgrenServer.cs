using Lidgren.Network;

namespace Frontend {
    public class LidgrenServer {
        private const int MAXIMUM_CONNECTIONS = 100;
        private const string LIDGREN_SERVER_NAME = "Autochess_frontend";
        private const int LIDGREN_PORT = 34560;

        private NetServer server;
        private MessageHandler messageHandler;

        public LidgrenServer() {
            var config = new NetPeerConfiguration(LIDGREN_SERVER_NAME) {
                Port = LIDGREN_PORT,
                MaximumConnections = MAXIMUM_CONNECTIONS
            };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            server = new NetServer(config);
            server.Start();

            messageHandler = new MessageHandler(server);
        }

        public void Update() {
            while(true) {
                messageHandler.Update();
            }
        }
    }
}
