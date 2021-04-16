using PubSub;
using Server.Game.Systems;
using Server.Game.Timers;

namespace Server.Game.EC.Components {
    public class MovementComponent : Component {
        public Hex position;

        private readonly Hub hub;
        private Hex hexToMoveTo;
        private readonly Timer movingTimer;

        public MovementComponent(Hub hub, StatsComponent stats) {
            this.hub = hub;

            movingTimer = new Timer(stats.MovementSpeed) {
                AutoReset = false
            };
            movingTimer.Elapsed += HandleTimerExpired;
        }

        public override void Update(double time, double deltaTime, Board board) {
            movingTimer.Update(deltaTime);
        }

        public void StartMove(Board board) {
            var state = parent.GetComponent<StateComponent>();
            state.StartActing();

            hexToMoveTo = board.GetHexInDirectionOfClosestEnemy(parent);
            if (!(hexToMoveTo is null)) {
                hexToMoveTo.locked = true;
                movingTimer.Start();
            }
        }

        private void HandleTimerExpired() {
            hub.Publish(new UnitMoved() {
                fromCoords = position.coords,
                toCoords = hexToMoveTo.coords,
                entity = parent
            });

            position.RemoveEntity();
            hexToMoveTo.AddEntity(parent);
            position = hexToMoveTo;
            hexToMoveTo.locked = false;
            hexToMoveTo = null;
            movingTimer.Stop();

            parent.GetComponent<StateComponent>().DoneActing();
        }
    }
}
