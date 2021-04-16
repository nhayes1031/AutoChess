using Server.Game.EC.Components;
using Server.Game.EC;
using System;
using Server.Game.Systems;
using PubSub;

namespace Server.Game {
    public static class CharacterFactory {
        public static CharacterData CreateFromName(string name) {
            return name switch {
                "Mage" => new Mage(),
                "Warrior" => new Warrior(),
                "Priest" => new Priest(),
                "Hunter" => new Hunter(),
                "Paladin" => new Paladin(),
                "Rogue" => new Rogue(),
                "Fighter" => new Fighter(),
                "Ranger" => new Ranger(),
                "Cleric" => new Cleric(),
                "Bard" => new Bard(),
                "Summoner" => new Summoner(),
                "Tank" => new Tank(),
                "Bladecaller" => new Bladecaller(),
                // Tier 2
                "Cultist" => new Cultist(),
                "Assassin" => new Assassin(),
                "Druid" => new Druid(),
                "Healer" => new Healer(),
                "Beastmaster" => new Beastmaster(),
                "Wizard" => new Wizard(),
                "Sorcerer" => new Sorcerer(),
                "Berserker" => new Berserker(),
                "Knight" => new Knight(),
                "Archon" => new Archon(),
                "Herald" => new Herald(),
                "Pirate" => new Pirate(),
                "Necromancer" => new Necromancer(),
                // Tier 3
                "Enchanter" => new Enchanter(),
                "Sage" => new Sage(),
                "Warlock" => new Warlock(),
                "Monk" => new Monk(),
                "Templar" => new Templar(),
                "Sentinel" => new Sentinel(),
                "Battlemage" => new Battlemage(),
                "Protector" => new Protector(),
                "Mystic" => new Mystic(),
                "Elementalist" => new Elementalist(),
                "Conjurer" => new Conjurer(),
                "Arbiter" => new Arbiter(),
                "Shaman" => new Shaman(),
                // Tier 4
                "Seer" => new Seer(),
                "Revenant" => new Revenant(),
                "Trickster" => new Trickster(),
                "Provoker" => new Provoker(),
                "Keeper" => new Keeper(),
                "Invoker" => new Invoker(),
                "Wanderer" => new Wanderer(),
                "Siren" => new Siren(),
                "Crusader" => new Crusader(),
                "Reaper" => new Reaper(),
                "Broodwarden" => new Broodwarden(),
                // Tier 5
                "Dreadnought" => new Dreadnought(),
                "Stalker" => new Stalker(),
                "Illusionist" => new Illusionist(),
                "Strider" => new Strider(),
                "Betrayer" => new Betrayer(),
                "Naturalist" => new Naturalist(),
                "Charlatan" => new Charlatan(),
                "Vindicator" => new Vindicator(),
                _ => null,
            };
        }

        public static Entity ConstructEntityFromCharacterData(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            return character switch {
                Mage => ConstructMage(position, character, playerId, simulationHub),
                Warrior => ConstructWarrior(position, character, playerId, simulationHub),
                Priest => ConstructPriest(position, character, playerId, simulationHub),
                Hunter => ConstructHunter(position, character, playerId, simulationHub),
                Paladin => ConstructPaladin(position, character, playerId, simulationHub),
                Rogue => ConstructRogue(position, character, playerId, simulationHub),
                Fighter => ConstructFighter(position, character, playerId, simulationHub),
                Ranger => ConstructRanger(position, character, playerId, simulationHub),
                Cleric => ConstructCleric(position, character, playerId, simulationHub),
                Bard => ConstructBard(position, character, playerId, simulationHub),
                Summoner => ConstructSummoner(position, character, playerId, simulationHub),
                Tank => ConstructTank(position, character, playerId, simulationHub),
                Bladecaller => ConstructBladecaller(position, character, playerId, simulationHub),
                Cultist => ConstructCultist(position, character, playerId, simulationHub),
                Assassin => ConstructAssassin(position, character, playerId, simulationHub),
                Druid => ConstructDruid(position, character, playerId, simulationHub),
                Healer => ConstructHealer(position, character, playerId, simulationHub),
                Beastmaster => ConstructBeastmaster(position, character, playerId, simulationHub),
                Wizard => ConstructWizard(position, character, playerId, simulationHub),
                Sorcerer => ConstructSorcerer(position, character, playerId, simulationHub),
                Berserker => ConstructBerserker(position, character, playerId, simulationHub),
                Knight => ConstructKnight(position, character, playerId, simulationHub),
                Archon => ConstructArchon(position, character, playerId, simulationHub),
                Herald => ConstructHerald(position, character, playerId, simulationHub),
                Pirate => ConstructPirate(position, character, playerId, simulationHub),
                Necromancer => ConstructNecromancer(position, character, playerId, simulationHub),
                Enchanter => ConstructEnchanter(position, character, playerId, simulationHub),
                Sage => ConstructSage(position, character, playerId, simulationHub),
                Warlock => ConstructWarlock(position, character, playerId, simulationHub),
                Monk => ConstructMonk(position, character, playerId, simulationHub),
                Templar => ConstructTemplar(position, character, playerId, simulationHub),
                Sentinel => ConstructSentinel(position, character, playerId, simulationHub),
                Battlemage => ConstructBattlemage(position, character, playerId, simulationHub),
                Protector => ConstructProtector(position, character, playerId, simulationHub),
                Mystic => ConstructMystic(position, character, playerId, simulationHub),
                Elementalist => ConstructElementalist(position, character, playerId, simulationHub),
                Conjurer => ConstructConjurer(position, character, playerId, simulationHub),
                Arbiter => ConstructArbiter(position, character, playerId, simulationHub),
                Shaman => ConstructShaman(position, character, playerId, simulationHub),
                Seer => ConstructSeer(position, character, playerId, simulationHub),
                Revenant => ConstructRevenant(position, character, playerId, simulationHub),
                Trickster => ConstructTrickster(position, character, playerId, simulationHub),
                Provoker => ConstructProvoker(position, character, playerId, simulationHub),
                Keeper => ConstructKeeper(position, character, playerId, simulationHub),
                Invoker => ConstructInvoker(position, character, playerId, simulationHub),
                Wanderer => ConstructWanderer(position, character, playerId, simulationHub),
                Siren => ConstructSiren(position, character, playerId, simulationHub),
                Crusader => ConstructCrusader(position, character, playerId, simulationHub),
                Reaper => ConstructReaper(position, character, playerId, simulationHub),
                Broodwarden => ConstructBroodwarden(position, character, playerId, simulationHub),
                Dreadnought => ConstructDreadnought(position, character, playerId, simulationHub),
                Stalker => ConstructStalker(position, character, playerId, simulationHub),
                Illusionist => ConstructIllusionist(position, character, playerId, simulationHub),
                Strider => ConstructStrider(position, character, playerId, simulationHub),
                Betrayer => ConstructBetrayer(position, character, playerId, simulationHub),
                Naturalist => ConstructNaturalist(position, character, playerId, simulationHub),
                Charlatan => ConstructCharlatan(position, character, playerId, simulationHub),
                Vindicator => ConstructVindicator(position, character, playerId, simulationHub),
                _ => throw new Exception("Can't construct an entity from character type " + character.GetType()),
            };
        }

