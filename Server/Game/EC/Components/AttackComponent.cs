using PubSub;
using Server.Game.Systems;
using Server.Game.Timers;

namespace Server.Game.EC.Components {
    public class AttackComponent : Component {
        private readonly Hub hub;
        private readonly StateComponent state;
        private readonly StatsComponent stats;

        private readonly Timer attackingTimer;
        private Entity currentTarget;

        public AttackComponent(Hub simulationHub, StateComponent state, StatsComponent stats) {
            hub = simulationHub;
            this.state = state;
            this.stats = stats;

            attackingTimer = new Timer(stats.AttackSpeed * 100) {
                AutoReset = false
            };
            attackingTimer.Elapsed += HandleTimerExpired;
        }

        public override void Update(double time, double deltaTime, Board board) {
            attackingTimer.Update(deltaTime);

            state.AttackTargetInRange = board.FindEnemyInRange(
                parent.GetComponent<MovementComponent>().position.coords, 
                stats.AttackRange,
                parent.GetComponent<TeamComponent>().Team
            );
        }

        public void StartAttack() {
            state.StartActing();

            currentTarget = state.AttackTargetInRange;
            attackingTimer.Start();
        }

        private void HandleTimerExpired() {
            currentTarget.GetComponent<StatsComponent>().Health -= stats.AttackPower;

            // Publish damage message
            hub.Publish(new UnitAttacked() {
                attacker = parent.GetComponent<MovementComponent>().position.coords,
                defender = currentTarget.GetComponent<MovementComponent>().position.coords,
                damage = stats.AttackPower
            });

            attackingTimer.Stop();
            state.DoneActing();
        }
    }
}
