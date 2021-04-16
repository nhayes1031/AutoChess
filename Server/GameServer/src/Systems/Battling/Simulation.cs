using Server.Game.EC;
using Server.Game.EC.Components;
using Server.Game.Systems;
using System;

namespace Server.Game.Systems {
    public class Simulation {
        public Action<Guid> Victory;

        private Board board;

        public Simulation(Board board) {
            this.board = board;
        }

        public void Update(double time, double deltaTime) {
            foreach (var unit in board.GetUnits()) {
                TakeAction(unit, time, deltaTime);
            }

            var units = board.GetUnits();
            if (units.Count == 1) {
                Victory?.Invoke(units[0].GetComponent<TeamComponent>().Team);
            }
        }

        private void TakeAction(Entity unit, double time, double deltaTime) {
            var state = unit.GetComponent<StateComponent>();

            if (state.CanCast) {
                if (state.SpellTargetInRange) {
                    unit.GetComponent<SpellComponent>().StartCast();
                } else {
                    unit.GetComponent<MovementComponent>().StartMove(board);
                }
            }

            if (state.CanAttack) {
                if (state.AttackTargetInRange) {
                    unit.GetComponent<AttackComponent>().StartAttack();
                } else {
                    unit.GetComponent<MovementComponent>().StartMove(board);
                }
            }

            unit.Update(time, deltaTime, board);
        }
    }
}
