using Lidgren.Network;
using Server.Extensions;
using Server.Game.Systems;
using Server.Game.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Timeline {
    public class Battling : IEvent {
        public Action<Guid> GameOver;

        private Timer battlingTimer;
        private bool timerElapsed;
        private List<Battle> battles = new List<Battle>();
        private Dictionary<NetConnection, PlayerData> playerDatas;

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

            CleanUpBattles();
            CheckForWinner();
        }

        private void CheckForWinner() {
            var playersLeftAlive = playerDatas
                .Select(x => x.Value)
                .Where(x => x.IsAlive);

            if (playersLeftAlive.Count() == 1) {
                GameOver?.Invoke(playersLeftAlive.First().Id);
            }
        }

        private void CleanUpBattles() {
            foreach (var battle in battles) {
                if (!battle.Finished)
                    battle.ForceFinish();
            }
            battles.Clear();
        }

        public void OnEnter(Dictionary<NetConnection, PlayerData> playerDatas) {
            this.playerDatas = playerDatas;
            var pairs = FindPairs();

            foreach (var pair in pairs) {
                battles.Add(new Battle(pair.Item1, pair.Item2));
            }
            battlingTimer.Start();
        }

        private List<Tuple<IPlayer, IPlayer>> FindPairs() {
            var alivePlayers = this.playerDatas
                .Where(x => x.Value.IsAlive)
                .Select(x => x.Value as IPlayer)
                .ToList();
            alivePlayers.Shuffle();
            
            if (alivePlayers.Count % 2 != 0) {
                var ghost = new GhostPlayerData(alivePlayers.PickRandom());
                alivePlayers.Add(ghost);
            }

            var groups = alivePlayers
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 2)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            var pairs = new List<Tuple<IPlayer, IPlayer>>();
            foreach (var group in groups) {
                pairs.Add(new Tuple<IPlayer, IPlayer>(group.First(), group.Last()));
            }
            return pairs;
        }

        private void HandleTimerExpired() {
            timerElapsed = true;
        }

        public override string ToString() => "Battling";
    }
}
