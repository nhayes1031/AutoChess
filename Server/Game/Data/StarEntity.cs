using System;

namespace Server.Game {
    public class StarEntity {
        private readonly Breed breed;
        private int level;
        private int cost;
        private int health;
        private int attackDamage;

        public string Name => breed.Name;
        public int StarLevel => level;
        public int Cost => cost;
        public int Health => health;
        public int AttackRange => breed.AttackRange;
        public int AttackSpeed => breed.AttackSpeed;
        public int AttackDamage => AttackDamage;
        public int Armor => breed.Armor;
        public int MagicResist => breed.MagicResist;
        public int Movespeed => breed.Movespeed;

        public StarEntity(Breed character) {
            breed = character;
            health = character.Health;
            attackDamage = character.AttackDamage;
            cost = character.Cost;
            level = 1;
        }

        public void LevelUp() {
            health = (int)MathF.Round(health * 1.9f);
            attackDamage = (int)MathF.Round(attackDamage * 1.9f);
            cost *= 2;
            level++;
        }
    }
}
