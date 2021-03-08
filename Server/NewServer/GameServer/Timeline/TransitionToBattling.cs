using Server.Game.Timers;

namespace Server.Game.Timeline {
    public class TransitionToBattling : IEvent {
        private Timer transitioningToBattlingTimer;
        private bool timerElapsed;

        public bool TimerExpired => timerElapsed;


        public TransitionToBattling() {
            transitioningToBattlingTimer = new Timer(Constants.MAX_TRANSITION_TO_BATTLING_TIME) {
                AutoReset = false
            };
            transitioningToBattlingTimer.Elapsed += HandleTimerExpired;
        }

        public void OnEnter() {
            transitioningToBattlingTimer.Start();
        }

        public bool Update(double time, double deltaTime) {
            transitioningToBattlingTimer.Update(deltaTime);
            return timerElapsed;
        }

        public void OnExit() {
            transitioningToBattlingTimer.Stop();
            timerElapsed = false;
        }

        private void HandleTimerExpired() {
            timerElapsed = true;
        }

        public override string ToString() => "Transitioning To Battling";
    }
}
