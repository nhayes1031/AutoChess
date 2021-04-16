namespace Server.Game.EC.Components {
    public class StateComponent : Component {
        public bool IsCasting { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsMoving { get; set; }
        public bool SpellTargetInRange { get; set; }
        public bool CanCast { get; set; } = true;
        public bool AttackTargetInRange { get; set; }
        public bool CanAttack { get; set; }

        public void StartMoving() {
            IsMoving = true;

            IsCasting = false;
            IsAttacking = false;
            CanCast = false;
            CanAttack = false;
            SpellTargetInRange = false;
            AttackTargetInRange = false;
        }

        public void DoneMoving() {
            CanCast = true;

            IsMoving = false;
            IsCasting = false;
            IsAttacking = false;
            CanAttack = false;
            SpellTargetInRange = false;
            AttackTargetInRange = false;
        }
    }
}
