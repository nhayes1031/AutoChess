namespace Server.Game {
    public static class CharacterFactory {
        public static Character CreateFromName(string name) {
            switch (name) {
                case "Mage":
                    return new Mage();
                case "Warrior":
                    return new Warrior();
                case "Priest":
                    return new Priest();
                case "Hunter":
                    return new Hunter();
                case "Paladin":
                    return new Paladin();
                case "Rogue":
                    return new Rogue();
                case "Fighter":
                    return new Fighter();
                case "Ranger":
                    return new Ranger();
                case "Cleric":
                    return new Cleric();
                case "Bard":
                    return new Bard();
                case "Summoner":
                    return new Summoner();
                case "Tank":
                    return new Tank();
                case "Bladecaller":
                    return new Bladecaller();
                // Tier 2
                case "Cultist":
                    return new Cultist();
                case "Assassin":
                    return new Assassin();
                case "Druid":
                    return new Druid();
                case "Healer":
                    return new Healer();
                case "Beastmaster":
                    return new Beastmaster();
                case "Wizard":
                    return new Wizard();
                case "Sorcerer":
                    return new Sorcerer();
                case "Berserker":
                    return new Berserker();
                case "Knight":
                    return new Knight();
                case "Archon":
                    return new Archon();
                case "Herald":
                    return new Herald();
                case "Pirate":
                    return new Pirate();
                case "Necromancer":
                    return new Necromancer();
                // Tier 3
                case "Enchanter":
                    return new Enchanter();
                case "Sage":
                    return new Sage();
                case "Warlock":
                    return new Warlock();
                case "Monk":
                    return new Monk();
                case "Templar":
                    return new Templar();
                case "Sentinel":
                    return new Sentinel();
                case "Battlemage":
                    return new Battlemage();
                case "Protector":
                    return new Protector();
                case "Mystic":
                    return new Mystic();
                case "Elementalist":
                    return new Elementalist();
                case "Conjurer":
                    return new Conjurer();
                case "Arbiter":
                    return new Arbiter();
                case "Shaman":
                    return new Shaman();
                // Tier 4
                case "Seer":
                    return new Seer();
                case "Revenant":
                    return new Revenant();
                case "Trickster":
                    return new Trickster();
                case "Provoker":
                    return new Provoker();
                case "Keeper":
                    return new Keeper();
                case "Invoker":
                    return new Invoker();
                case "Wanderer":
                    return new Wanderer();
                case "Siren":
                    return new Siren();
                case "Crusader":
                    return new Crusader();
                case "Reaper":
                    return new Reaper();
                case "Broodwarden":
                    return new Broodwarden();
                // Tier 5
                case "Dreadnought":
                    return new Dreadnought();
                case "Stalker":
                    return new Stalker();
                case "Illusionist":
                    return new Illusionist();
                case "Strider":
                    return new Strider();
                case "Betrayer":
                    return new Betrayer();
                case "Naturalist":
                    return new Naturalist();
                case "Charlatan":
                    return new Charlatan();
                case "Vindicator":
                    return new Vindicator();
                default:
                    return null;
            }
        }
    }
}
