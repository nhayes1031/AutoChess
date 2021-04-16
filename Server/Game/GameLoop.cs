using Lidgren.Network;
using System;

namespace Server.Game {
    public class GameLoop {
        public Action Finished;

        private readonly GameMessageHandler gameMessageHandler;
        private readonly MessageHub messageHub;
        private readonly GameLogic gameLogic;

        // TODO: What if I had a variable delta time?
        private double time = 0.0;
        private const double deltaTime = 0.01;

        private double currentTime;
        private double accumulator = 0.0;

        public GameLoop(NetServer server) {
            currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            gameMessageHandler = new GameMessageHandler(server);
            messageHub = new MessageHub(server);
            gameLogic = new GameLogic(server);
        }

        public void Initialize() {
            gameLogic.Initialize();
        }

        public void Update() {
            gameMessageHandler.Update();

            double newTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double frameTime = newTime - currentTime;
            currentTime = newTime;

            accumulator += frameTime;

            while ( accumulator >= deltaTime) {
                gameLogic.Update(time, deltaTime);
                accumulator -= deltaTime;
                time += deltaTime;
            }
        }

        public void CleanUp() {
            time = 0.0;
            accumulator = 0.0;

            gameLogic.CleanUp();
        }
    }
}
