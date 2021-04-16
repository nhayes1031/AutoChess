using NUnit.Framework;
using Server.Game.EC;
using Server.Game.EC.Components;
using Server.Game.Systems;
using System;
using System.Collections.Generic;

namespace GameTests {
    public class SimulationTests {
        [Test]
        public void Should_update_all_units_on_the_board() {
            var team1 = Guid.NewGuid();
            var team2 = Guid.NewGuid();

            var entity1 = CreateEntity(team1, new HexCoords(0, 0));
            var entity2 = CreateEntity(team2, new HexCoords(0, 0));
     
            var board = new Board(new List<Entity> { entity1 }, new List<Entity> { entity2 });
            var simulation = new Simulation(board);

            simulation.Update(0.0d, 101);
            simulation.Update(0.0d, 101);
            simulation.Update(0.0d, 101);
            simulation.Update(0.0d, 101);
            simulation.Update(0.0d, 101);
            Assert.IsTrue(board.Contains(entity1, new HexCoords(2, 3)));
            Assert.IsTrue(board.Contains(entity2, new HexCoords(2, 4)));
        }

        [Test]
        public void Units_should_attack_one_another() {
            var team1 = Guid.NewGuid();
            var team2 = Guid.NewGuid();

            var entity1 = CreateEntity(team1, new HexCoords(0, 0));
            var entity2 = CreateEntity(team2, new HexCoords(0, 0));

            var board = new Board(new List<Entity> { entity1 }, new List<Entity> { entity2 });
            var simulation = new Simulation(board);

            for(int i = 0; i < 11; i++) {
                simulation.Update(0.0d, 101);
            }

            Assert.AreEqual(5, entity1.GetComponent<StatsComponent>().Health);
            Assert.AreEqual(5, entity2.GetComponent<StatsComponent>().Health);
        }

        [Test]
        public void Should_report_a_draw() {
            var team1 = Guid.NewGuid();
            var team2 = Guid.NewGuid();

            var entity1 = CreateEntity(team1, new HexCoords(0, 0));
            var entity2 = CreateEntity(team2, new HexCoords(0, 0));

            var board = new Board(new List<Entity> { entity1 }, new List<Entity> { entity2 });
            var simulation = new Simulation(board);

            var asserted = false;
            void handler() {
                asserted = true;
            }
            simulation.Draw += handler;

            for (int i = 0; i < 16; i++) {
                simulation.Update(0.0d, 101);
            }

            simulation.Draw -= handler;
            Assert.IsTrue(asserted);
        }

        [Test]
        public void Should_report_a_victory() {
            var team1 = Guid.NewGuid();
            var team2 = Guid.NewGuid();

            var entity1 = CreateEntity(team1, new HexCoords(0, 0));
            entity1.GetComponent<StatsComponent>().AttackPower = 2;
            var entity2 = CreateEntity(team2, new HexCoords(0, 0));

            var board = new Board(new List<Entity> { entity1 }, new List<Entity> { entity2 });
            var simulation = new Simulation(board);

            var asserted = false;
            void handler(Guid guid) {
                Assert.AreEqual(team1, guid);
                asserted = true;
            }
            simulation.Victory += handler;

            for (int i = 0; i < 11; i++) {
                simulation.Update(0.0d, 101);
            }

            simulation.Victory -= handler;
            Assert.IsTrue(asserted);
        }

        [Test]
        public void Should_work_with_multiple_units() {
            var team1 = Guid.NewGuid();
            var team2 = Guid.NewGuid();

            var entity1 = CreateEntity(team1, new HexCoords(0, 0));
            entity1.GetComponent<StatsComponent>().AttackPower = 2;
            var entity2 = CreateEntity(team1, new HexCoords(1, 0));
            var entity3 = CreateEntity(team1, new HexCoords(2, 0));
            var entity4 = CreateEntity(team1, new HexCoords(3, 0));
            var entity5 = CreateEntity(team1, new HexCoords(4, 0));

            var entity6 = CreateEntity(team2, new HexCoords(0, 0));
            var entity7 = CreateEntity(team2, new HexCoords(1, 0));
            var entity8 = CreateEntity(team2, new HexCoords(2, 0));
            var entity9 = CreateEntity(team2, new HexCoords(3, 0));
            var entity10 = CreateEntity(team2, new HexCoords(4, 0));

            var board = new Board(new List<Entity> { entity1, entity2, entity3, entity4, entity5 }, new List<Entity> { entity6, entity7, entity8, entity9, entity10 });
            var simulation = new Simulation(board);

            var asserted = false;
            void handler(Guid guid) {
                Assert.AreEqual(team1, guid);
                asserted = true;
            }
            simulation.Victory += handler;

            for (int i = 0; i < 40; i++) {
                simulation.Update(0.0d, 101);
            }

            simulation.Victory -= handler;
            Assert.IsTrue(asserted);
        }

        private static Entity CreateEntity(Guid team, HexCoords coords, int attackPower = 1) {
            var state = new StateComponent() { CanAct = true };
            var stats = new StatsComponent() {
                Health = 10,
                AttackRange = 1,
                AttackSpeed = 100,
                AttackPower = attackPower,
                MovementSpeed = 100
            };
            return new Entity(
                new TeamComponent() { Team = team },
                new LocationComponent() { StartingCoords = coords },
                state,
                new AttackComponent(new PubSub.Hub(), state, stats),
                new MovementComponent(new PubSub.Hub(), stats),
                new SpellComponent(),
                stats
            );
        }
    }
}
