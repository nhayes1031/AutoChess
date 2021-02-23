using System;

namespace Server.Game.Timeline {
    // TODO: Maybe send out a packet to tell the user that the game is over
    public class ForcefullyEndingGame : IEvent {
        private Action endGame;

        public ForcefullyEndingGame(Action endGame) {
            this.endGame = endGame;
        }

        public void OnEnter() {
            Logger.Warn("Forcefully ending game");
            endGame?.Invoke();
        }

        public bool Update(double time, double deltaTime) { return false; }

        public void OnExit() { }
    }
}
