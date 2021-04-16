using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using Server.Game.Systems;
using Server.Game.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Timeline {
    public class Battling : IEvent {
        private Timer battlingTimer;
        private bool timerElapsed;
        private List<Battle> battles = new List<Battle>();

        public bool TimerExpired => timerElapsed;

        public Battling() {
            battlingTimer = new Timer(Constants.MAX_BATTLING_TIME) {
                AutoReset = false
            };
            battlingTimer.Elapsed += HandleTimerExpired;
        }

        public bool Update(double time, double deltaTime) {
            battlingTimer.Update(deltaTime);
            foreach (var battle in battles) {
                battle.Update(time, deltaTime);
            }
            return timerElapsed;
        }

        public void OnExit() {
            battlingTimer.Stop();
            timerElapsed = false;
            battles.Clear();
        }

        public void OnEnter(Dictionary<NetConnection, PlayerData> playerDatas) {
            // TODO: This fails for odd number of players
            var pairs = playerDatas
                .Select(x => x.Value)
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 2)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            foreach (var pair in pairs) {
                battles.Add(new Battle(pair.First(), pair.Last()));
            }
            battlingTimer.Start();
        }

        private void HandleTimerExpired() {
            timerElapsed = true;
        }

        public override string ToString() => "Battling";
    }
}
