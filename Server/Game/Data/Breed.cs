namespace Server.Game {
    public class Breed {
        public string Name { get; private set; }
        public int Cost { get; private set; }
        public int Health { get; private set; }
        public int Mana { get; private set; }
        public int StartingMana { get; private set; }
        public int Armor { get; private set; }
        public int MagicResist { get; private set; }
        public int AttackDamage { get; private set; }
        public float AttackSpeed { get; private set; }
        public int CriticalStrikeChance { get; private set; }
        public int AttackRange { get; private set; }
        public int Movespeed { get; private set; }

        public Breed(
            string name, 
            int cost, 
            int health, 
            int mana, 
            int startingMana, 
            int armor, 
            int magicResist, 
            int attackDamage, 
            float attackSpeed, 
            int criticalStrikeChance, 
            int attackRange, 
            int movespeed
         ) {
            Name = name;
            Cost = cost;
            Health = health;
            Mana = mana;
            StartingMana = startingMana;
            Armor = armor;
            MagicResist = magicResist;
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
            CriticalStrikeChance = criticalStrikeChance;
            AttackRange = attackRange;
            Movespeed = movespeed;
        }
        public Breed Clone() {
            return new Breed(
                Name,
                Cost,
                Health,
                Mana,
                StartingMana,
                Armor,
                MagicResist,
                AttackDamage,
                AttackSpeed,
                CriticalStrikeChance,
                AttackRange,
                Movespeed
            );
        }
    }
}
