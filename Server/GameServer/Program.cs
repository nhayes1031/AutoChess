using Agones;
using System;
using System.Threading.Tasks;

namespace Server.Game {
    public class Program {
        private const string CONSOLE_TITLE = "Game Server";
        private const int RESERVE_TIME = 3600;

        static async Task Main(string[] args) {
            Console.Title = CONSOLE_TITLE;

            var agones = new AgonesSDK();
            bool ok = await agones.ConnectAsync();
            if (ok) {
                await agones.ReadyAsync();
            }
        }

        private static async void HandleReserve(AgonesSDK agones) {
            Logger.Info("Reserving Game server...");

            var status = await agones.ReserveAsync(RESERVE_TIME);
            switch (status.StatusCode) {
                case Grpc.Core.StatusCode.OK:
                    Logger.Info("Reserved for " + RESERVE_TIME + " seconds!");
                    break;
                default:
                    Logger.Info("Failed to reserve Game server...");
                    break;
            }
        }

        private static async void HandleShutdown(AgonesSDK agones) {
            Logger.Info("Attempting to shutdown...");

            var status = await agones.ShutDownAsync();
            switch (status.StatusCode) {
                case Grpc.Core.StatusCode.OK:
                    Logger.Info("Shutdown was successful!");
                    Logger.Info("Game server marked as completed!");
                    break;
                default:
                    Logger.Info("Failed to shutdown Game server...");
                    Logger.Info("Server will automatically be destroyed after " + RESERVE_TIME + "seconds");
                    break;
            }
        }

        private static async void HandleHealthy(AgonesSDK agones) {
            Logger.Info("Sending Health check...");
            await agones.HealthAsync();
        }
    }
}
