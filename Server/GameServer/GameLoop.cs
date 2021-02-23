using Lidgren.Network;
using System;

namespace Server.Game {
    public class GameLoop {
        public Action Finished;

        private GameMessageHandler gameMessageHandler;
        private GameLogic gameLogic;

        // TODO: What if I had a variable delta time?
        private double time = 0.0;
        private const double deltaTime = 0.01;

        private double currentTime;
        private double accumulator = 0.0;

        public GameLoop(NetServer server) {
            currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            gameMessageHandler = new GameMessageHandler(server);
            gameLogic = new GameLogic(server);

            gameLogic.Finished += () => Finished?.Invoke();

            gameMessageHandler.RerollRequested += (client) => gameLogic.Reroll(client);
            gameMessageHandler.UnitPurchaseRequested += (request) => gameLogic.PurchaseUnit(request);

            gameLogic.Lock += HandleGameLogicLocking;
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

        private void HandleGameLogicLocking(bool status) {
            gameMessageHandler.SetLock(status);
        }
    }
}
