using Lidgren.Network;
using PubSub;
using Server.Game.EC;
using Server.Game.EC.Components;
using Server.Game.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Systems {
    public class Battle {
        private readonly Board battlefield;
        private readonly Simulation simulation;
        private readonly Hub hub;
        private readonly Hub localHub;
        private readonly IPlayer player1;
        private readonly IPlayer player2;
        private bool finished;

        public bool Finished => finished;

        public Battle(IPlayer player1, IPlayer player2) {
            hub = Hub.Default;
            finished = false;
            localHub = new Hub();
            localHub.Subscribe<UnitMoved>(this, HandleUnitMoved);
            localHub.Subscribe<UnitAttacked>(this, HandleUnitAttacked);

            this.player1 = player1;
            this.player2 = player2;

            var player1CharacterEntities = ConstructCharacterEntities(player1);
            var player2CharacterEntities = ConstructCharacterEntities(player2);

            battlefield = new Board(player1CharacterEntities, player2CharacterEntities);
            battlefield.UnitDied += HandleUnitDied;
            Hub.Default.Publish(new SimulationCombatStarted() {
                bottom = player1,
                top = player2,
                units = battlefield.GetUnits()
            });

            simulation = new Simulation(battlefield);
            simulation.Victory += HandleVictory;
            simulation.Draw += HandleDraw;
        }

        public void Update(double time, double deltaTime) {
            if (!finished)
                simulation.Update(time, deltaTime);
        }

        public void ForceFinish() {
            HandleDraw(player1.Id, player2.Id);
        }

        private void HandleUnitDied(HexCoords coords) {
            Hub.Default.Publish(new SimulationUnitDied() {
                connections = new () { player1.Connection, player2.Connection },
                unit = coords
            });
        }

        private void HandleVictory(Guid winnerId) {
            finished = true;

            var winner = player1.Id == winnerId ? player1 : player2;
            var loser = player1.Id == winnerId ? player2 : player1;
            var damage = CalculateDamage(loser.Id);

            loser.Health -= damage;
            Hub.Default.Publish(new SimulationEndedInVictory() {
                winner = winner.Connection,
                loser = loser.Connection,
            });
        }

        private void HandleDraw(Guid participant1, Guid participant2) {
            finished = true;

            var participant1Damage = CalculateDamage(participant1);
            var participant2Damage = CalculateDamage(participant2);

            Hub.Default.Publish(new SimulationEndedInDraw() {
                participant1 = player1.Connection,
                participant1Damage = participant1Damage,
                participant2 = player2.Connection,
                participant2Damage = participant2Damage,
            });
        }

        private void HandleUnitMoved(UnitMoved e) {
            hub.Publish(new SimulationUnitMoved() {
                connections = new () { player1.Connection, player2.Connection },
                fromCoords = e.fromCoords,
                toCoords = e.toCoords
            });
        }

        private void HandleUnitAttacked(UnitAttacked e) {
            hub.Publish(new SimulationUnitAttacked() {
                connections = new List<NetConnection>() { player1.Connection, player2.Connection },
                attacker = e.attacker,
                defender = e.defender,
                damage = e.damage
            });
        }

        private int CalculateDamage(Guid playerId) {
            int damage = 0;
            var units = battlefield.GetUnits().Where(x => x.GetComponent<TeamComponent>().Team != playerId);
            foreach (var unit in units) {
                damage += unit.GetComponent<StatsComponent>().StarLevel;
            }
            return damage;
        }

        private List<Entity> ConstructCharacterEntities(IPlayer player) {
            var entities = new List<Entity>();
            foreach (var entry in player.Board) {
                var entity = CharacterFactory.ConstructEntityFromCharacterData(entry.Key, entry.Value, player.Id, localHub);
                entities.Add(entity);
            }
            return entities;
        }
    }
}
