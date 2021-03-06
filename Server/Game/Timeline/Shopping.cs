﻿using System;
using System.Collections.Generic;
using Lidgren.Network;
using Server.Game.Timers;

namespace Server.Game.Timeline {
    public class Shopping : IEvent {
        public Action RoundStart;

        private Timer shoppingTimer;
        private bool timerElapsed;

        public bool TimerExpired => timerElapsed;

        public Shopping() {
            shoppingTimer = new Timer(Constants.MAX_SHOPPING_TIME) {
                AutoReset = false
            };
            shoppingTimer.Elapsed += HandleTimerExpired;
        }

        public void OnEnter(Dictionary<NetConnection, PlayerData> playerDatas) {
            shoppingTimer.Start();
            RoundStart?.Invoke();
        }

        public bool Update(double time, double deltaTime) {
            shoppingTimer.Update(deltaTime);
            return timerElapsed;
        }

        public void OnExit() {
            shoppingTimer.Stop();
            timerElapsed = false;
        }

        private void HandleTimerExpired() {
            timerElapsed = true;
        }

        public override string ToString() => "Shopping";
    }
}
