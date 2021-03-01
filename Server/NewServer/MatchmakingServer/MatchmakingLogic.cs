using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;

namespace Server.Matchmaking {
    public class MatchmakingLogic {
        private const int NUMBER_OF_PLAYERS_REQUIRED_FOR_A_GAME = 2;
        private List<NetConnection> queuedPlayers;

        public MatchmakingLogic() {
            queuedPlayers = new List<NetConnection>();
        }

        public bool RemoveFromQueue(NetConnection connection) {
            var removed = queuedPlayers.RemoveAll(player => player == connection);

            if (removed > 0)
                return true;
            else
                return false;
        }

        public void ClearQueue() {
            queuedPlayers.Clear();
        }

        public bool AddToQueue(NetConnection connection) {
            if (!queuedPlayers.Contains(connection)) {
                queuedPlayers.Add(connection);
                return true;
            }

            return false;
        }

        public bool IsThereEnoughPlayersForAGame() {
            if (queuedPlayers.Count == NUMBER_OF_PLAYERS_REQUIRED_FOR_A_GAME)
                return true;
            else
                return false;
        }

        public List<NetConnection> GetPlayersForAGame() {
            if (queuedPlayers.Count >= 2) {
                var players = queuedPlayers.Take(NUMBER_OF_PLAYERS_REQUIRED_FOR_A_GAME).ToList();
                queuedPlayers = queuedPlayers.Except(players).ToList();
                return players;
            }
            else
                return null;
        }
    }
}
