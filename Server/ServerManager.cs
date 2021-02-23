using Server.Game;
using Server.Matchmaking;
using System.Collections.Generic;
using System.Linq;

namespace Server {
    public class ServerManager {
        public static ServerManager instance;

        private const int NUMBER_OF_SERVERS_TO_PRELOAD = 1;

        private MatchmakingServer mmserver;
        private List<GameServer> gservers;

        public ServerManager() {
            instance = this;

            // TODO: This should be any open port on the machine.
            // Use server discovery to find the right port on the client
            mmserver = new MatchmakingServer(34560);

            gservers = new List<GameServer>();
            int port;
            for (int i = 0; i < NUMBER_OF_SERVERS_TO_PRELOAD; i++) {
                port = GetOpenPort();
                if (port == -1)
                    throw new System.Exception("Failed to find open ports to preload game server");
                gservers.Add(new GameServer(port));
            }
        }

        private int GetOpenPort() {
            var startingAtPort = 5000;
            var maxNumberOfPortsToCheck = 500;
            var range = Enumerable.Range(startingAtPort, maxNumberOfPortsToCheck);
            var portsInUse =
                from p in range
                    join used in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners()
                        on p equals used.Port
                            select p;

            var FirstFreeUDPPortInRange = range.Except(portsInUse).FirstOrDefault();

            if (FirstFreeUDPPortInRange > 0) {
                return FirstFreeUDPPortInRange;
            } else {
                return -1;
            }
        }

        public GameServer GetGameServer() {
            return gservers.First(s => !s.InUse);
        }

        public bool GameServerAvailable() {
            return gservers.First(x => !x.InUse) != null;
        }
    }
}
