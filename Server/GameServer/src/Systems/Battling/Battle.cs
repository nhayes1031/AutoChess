using Lidgren.Network;
using PubSub;
using Server.Game.EC;
using Server.Game.Messages;
using System;
using System.Collections.Generic;

namespace Server.Game.Systems {
    public class Battle {
        private Board battlefield;
        private Simulation simulation;
        private Hub hub;
        private Hub simulationHub;
        private PlayerData player1;
        private PlayerData player2;

        public Battle(PlayerData player1, PlayerData player2) {
            hub = Hub.Default;
            simulationHub = new Hub();
            simulationHub.Subscribe<UnitMoved>(this, HandleUnitMoved);

            this.player1 = player1;
            this.player2 = player2;

            var player1CharacterEntities = ConstructCharacterEntities(player1);
            var player2CharacterEntities = ConstructCharacterEntities(player2);

            battlefield = new Board(player1CharacterEntities, player2CharacterEntities);

            simulation = new Simulation(battlefield);
            simulation.Victory += HandleVictory;
        }

        public void Update(double time, double deltaTime) {
            simulation.Update(time, deltaTime);
        }

        private void HandleVictory(Guid victor) {

        }

        private void HandleUnitMoved(UnitMoved e) {
            hub.Publish(new SimulationUnitMoved() {
                connections = new List<NetConnection>() { player1.connection, player2.connection },
                fromCoords = e.fromCoords,
                toCoords = e.toCoords
            });
        }

        private List<Entity> ConstructCharacterEntities(PlayerData player) {
            var entities = new List<Entity>();
            foreach (var entry in player.Board) {
                var entity = CharacterFactory.ConstructEntityFromCharacterData(entry.Key, entry.Value, player.Id, simulationHub);
                entities.Add(entity);
            }
            return entities;
        }
    }
}
