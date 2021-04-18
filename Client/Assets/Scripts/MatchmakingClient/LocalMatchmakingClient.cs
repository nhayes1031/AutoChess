using System;
using System.Threading.Tasks;

namespace Client.Matchmaking {
    public class LocalMatchmakingClient : IMatchmakingClient {
        public Action<bool> Connected { get; set; }
        public Action GameFound { get; set; }
        public Action<bool> Queued { get; set; }

        public LocalMatchmakingClient() {
            _ = AsyncTriggerConnected(true);
        }

        public void CancelMatchmakingRequest() {
            _ = AsyncTriggerQueued(false);
        }

        public void Disconnect() {
            _ = AsyncTriggerConnected(false);
        }

        public void SendMatchmakingRequest() {
            _ = AsyncTriggerQueued(true);
            _ = AsyncTriggerGameFound();
        }

        private async Task AsyncTriggerConnected(bool status) {
            await Task.Delay(100);
            Connected?.Invoke(status);
        }

        private async Task AsyncTriggerQueued(bool status) {
            await Task.Delay(100);
            Queued?.Invoke(status);
        }

        private async Task AsyncTriggerGameFound() {
            await Task.Delay(250);
            Queued?.Invoke(false);
            GameFound?.Invoke();
            ConnectToGameServer();
        }

        private void ConnectToGameServer() {
            Manager.GameClient.Initialize(34561, "127.0.0.1", "AutoChess Game");
        }
    }
}
