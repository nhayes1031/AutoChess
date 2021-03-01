using Server.Game.Timers;

namespace Server.Game.Timeline {
    public class Battling : IEvent {
        private Timer battlingTimer;
        private bool timerElapsed;

        public bool TimerExpired => timerElapsed;

        public Battling() {
            battlingTimer = new Timer(Constants.MAX_BATTLING_TIME) {
                AutoReset = false
            };
            battlingTimer.Elapsed += HandleTimerExpired;
        }

        public void OnEnter() {
            battlingTimer.Start();
        }

        public bool Update(double time, double deltaTime) {
            battlingTimer.Update(deltaTime);
            return timerElapsed;
        }

        public void OnExit() {
            battlingTimer.Stop();
            timerElapsed = false;
        }

        private void HandleTimerExpired() {
            timerElapsed = true;
        }
    }
}
