using System;

namespace Server.Game.Timeline {
    public class GameTimeline {
        public Action Finished;
        public Action Changed;
        public Action RoundStart;

        private Shopping shopping;
        private TransitionToBattling transitionToBattling;
        private Battling battling;
        private TransitionToShopping transitionToShopping;
        private ForcefullyEndingGame forcefullyEndingGame;

        private IEvent[] events;
        private int current;

        public string CurrentEvent => events[current].GetType().ToString();

        public GameTimeline() {
            shopping = new Shopping();
            transitionToBattling = new TransitionToBattling();
            battling = new Battling();
            transitionToShopping = new TransitionToShopping();
            forcefullyEndingGame = new ForcefullyEndingGame(EndGame);

            events = new IEvent[] {
                // Round: 1
                shopping,
                transitionToBattling,
                battling,
                transitionToShopping,
                // Round: 2
                shopping,
                transitionToBattling,
                battling,
                transitionToShopping,
                // Round: 3
                shopping,
                transitionToBattling,
                battling,
                transitionToShopping,
                // Round: 4
                shopping,
                transitionToBattling,
                battling,
                transitionToShopping,
                // Round: 5
                shopping,
                transitionToBattling,
                battling,
                transitionToShopping,
                // Game has gone on for the max amount of turns
                forcefullyEndingGame
            };

            current = 0;

            shopping.RoundStart += () => RoundStart?.Invoke();
        }

        public void Start() {
            events[current].OnEnter();
        }

        public void Update(double time, double deltaTime) {
            bool finished = events[current].Update(time, deltaTime);
            if (finished) {
                events[current].OnExit();
                events[current] = events[++current];
                events[current].OnEnter();

                Changed?.Invoke();
            }
        }
        public void Reset() {
            events[current].OnExit();
            current = 0;
        }

        private void EndGame() {
            Finished?.Invoke();
        }
    }
}
