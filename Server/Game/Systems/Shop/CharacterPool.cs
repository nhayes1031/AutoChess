using System;
using System.Collections.Generic;

namespace Server.Game.Systems {

    /// <summary>
    /// TODO:
    /// Should contain all characters in the match
    /// As characters are purchases they should be removed from the pool
    /// There should be a weight pull option based on level
    /// </summary>
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

        private readonly Type[] Tier1Characters = new Type[] {
                typeof(Mage),
                typeof(Warrior),
                typeof(Priest),
                typeof(Hunter),
                typeof(Paladin),
                typeof(Rogue),
                typeof(Fighter),
                typeof(Ranger),
                typeof(Cleric),
                typeof(Bard),
                typeof(Summoner),
                typeof(Tank),
                typeof(Bladecaller)
        };
        private readonly Type[] Tier2Characters = new Type[] {
                typeof(Cultist),
                typeof(Assassin),
                typeof(Druid),
                typeof(Healer),
                typeof(Beastmaster),
                typeof(Wizard),
                typeof(Sorcerer),
                typeof(Berserker),
                typeof(Knight),
                typeof(Archon),
                typeof(Herald),
                typeof(Pirate),
                typeof(Necromancer)
        };
        private readonly Type[] Tier3Characters = new Type[] {
                typeof(Enchanter),
                typeof(Sage),
                typeof(Warlock),
                typeof(Monk),
                typeof(Templar),
                typeof(Sentinel),
                typeof(Battlemage),
                typeof(Protector),
                typeof(Mystic),
                typeof(Elementalist),
                typeof(Conjurer),
                typeof(Arbiter),
                typeof(Shaman)
        };
        private readonly Type[] Tier4Characters = new Type[] {
                typeof(Seer),
                typeof(Revenant),
                typeof(Trickster),
                typeof(Provoker),
                typeof(Keeper),
                typeof(Invoker),
                typeof(Wanderer),
                typeof(Siren),
                typeof(Crusader),
                typeof(Reaper),
                typeof(Broodwarden)
        };
        private readonly Type[] Tier5Characters = new Type[] {
                typeof(Dreadnought),
                typeof(Stalker),
                typeof(Illusionist),
                typeof(Strider),
                typeof(Betrayer),
                typeof(Naturalist),
                typeof(Charlatan),
                typeof(Vindicator)
        };

        private readonly PoolTier Tier1;
        private readonly PoolTier Tier2;
        private readonly PoolTier Tier3;
        private readonly PoolTier Tier4;
        private readonly PoolTier Tier5;

        private Random rnd;

        public CharacterPool() {
            Tier1 = new PoolTier(45, Tier1Characters);
            Tier2 = new PoolTier(30, Tier2Characters);
            Tier3 = new PoolTier(25, Tier3Characters);
            Tier4 = new PoolTier(15, Tier4Characters);
            Tier5 = new PoolTier(10, Tier5Characters);

            rnd = new Random();
        }

        public CharacterData GetCharacter(int level) {
            var roll = rnd.Next(0, 100);
            var pullChance = pullChanceBreakdown[level];

            if (roll <= pullChance.Tier1Percentage) {
                return PullCharacter(Tier1);
            }
            roll -= pullChance.Tier1Percentage;
            if (roll <= pullChance.Tier2Percentage) {
                return PullCharacter(Tier2);
            }
            roll -= pullChance.Tier2Percentage;
            if (roll <= pullChance.Tier3Percentage) {
                return PullCharacter(Tier3);
            }
            roll -= pullChance.Tier3Percentage;
            if (roll <= pullChance.Tier4Percentage) {
                return PullCharacter(Tier4);
            }
            roll -= pullChance.Tier4Percentage;
            if (roll <= pullChance.Tier5Percentage) {
                return PullCharacter(Tier5);
            }

            Logger.Error("Unabled to pull a character... Pull Chance Breakdown probably doesn't add up to 100");
            return null;
        }

        private CharacterData PullCharacter(PoolTier pool) {
            var roll = rnd.Next(0, pool.Length);
            return pool.Pull(roll);
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
            private List<CharacterData> pool;

            public int Length => pool.Count;

            public PoolTier(int occurrences, Type[] characters) {
                pool = new List<CharacterData>();

                foreach (var character in characters) {
                    for (int i = 0; i < occurrences; i++) {
                        pool.Add((CharacterData)Activator.CreateInstance(character));
                    }
                }
            }

            public CharacterData Pull(int index) {
                return pool[index];
            }

            public bool Remove(CharacterData character) {
                return pool.Remove(character);
            }
        }
    }
}
