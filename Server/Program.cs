using Server;
using System;

public class Program {
    private const string CONSOLE_TITLE = "Game Server";

    static void Main(string[] args) {
        Console.Title = CONSOLE_TITLE;

        ServerManager serverManager = new ServerManager();
    }
}
