using System.Linq;

namespace Server.Game {

    public class PlayerData {
        public int Level;
        public int Gold;
        public int XP;
        public CharacterList Bench;
        public CharacterList Board;
        public CharacterList Shop;

        public PlayerData() {
            Level = 1;
            Gold = 0;
            XP = 0;

            Shop = new CharacterList(5);
            Bench = new CharacterList(10);
            Board = new CharacterList(32);
        }

        public void AddReward(Reward reward) {
            Gold += reward.Gold;
            XP += reward.XP;
        }

        public void UpdateShop(Character[] newShop) {
            if (newShop.Length == 5) {
                Shop.Clear();
                Shop.AddRange(newShop);
            } else {
                Logger.Error("Tried to assign a new shop that did not contain 5 elements.");
            }
        }

        public string[] GetShopAsStringArray() {
            return Shop.List.Select(x => x.Name).ToArray();
        }

        // TODO: Add more robust logic around the shop and the board. 
        // Maybe turn them into classes?
        // TODO: There is an issue here when removing a unit when there are two of them in the array
        public bool Purchase(string name) {
            var character = CharacterFactory.CreateFromName(name);
            if (character != null) {
                if (Shop.Contains(character)) {
                    Shop.Remove(character);

                    Bench.Add(character);
                    return true;
                } else {
                    Logger.Error("Someone tried to request a character that wasn't in their shop!");
                }
            } else {
                Logger.Error("Someone tried to request a character that didn't exist!");
            }
            return false;
        }
    }

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
