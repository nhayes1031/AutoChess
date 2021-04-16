using Lidgren.Network;
using Server.Game.Timers;
using System.Collections.Generic;

namespace Server.Game.Timeline {
    public class TransitionToShopping : IEvent {
        private Timer transitioningToShoppingTimer;
        private bool timerElapsed;

        public bool TimerExpired => timerElapsed;

        public TransitionToShopping() {
            transitioningToShoppingTimer = new Timer(Constants.MAX_TRANSITION_TO_SHOPPING_TIME) {
                AutoReset = false
            };
            transitioningToShoppingTimer.Elapsed += HandleTimerExpired;
        }

        public void OnEnter(Dictionary<NetConnection, PlayerData> playerDatas) {
            transitioningToShoppingTimer.Start();
        }

        public bool Update(double time, double deltaTime) {
            transitioningToShoppingTimer.Update(deltaTime);
            return timerElapsed;
        }

        public void OnExit() {
            transitioningToShoppingTimer.Stop();
            timerElapsed = false;
        }

        private void HandleTimerExpired() {
            timerElapsed = true;
        }

        public override string ToString() => "Transitioning To Shopping";
    }
}
