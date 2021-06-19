using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Game.Systems {
    public class CharacterPool {
        private readonly PullChance[] pullChanceBreakdown = new PullChance[]{
            new PullChance(0, 0, 0, 0, 0),      // Level 0
            new PullChance(80, 20, 0, 0, 0),    // Level 1
            new PullChance(60, 40, 0, 0, 0),    // Level 2
            new PullChance(40, 40, 20, 0, 0),   // Level 3
            new PullChance(30, 40, 30, 0, 0),   // Level 4
            new PullChance(25, 30, 35, 10, 0),  // Level 5
            new PullChance(15, 30, 40, 15, 0),  // Level 6
            new PullChance(15, 25, 40, 20, 0),  // Level 7
            new PullChance(15, 20, 35, 25, 5),  // Level 8
            new PullChance(10, 20, 30, 30, 10), // Level 9
            new PullChance(10, 15, 25, 30, 20), // Level 10
        };

        private readonly PoolTier Tier1;
        private readonly PoolTier Tier2;
        private readonly PoolTier Tier3;
        private readonly PoolTier Tier4;
        private readonly PoolTier Tier5;

        private readonly Random rnd;

        public CharacterPool() {
            var characters = new Dictionary<int, List<Breed>>();
            GetCharacters().ForEach(c => {
                if (characters.ContainsKey(c.Cost))
                    characters[c.Cost].Add(c);
                else {
                    characters[c.Cost] = new List<Breed>();
                    characters[c.Cost].Add(c);
                }
            });

            Tier1 = new PoolTier(45, characters[1]);
            Tier2 = new PoolTier(30, characters[2]);
            Tier3 = new PoolTier(25, characters[3]);
            Tier4 = new PoolTier(15, characters[4]);
            Tier5 = new PoolTier(10, characters[5]);

            rnd = new Random();
        }

        private static List<Breed> GetCharacters() {
            return JsonConvert.DeserializeObject<List<Breed>>(File.ReadAllText(@"./Data/characters.json"));
        }

        public Breed[] Pop(int amount, int playerLevel) {
            var characters = new Breed[amount];
            for (int i = 0; i < amount; i++) {
                characters[i] = Pop(playerLevel);
            }
            return characters;
        }

        public void Push(Breed[] characters) {
            foreach (var character in characters) {
                if (!(character is null)) {
                    switch(character.Cost) {
                        case 1:
                            Tier1.Push(character);
                            break;
                        case 2:
                            Tier2.Push(character);
                            break;
                        case 3:
                            Tier3.Push(character);
                            break;
                        case 4:
                            Tier4.Push(character);
                            break;
                        case 5:
                            Tier5.Push(character);
                            break;
                    }
                }
            }
        }

        private Breed Pop(int playerLevel) {
            var pool = GetPool(playerLevel);
            return pool.Pop();
        }

        private PoolTier GetPool(int playerLevel) {
            var roll = rnd.Next(0, 100);
            var pullChance = pullChanceBreakdown[playerLevel];

            if (roll <= pullChance.Tier1Percentage) {
                return Tier1;
            }
            roll -= pullChance.Tier1Percentage;
            if (roll <= pullChance.Tier2Percentage) {
                return Tier2;
            }
            roll -= pullChance.Tier2Percentage;
            if (roll <= pullChance.Tier3Percentage) {
                return Tier3;
            }
            roll -= pullChance.Tier3Percentage;
            if (roll <= pullChance.Tier4Percentage) {
                return Tier4;
            }
            roll -= pullChance.Tier4Percentage;
            if (roll <= pullChance.Tier5Percentage) {
                return Tier5;
            }

            Logger.Error("Unable to get pool... Pull Chance Breakdown probably doesn't add up to 100");
            return null;
        }

        private class PullChance {
            public int Tier1Percentage { get; private set; }
            public int Tier2Percentage { get; private set; }
            public int Tier3Percentage { get; private set; }
            public int Tier4Percentage { get; private set; }
            public int Tier5Percentage { get; private set; }

            public PullChance(int tier1Percent, int tier2Percent, int tier3Percent, int tier4Percent, int tier5Percent) {
                Tier1Percentage = tier1Percent;
                Tier2Percentage = tier2Percent;
                Tier3Percentage = tier3Percent;
                Tier4Percentage = tier4Percent;
                Tier5Percentage = tier5Percent;
            }
        }

        private class PoolTier {
            private List<Breed> pool;
            private Random rnd;

            public PoolTier(int occurrences, List<Breed> characters) {
                rnd = new Random();
                pool = new List<Breed>();

                foreach (var character in characters) {
                    for (int i = 0; i < occurrences; i++) {
                        pool.Add(character.Clone());
                    }
                }
            }

            public Breed Pop() {
                var index = rnd.Next(0, pool.Count - 1);
                var character = pool[index];
                pool.Remove(character);
                return character;
            }

            public void Push(Breed character) {
                pool.Add(character);
            }
        }
    }
}
