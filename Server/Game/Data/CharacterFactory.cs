using Server.Game.EC.Components;
using Server.Game.EC;
using System;
using Server.Game.Systems;
using PubSub;

namespace Server.Game {
    public static class CharacterFactory {
        public static Entity ConstructEntityFromCharacterData(HexCoords position, StarEntity character, Guid playerId, Hub simulationHub) {
            var state = new StateComponent();
            var stats = new StatsComponent() {
                Health = character.Health,
                AttackRange = character.AttackRange,
                Armor = character.Armor,
                MagicArmor = character.MagicResist,
                AttackSpeed = character.AttackSpeed,
                MovementSpeed = character.Movespeed,
                AttackPower = character.AttackDamage,
                Name = character.Name,
                StarLevel = character.StarLevel
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
    }
}
