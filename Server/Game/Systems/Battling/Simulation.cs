using Server.Game.EC;
using Server.Game.EC.Components;
using System;

namespace Server.Game.Systems {
    public class Simulation {
        public Action<Guid> Victory;
        public Action<Guid, Guid> Draw;

        private readonly Board board;

        public Simulation(Board board) {
            this.board = board;

            board.Draw += HandleDraw;
            board.Victory += HandleVictory;
        }

        private void HandleDraw(Guid participant1, Guid participant2) {
            Draw?.Invoke(participant1, participant2);
        }

        private void HandleVictory(Guid winner) {
            Victory?.Invoke(winner);
        }

        public void Update(double time, double deltaTime) {
            foreach (var unit in board.GetUnits()) {
                TakeAction(unit, time, deltaTime);
            }
        }

        private void TakeAction(Entity unit, double time, double deltaTime) {
            var state = unit.GetComponent<StateComponent>();

            if (state.CanAct) {
                if (state.SpellTargetInRange) {
                    unit.GetComponent<SpellComponent>().StartCast();
                }
                else if (!(state.AttackTargetInRange is null)) {
                    unit.GetComponent<AttackComponent>().StartAttack();
                }
                else {
                    unit.GetComponent<MovementComponent>().StartMove(board);
                }
            }

            unit.Update(time, deltaTime, board);
        }
    }
}