        #region Tier 1          x13
        private static Entity ConstructMage(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 4,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructWarrior(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 10,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructPriest(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructHunter(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 4,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 50,
                MovementSpeed = 100,
                AttackPower = 2,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructPaladin(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 7,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructRogue(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 50,
                MovementSpeed = 100,
                AttackPower = 3,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructFighter(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 75,
                MovementSpeed = 100,
                AttackPower = 6,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructRanger(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 90,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructCleric(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 90,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructBard(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 90,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructSummoner(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructTank(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 150,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 125,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructBladecaller(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 90,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        #endregion
        #region Tier 2          x13
        private static Entity ConstructCultist(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructAssassin(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 90,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 70,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructDruid(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructHealer(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 3,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructBeastmaster(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 125,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructWizard(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 4,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 65,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructSorcerer(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 4,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 75,
                MovementSpeed = 100,
                AttackPower = 3,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructBerserker(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 50,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructKnight(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructArchon(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructHerald(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructPirate(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 85,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructNecromancer(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 120,
                MovementSpeed = 100,
                AttackPower = 3,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        #endregion
        #region Tier 3          x13
        private static Entity ConstructEnchanter(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructSage(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 90,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructWarlock(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 110,
                MovementSpeed = 100,
                AttackPower = 6,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructMonk(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructTemplar(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructSentinel(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 125,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructBattlemage(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 90,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructProtector(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 200,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 150,
                MovementSpeed = 100,
                AttackPower = 2,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructMystic(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 3,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructElementalist(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 75,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructConjurer(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 45,
                MovementSpeed = 100,
                AttackPower = 2,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructArbiter(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructShaman(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        #endregion
        #region Tier 4          x11
        private static Entity ConstructSeer(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 3,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructRevenant(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructTrickster(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructProvoker(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 125,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructKeeper(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 150,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructInvoker(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructWanderer(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructSiren(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 110,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructCrusader(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 125,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 6,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructReaper(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructBroodwarden(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 3,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 60,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        #endregion
        #region Tier 5          x8
        private static Entity ConstructDreadnought(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructStalker(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 8,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructIllusionist(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 4,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructStrider(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructBetrayer(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructNaturalist(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 2,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructCharlatan(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 100,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        private static Entity ConstructVindicator(HexCoords position, CharacterData character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = 125,
                AttackRange = 1,
                Armor = 1,
                MagicArmor = 1,
                AttackSpeed = 100,
                MovementSpeed = 100,
                AttackPower = 5,
                Name = character.Name,
                StarLevel = character.Level
            };
            return new Entity(
                new LocationComponent() { StartingCoords = position },
                new TeamComponent() { Team = playerId },
                stats,
                new SpellComponent(),
                new MovementComponent(simulationHub, stats),
                new AttackComponent(simulationHub, state, stats),
                state
            );
        }
        #endregion
    }
}
