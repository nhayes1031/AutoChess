using Server.Game.Systems;
using System;

namespace Server.Game.EC.Components {
    public class StatsComponent : Component {
        public Action<Entity> Died;

        public string Name { set; get; }
        private int health;
        public int Health { 
            set {
                health = value;
                if (health <= 0) {
                    Died?.Invoke(parent);
                }
            }
            get {
                return health;
            }
        }
        public int AttackRange { set; get; }
        public int AttackPower { set; get; }
        public int Armor { set; get; }
        public int MagicArmor { set; get; }
        public float AttackSpeed { set; get; }
        public float MovementSpeed { set; get; }
        public int StarLevel { get; set; }
    }
}
