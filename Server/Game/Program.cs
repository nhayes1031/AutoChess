using System;
using System.Threading.Tasks;

namespace Server.Game {
    public class Program {
        private const string CONSOLE_TITLE = "Game Server";

        public static async Task Main() {
            Console.Title = CONSOLE_TITLE;

            try {
                Logger.Info("Create game server");
                var server = new GameServer();
                Logger.Info("Connecting to agones");
                await server.ConnectToAgonesAsync();
                Logger.Info("Starting Lidgren server");
                await server.ReadyLidgrenServer();
                Logger.Info("Starting server");
                await server.Start();
            } catch (Exception e) {
                Logger.Error(e.Message);
            }
        }
    }
}
