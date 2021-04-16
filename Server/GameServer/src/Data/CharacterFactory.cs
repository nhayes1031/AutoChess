using Server.Game.EC.Components;
using Server.Game.EC;
using System;
using Server.Game.Systems;
using PubSub;

namespace Server.Game {
    public static class CharacterFactory {
        public static CharacterData CreateFromName(string name) {
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

        public static Entity ConstructEntityFromCharacterData(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            return character switch {
                Mage => ConstructMage(position, character, playerId, simulationHub),
                Warrior => ConstructWarrior(position, character, playerId),
                Priest => ConstructPriest(position, character, playerId),
                Hunter => ConstructHunter(position, character, playerId),
                Paladin => ConstructPaladin(position, character, playerId),
                Rogue => ConstructRogue(position, character, playerId),
                Fighter => ConstructFighter(position, character, playerId),
                Ranger => ConstructRanger(position, character, playerId),
                Cleric => ConstructCleric(position, character, playerId),
                Bard => ConstructBard(position, character, playerId),
                Summoner => ConstructSummoner(position, character, playerId),
                Tank => ConstructTank(position, character, playerId),
                Bladecaller => ConstructBladecaller(position, character, playerId),
                Cultist => ConstructCultist(position, character, playerId),
                Druid => ConstructDruid(position, character, playerId),
                Healer => ConstructHealer(position, character, playerId),
                Beastmaster => ConstructBeastmaster(position, character, playerId),
                Wizard => ConstructWizard(position, character, playerId),
                Sorcerer => ConstructSorcerer(position, character, playerId),
                Berserker => ConstructBerserker(position, character, playerId),
                Knight => ConstructKnight(position, character, playerId),
                Archon => ConstructArchon(position, character, playerId),
                Herald => ConstructHerald(position, character, playerId),
                Pirate => ConstructPirate(position, character, playerId),
                Necromancer => ConstructNecromancer(position, character, playerId),
                Enchanter => ConstructEnchanter(position, character, playerId),
                Sage => ConstructSage(position, character, playerId),
                Warlock => ConstructWarlock(position, character, playerId),
                Monk => ConstructMonk(position, character, playerId),
                Templar => ConstructTemplar(position, character, playerId),
                Sentinel => ConstructSentinel(position, character, playerId),
                Battlemage => ConstructBattlemage(position, character, playerId),
                Protector => ConstructProtector(position, character, playerId),
                Mystic => ConstructMystic(position, character, playerId),
                Elementalist => ConstructElementalist(position, character, playerId),
                Conjurer => ConstructConjurer(position, character, playerId),
                Arbiter => ConstructArbiter(position, character, playerId),
                Shaman => ConstructShaman(position, character, playerId),
                Seer => ConstructSeer(position, character, playerId),
                Revenant => ConstructRevenant(position, character, playerId),
                Trickster => ConstructTrickster(position, character, playerId),
                Provoker => ConstructProvoker(position, character, playerId),
                Keeper => ConstructKeeper(position, character, playerId),
                Invoker => ConstructInvoker(position, character, playerId),
                Wanderer => ConstructWanderer(position, character, playerId),
                Siren => ConstructSiren(position, character, playerId),
                Crusader => ConstructCrusader(position, character, playerId),
                Reaper => ConstructReaper(position, character, playerId),
                Broodwarden => ConstructBroodwarden(position, character, playerId),
                Dreadnought => ConstructDreadnought(position, character, playerId),
                Stalker => ConstructStalker(position, character, playerId),
                Illusionist => ConstructIllusionist(position, character, playerId),
                Strider => ConstructStrider(position, character, playerId),
                Betrayer => ConstructBetrayer(position, character, playerId),
                Naturalist => ConstructNaturalist(position, character, playerId),
                Charlatan => ConstructCharlatan(position, character, playerId),
                Vindicator => ConstructVindicator(position, character, playerId),
                _ => throw new Exception("Can't construct an entity from character type " + character.GetType()),
            };
        }

        #region Tier 1          x13
        private static Entity ConstructMage(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                new StatsComponent() {
                    Health = 100,
                    AttackRange = 1,
                    Armor = 1,
                    MagicArmor = 1,
                    AttackSpeed = 0.5f
                },
                new SpellComponent(),
                new MovementComponent() { hub = simulationHub },
                new AttackComponent(),
                new StateComponent()
            );
        }
        private static Entity ConstructWarrior(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructPriest(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructHunter(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructPaladin(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructRogue(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructFighter(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructRanger(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructCleric(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructBard(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructSummoner(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructTank(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructBladecaller(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        #endregion
        #region Tier 2          x13
        private static Entity ConstructCultist(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructDruid(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructHealer(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructBeastmaster(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructWizard(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructSorcerer(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructBerserker(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructKnight(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructArchon(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructHerald(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructPirate(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructNecromancer(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        #endregion
        #region Tier 3          x13
        private static Entity ConstructEnchanter(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructSage(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructWarlock(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructMonk(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructTemplar(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructSentinel(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructBattlemage(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructProtector(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructMystic(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructElementalist(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructConjurer(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructArbiter(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructShaman(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        #endregion
        #region Tier 4          x11
        private static Entity ConstructSeer(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructRevenant(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructTrickster(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructProvoker(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructKeeper(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructInvoker(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructWanderer(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructSiren(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructCrusader(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructReaper(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructBroodwarden(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        #endregion
        #region Tier 5          x8
        private static Entity ConstructDreadnought(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructStalker(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructIllusionist(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructStrider(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructBetrayer(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructNaturalist(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructCharlatan(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        private static Entity ConstructVindicator(HexCoords position, CharacterData character, Guid playerId) {
            return new Entity();
        }
        #endregion
    }
}
