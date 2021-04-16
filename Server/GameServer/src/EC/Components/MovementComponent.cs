using PubSub;
using Server.Game.Systems;
using Server.Game.Timers;

namespace Server.Game.EC.Components {
    public class MovementComponent : Component {
        public Hex position;
        public Hub hub;

        private Hex hexToMoveTo;
        private Timer movingTimer;

        public MovementComponent() {
            movingTimer = new Timer(500) {
                AutoReset = false
            };
            movingTimer.Elapsed += HandleTimerExpired;
        }

        public override void Update(double time, double deltaTime, Board board) {
            movingTimer.Update(deltaTime);
        }

        public void StartMove(Board board) {
            var state = parent.GetComponent<StateComponent>();
            state.StartMoving();

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

            parent.GetComponent<StateComponent>().DoneMoving();
        }
    }
}
