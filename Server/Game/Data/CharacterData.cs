namespace Server.Game {
    public class CharacterData {
        public string Name;
        public int Cost;
        public int Level;

        public static bool operator ==(CharacterData a, CharacterData b) => (!(a is null)) && a.Equals(b);
        public static bool operator !=(CharacterData a, CharacterData b) => !a.Equals(b);
        public override bool Equals(object other) => other is CharacterData character && Equals(character);
        public bool Equals(CharacterData character) => Name == character.Name;
        public static bool Equals(CharacterData a, CharacterData b) => a.Equals(b);
        public override int GetHashCode() => Name.GetHashCode();
        public override string ToString() => $"[Name:{Name},Cost:{Cost},Level:{Level}]";
    }

    #region Tier 1          x13
    public class Mage : CharacterData { 
        public Mage() { 
            Name = "Mage";
            Cost = 1;
            Level = 1;
        } 
    }
    public class Warrior : CharacterData { 
        public Warrior() {
            Name = "Warrior";
            Cost = 1;
            Level = 1;
        }
    }
    public class Priest : CharacterData {
        public Priest() {
            Name = "Priest";
            Cost = 1;
            Level = 1;
        }
    }
    public class Hunter : CharacterData {
        public Hunter() {
            Name = "Hunter";
            Cost = 1;
            Level = 1;
        }
    }
    public class Paladin : CharacterData {
        public Paladin() {
            Name = "Paladin";
            Cost = 1;
            Level = 1;
        }
    }
    public class Rogue : CharacterData {
        public Rogue() {
            Name = "Rogue";
            Cost = 1;
            Level = 1;
        }
    }
    public class Fighter : CharacterData {
        public Fighter() {
            Name = "Fighter";
            Cost = 1;
            Level = 1;
        }
    }
    public class Ranger : CharacterData {
        public Ranger() {
            Name = "Ranger";
            Cost = 1;
            Level = 1;
        }
    }
    public class Cleric : CharacterData {
        public Cleric() {
            Name = "Cleric";
            Cost = 1;
            Level = 1;
        }
    }
    public class Bard : CharacterData {
        public Bard() {
            Name = "Bard";
            Cost = 1;
            Level = 1;
        }
    }
    public class Summoner : CharacterData {
        public Summoner() {
            Name = "Summoner";
            Cost = 1;
            Level = 1;
        }
    }
    public class Tank : CharacterData {
        public Tank() {
            Name = "Tank";
            Cost = 1;
            Level = 1;
        }
    }
    public class Bladecaller : CharacterData {
        public Bladecaller() {
            Name = "Bladecaller";
            Cost = 1;
            Level = 1;
        }
    }
    #endregion

    #region Tier 2          x13
    public class Cultist : CharacterData { 
        public Cultist() {
            Name = "Cultist";
            Cost = 2;
            Level = 1;
        }
    }
    public class Assassin : CharacterData {
        public Assassin() {
            Name = "Assassin";
            Cost = 2;
            Level = 1;
        }
    }
    public class Druid : CharacterData {
        public Druid() {
            Name = "Druid";
            Cost = 2;
            Level = 1;
        }
    }
    public class Healer : CharacterData {
        public Healer() {
            Name = "Healer";
            Cost = 2;
            Level = 1;
        }
    }
    public class Beastmaster : CharacterData {
        public Beastmaster() {
            Name = "Beastmaster";
            Cost = 2;
            Level = 1;
        }
    }
    public class Wizard : CharacterData {
        public Wizard() {
            Name = "Wizard";
            Cost = 2;
            Level = 1;
        }
    }
    public class Sorcerer : CharacterData {
        public Sorcerer() {
            Name = "Sorcerer";
            Cost = 2;
            Level = 1;
        }
    }
    public class Berserker : CharacterData {
        public Berserker() {
            Name = "Berserker";
            Cost = 2;
            Level = 1;
        }
    }
    public class Knight : CharacterData {
        public Knight() {
            Name = "Knight";
            Cost = 2;
            Level = 1;
        }
    }
    public class Archon : CharacterData {
        public Archon() {
            Name = "Archon";
            Cost = 2;
            Level = 1;
        }
    }
    public class Herald : CharacterData {
        public Herald() {
            Name = "Herald";
            Cost = 2;
            Level = 1;
        }
    }
    public class Pirate : CharacterData {
        public Pirate() {
            Name = "Pirate";
            Cost = 2;
            Level = 1;
        }
    }
    public class Necromancer : CharacterData {
        public Necromancer() {
            Name = "Necromancer";
            Cost = 2;
            Level = 1;
        }
    }
    #endregion

    #region Tier 3          x13
    public class Enchanter : CharacterData {
        public Enchanter() {
            Name = "Enchanter";
            Cost = 3;
            Level = 1;
        }
    }
    public class Sage : CharacterData {
        public Sage() {
            Name = "Sage";
            Cost = 3;
            Level = 1;
        }
    }
    public class Warlock : CharacterData {
        public Warlock() {
            Name = "Warlock";
            Cost = 3;
            Level = 1;
        }
    }
    public class Monk : CharacterData {
        public Monk() {
            Name = "Monk";
            Cost = 3;
            Level = 1;
        }
    }
    public class Templar : CharacterData {
        public Templar() {
            Name = "Templar";
            Cost = 3;
            Level = 1;
        }
    }
    public class Sentinel : CharacterData {
        public Sentinel() {
            Name = "Sentinel";
            Cost = 3;
            Level = 1;
        }
    }
    public class Battlemage : CharacterData {
        public Battlemage() {
            Name = "Battlemage";
            Cost = 3;
            Level = 1;
        }
    }
    public class Protector : CharacterData {
        public Protector() {
            Name = "Protector";
            Cost = 3;
            Level = 1;
        }
    }
    public class Mystic : CharacterData {
        public Mystic() {
            Name = "Mystic";
            Cost = 3;
            Level = 1;
        }
    }
    public class Elementalist : CharacterData {
        public Elementalist() {
            Name = "Elementalist";
            Cost = 3;
            Level = 1;
        }
    }
    public class Conjurer : CharacterData {
        public Conjurer() {
            Name = "Conjurer";
            Cost = 3;
            Level = 1;
        }
    }
    public class Arbiter : CharacterData {
        public Arbiter() {
            Name = "Arbiter";
            Cost = 3;
            Level = 1;
        }
    }
    public class Shaman : CharacterData {
        public Shaman() {
            Name = "Shaman";
            Cost = 3;
            Level = 1;
        }
    }
    #endregion

    #region Tier 4          x11
    public class Seer : CharacterData {
        public Seer() {
            Name = "Seer";
            Cost = 4;
            Level = 1;
        }
    }
    public class Revenant : CharacterData {
        public Revenant() {
            Name = "Revenant";
            Cost = 4;
            Level = 1;
        }
    }
    public class Trickster : CharacterData {
        public Trickster() {
            Name = "Trickster";
            Cost = 4;
            Level = 1;
        }
    }
    public class Provoker : CharacterData {
        public Provoker() {
            Name = "Provoker";
            Cost = 4;
            Level = 1;
        }
    }
    public class Keeper : CharacterData {
        public Keeper() {
            Name = "Keeper";
            Cost = 4;
            Level = 1;
        }
    }
    public class Invoker : CharacterData {
        public Invoker() {
            Name = "Invoker";
            Cost = 4;
            Level = 1;
        }
    }
    public class Wanderer : CharacterData {
        public Wanderer() {
            Name = "Wanderer";
            Cost = 4;
            Level = 1;
        }
    }
    public class Siren : CharacterData {
        public Siren() {
            Name = "Siren";
            Cost = 4;
            Level = 1;
        }
    }
    public class Crusader : CharacterData {
        public Crusader() {
            Name = "Crusader";
            Cost = 4;
            Level = 1;
        }
    }
    public class Reaper : CharacterData {
        public Reaper() {
            Name = "Reaper";
            Cost = 4;
            Level = 1;
        }
    }
    public class Broodwarden : CharacterData {
        public Broodwarden() {
            Name = "Broodwarden";
            Cost = 4;
            Level = 1;
        }
    }
    #endregion

    #region Tier 5          x8 
    public class Dreadnought : CharacterData {
        public Dreadnought() {
            Name = "Dreadnought";
            Cost = 5;
            Level = 1;
        }
    }
    public class Stalker : CharacterData {
        public Stalker() {
            Name = "Stalker";
            Cost = 5;
            Level = 1;
        }
    }
    public class Illusionist : CharacterData {
        public Illusionist() {
            Name = "Illusionist";
            Cost = 5;
            Level = 1;
        }
    }
    public class Strider : CharacterData {
        public Strider() {
            Name = "Strider";
            Cost = 5;
            Level = 1;
        }
    }
    public class Betrayer : CharacterData {
        public Betrayer() {
            Name = "Betrayer";
            Cost = 5;
            Level = 1;
        }
    }
    public class Naturalist : CharacterData {
        public Naturalist() {
            Name = "Naturalist";
            Cost = 5;
            Level = 1;
        }
    }
    public class Charlatan : CharacterData {
        public Charlatan() {
            Name = "Charlatan";
            Cost = 5;
            Level = 1;
        }
    }
    public class Vindicator : CharacterData {
        public Vindicator() {
            Name = "Vindicator";
            Cost = 5;
            Level = 1;
        }
    }
    #endregion
}
