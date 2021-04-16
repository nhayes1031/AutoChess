using System;

namespace Server.Game.Timers {
    public class Timer {
        public Action Elapsed;
        public bool AutoReset { get; set; }
        public double Interval { get; set; }

        private double elapsedTime = 0.0d;
        private bool started = false;

        public Timer(double interval) {
            Interval = interval;
        }

        public void Start() {
            started = true;
        }

        public void Stop() {
            started = false;
        }

        public void Update(double deltaTime) {
            if (started) {
                elapsedTime += deltaTime;
                while (elapsedTime >= Interval) {
                    elapsedTime -= Interval;
                    Elapsed?.Invoke();

                    if (!AutoReset) {
                        started = false;
                    }
                }
            }
        }
    }
}
