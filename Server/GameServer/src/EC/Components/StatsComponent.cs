namespace Server.Game.EC.Components {
    public class StatsComponent : Component {
        public int Health { set; get; }
        public int AttackRange { set; get; }
        public int Armor { set; get; }
        public int MagicArmor { set; get; }
        public float AttackSpeed { set; get; }
        public float MovementSpeed { set; get; }
    }
}
