namespace Server.Game.EC.Components {
    public class StateComponent : Component {
        public bool CanAct { get; set; } = true;
        public bool SpellTargetInRange { get; set; }
        public Entity AttackTargetInRange { get; set; }

        public void StartActing() {
            CanAct = false;
        }

        public void DoneActing() {
            CanAct = true;
        }
    }
}
