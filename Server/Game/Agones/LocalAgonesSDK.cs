using Agones;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace Server.Game {
    public class LocalAgonesSDK : IAgonesSDK {
        private const int PORT = 34561;

        public Task<Status> AllocateAsync() {
            return Task.FromResult(Status.DefaultSuccess);
        }

        public IAgonesAlphaSDK Alpha() {
            throw new NotImplementedException();
        }

        public Task<bool> ConnectAsync() {
            return Task.FromResult(true);
        }

        public void Dispose() {
            
        }

        public Task<Agones.Dev.Sdk.GameServer> GetGameServerAsync() {
            var gameServer = new Agones.Dev.Sdk.GameServer();
            gameServer.Status = new Agones.Dev.Sdk.GameServer.Types.Status();
            gameServer.Status.Ports.Add(
                new Agones.Dev.Sdk.GameServer.Types.Status.Types.Port {
                    Port_ = PORT
                }
            );
            return Task.FromResult(gameServer);
        }

        public Task<Status> HealthAsync() {
            Logger.Info("Health Checked");
            return Task.FromResult(Status.DefaultSuccess);
        }

        public Task<Status> ReadyAsync() {
            return Task.FromResult(Status.DefaultSuccess);
        }

        public Task<Status> ReserveAsync(long seconds) {
            throw new NotImplementedException();
        }

        public Task<Status> SetAnnotationAsync(string key, string value) {
            throw new NotImplementedException();
        }

        public Task<Status> SetLabelAsync(string key, string value) {
            throw new NotImplementedException();
        }

        public Task<Status> ShutDownAsync() {
            return Task.FromResult(Status.DefaultSuccess);
        }

        public void WatchGameServer(Action<Agones.Dev.Sdk.GameServer> callback) {
            throw new NotImplementedException();
        }
    }
}
