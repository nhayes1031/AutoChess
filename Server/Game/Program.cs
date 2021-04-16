using System;

namespace Server.Game {
    public class Program {
        private const string CONSOLE_TITLE = "Game Server";
        private const int PORT = 34561;

        public static void Main(string[] args) {
            Console.Title = CONSOLE_TITLE;

            var server = new GameServer(PORT);
            server.Initialize();
        }
    }
}
