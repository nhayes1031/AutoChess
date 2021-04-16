namespace Server.Game {
    public class Constants {
        public const int HEALTH_CHECK_PERIOD = 60000;

        public const int MAXIMUM_CONNECTIONS = 2;
        public const int MINIMUM_PLAYERS_FOR_A_GAME = 2;
        public const int MAXIMUM_TIME_TO_WAIT_FOR_PLAYERS = 10000;
        public const string LIDGREN_SERVER_NAME = "AutoChess Game";
        public const int SERVER_IN_USE_TIMER = 10000;
        public const double MS_PER_UPDATE = 16.0d;

        public const int MAX_SHOPPING_TIME = 15000;
        public const int MAX_TRANSITION_TO_BATTLING_TIME = 1000;
        public const int MAX_BATTLING_TIME = 10000;
        public const int MAX_TRANSITION_TO_SHOPPING_TIME = 1000;

        public const int XP_PER_ROUND = 1;

        public const int BOARD_HEIGHT = 4;
        public const int BOARD_WIDTH = 8;
    }
}
