using NUnit.Framework;
using Server.Game.Systems;
using Server.Game.EC;
using Server.Game.EC.Components;
using System;
using System.Collections.Generic;

namespace GameTests {
    public class BoardTests {
        [Test]
        public void Should_return_true_if_board_contains_the_character_at_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var entity = new Entity();
            var coords = new HexCoords(0, 1);

            board.AddUnit(entity, coords);
            var actual = board.Contains(entity, coords);
            Assert.IsTrue(actual);
        }

        [Test]
        public void Should_return_false_if_the_board_does_not_contain_the_character_at_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var entity1 = new Entity();
            var entity2 = new Entity();
            var coords = new HexCoords(0, 1);

            board.AddUnit(entity1, coords);
            var actual = board.Contains(entity2, coords);
            Assert.IsFalse(actual);
        }

        [Test]
        public void Should_return_true_if_board_contains_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var coords = new HexCoords(0, 1);

            Assert.IsTrue(board.Contains(coords));
        }

        [Test]
        public void Should_return_false_if_board_does_not_contain_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var coords = new HexCoords(99, 99);

            Assert.IsFalse(board.Contains(coords));
        }

        [Test]
        public void Should_remove_the_unit_at_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var entity = new Entity();
            var coords = new HexCoords(0, 1);

            board.AddUnit(entity, coords);
            var actual = board.RemoveUnit(coords);
            Assert.AreEqual(entity, actual);
            Assert.IsFalse(board.Contains(entity, coords));
        }

        [Test]
        public void Should_return_an_enemy_when_there_are_multiple_allies() {
            var bottomLeft = new HexCoords(0, 0);
            var bottom = new HexCoords(0, 1);
            var coordInRange = new HexCoords(1, 0);

            var team1 = Guid.NewGuid();
            var stats = new StatsComponent();
            var entity1 = new Entity(
                new LocationComponent() { StartingCoords = bottomLeft },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = team1 },
                stats
            );
            var friendly1 = new Entity(
                new LocationComponent() { StartingCoords = bottom },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = team1 },
                stats
            );
            var entity2 = new Entity(
                new LocationComponent() { StartingCoords = coordInRange },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = Guid.NewGuid() },
                stats
            );

            var list1 = new List<Entity>() { entity1, friendly1, entity2 };
            var list2 = new List<Entity>() { };

            var battlefield = new Board(list1, list2);
            var actual = battlefield.FindEnemyInRange(bottomLeft, 1, team1);
            Assert.AreEqual(entity2, actual);
        }

        [Test]
        public void Should_return_an_enemy_in_range_when_one_is_there() {
            var bottomLeft = new HexCoords(0, 0);
            var coordInRange = new HexCoords(1, 0);

            var stats = new StatsComponent();
            var team1 = Guid.NewGuid();
            var entity1 = new Entity(
                new LocationComponent() { StartingCoords = bottomLeft },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = team1 },
                stats
            );
            var entity2 = new Entity(
                new LocationComponent() { StartingCoords = coordInRange },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = Guid.NewGuid() },
                stats
            );

            var list1 = new List<Entity>() { entity1, entity2 };
            var list2 = new List<Entity>();

            var battlefield = new Board(list1, list2);
            var actual = battlefield.FindEnemyInRange(bottomLeft, 1, team1);
            Assert.AreEqual(entity2, actual);
        }

        [Test]
        public void Should_return_null_if_there_are_no_enemies_in_range() {
            var bottomLeft = new HexCoords(0, 0);

            var stats = new StatsComponent();
            var team = Guid.NewGuid();
            var entity1 = new Entity(
                new LocationComponent() { StartingCoords = bottomLeft },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = team },
                stats
            );

            var list1 = new List<Entity>() { entity1 };
            var list2 = new List<Entity>();

            var battlefield = new Board(list1, list2);
            var actual = battlefield.FindEnemyInRange(bottomLeft, 1, team);
            Assert.AreEqual(null, actual);
        }

        [Test]
        public void Should_return_the_unit_at_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var entity = new Entity();
            var coords = new HexCoords(0, 1);

            board.AddUnit(entity, coords);
            var actual = board.GetUnit(coords);
            Assert.AreEqual(entity, actual);
            Assert.IsTrue(board.Contains(entity, coords));
        }

        [Test]
        public void Should_return_null_when_there_is_not_a_unit_at_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var coords = new HexCoords(0, 1);

            var actual = board.GetUnit(coords);
            Assert.AreEqual(null, actual);
        }

        [Test]
        public void Should_return_true_if_the_hex_is_empty_at_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var coords = new HexCoords(0, 1);
            Assert.IsTrue(board.IsHexEmpty(coords));
        }

        [Test]
        public void Should_return_false_if_the_hex_contains_a_unit_at_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var entity = new Entity();
            var coords = new HexCoords(0, 1);

            board.AddUnit(entity, coords);
            Assert.IsFalse(board.IsHexEmpty(coords));
        }

        [Test]
        public void Should_move_a_unit_from_one_coord_to_another() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var entity = new Entity();
            var coords1 = new HexCoords(0, 1);
            var coords2 = new HexCoords(3, 0);

            board.AddUnit(entity, coords1);
            Assert.IsTrue(board.Contains(entity, coords1));
            Assert.IsTrue(board.IsHexEmpty(coords2));

            board.MoveUnit(coords1, coords2);
            Assert.IsTrue(board.Contains(entity, coords2));
            Assert.IsTrue(board.IsHexEmpty(coords1));
        }

        [Test]
        public void Should_add_the_unit_to_the_board_at_the_coords() {
            var emptyList = new List<Entity>();
            var board = new Board(emptyList, emptyList);
            var entity = new Entity();
            var coords = new HexCoords(7, 1);

            board.AddUnit(entity, coords);
            Assert.IsTrue(board.Contains(entity, coords));

            var actual = board.RemoveUnit(coords);
            Assert.AreEqual(entity, actual);
        }

        [Test]
        public void Should_correctly_populate_the_battlefield() {
            var bottomLeft = new HexCoords(0, 0);
            var bottomRight = new HexCoords(7, 0);
            var topLeft = new HexCoords(-1, 3);
            var topRight = new HexCoords(6, 3);

            var stats = new StatsComponent();
            var entity1 = new Entity(
                new LocationComponent() { StartingCoords = bottomLeft },
                new MovementComponent(new PubSub.Hub(), stats),
                new StatsComponent()
            );
            var entity2 = new Entity(
                new LocationComponent() { StartingCoords = bottomRight },
                new MovementComponent(new PubSub.Hub(), stats),
                new StatsComponent()
            );
            var entity3 = new Entity(
                new LocationComponent() { StartingCoords = topLeft },
                new MovementComponent(new PubSub.Hub(), stats),
                new StatsComponent()
            );
            var entity4 = new Entity(
                new LocationComponent() { StartingCoords = topRight },
                new MovementComponent(new PubSub.Hub(), stats),
                new StatsComponent()
            );

            var player = new List<Entity>() { entity1, entity2, entity3, entity4 };
            var battlefield = new Board(player, player);
            Assert.IsTrue(battlefield.Contains(entity1, bottomLeft));
            Assert.IsTrue(battlefield.Contains(entity2, bottomRight));
            Assert.IsTrue(battlefield.Contains(entity3, topLeft));
            Assert.IsTrue(battlefield.Contains(entity4, topRight));

            Assert.IsTrue(battlefield.Contains(entity4, new HexCoords(-2, 4)));
            Assert.IsTrue(battlefield.Contains(entity3, new HexCoords(5, 4)));
            Assert.IsTrue(battlefield.Contains(entity2, new HexCoords(-3, 7)));
            Assert.IsTrue(battlefield.Contains(entity1, new HexCoords(4, 7)));
        }

        [Test]
        public void Should_return_a_hex_in_the_direction_of_another_hex() {
            var bottomLeft = new HexCoords(0, 0);
            var topRight = new HexCoords(-1, 3);
            
            var hex1 = new Hex(bottomLeft);
            var hex2 = new Hex(topRight);

            var emptyList = new List<Entity>();
            var battlefield = new Board(emptyList, emptyList);

            var actual = battlefield.GetHexInDirectionOf(hex1, hex2);
            Assert.AreEqual(new HexCoords(0, 1), actual.coords);
        }

        [Test]
        public void Should_return_a_hex_in_the_direction_of_the_nearest_enemy() {
            var bottomLeft = new HexCoords(0, 0);
            var topRight = new HexCoords(-1, 3);

            var stats = new StatsComponent();
            var entity1 = new Entity(
                new LocationComponent() { StartingCoords = bottomLeft },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = Guid.NewGuid() },
                stats
            );
            var entity2 = new Entity(
                new LocationComponent() { StartingCoords = topRight },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = Guid.NewGuid() },
                stats
            );

            var list1 = new List<Entity>() { entity1 };
            var list2 = new List<Entity>() { entity2 };

            var battlefield = new Board(list1, list2);
            var actual = battlefield.GetHexInDirectionOfClosestEnemy(entity1);
            Assert.AreEqual(new HexCoords(1, 0), actual.coords);
        }

        [Test]
        public void Should_return_a_hex_in_the_direction_of_the_nearest_enemy_when_there_are_multiple_allies() {
            var bottomLeft = new HexCoords(0, 0);
            var bottom = new HexCoords(0, 1);
            var topRight = new HexCoords(-1, 3);

            var stats = new StatsComponent();
            var team1 = Guid.NewGuid();
            var entity1 = new Entity(
                new LocationComponent() { StartingCoords = bottomLeft },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = team1 },
                stats
            );
            var entity2= new Entity(
                new LocationComponent() { StartingCoords = bottom },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = team1 },
                stats
            );
            var entity3 = new Entity(
                new LocationComponent() { StartingCoords = topRight },
                new MovementComponent(new PubSub.Hub(), stats),
                new TeamComponent() { Team = Guid.NewGuid() },
                stats
            );

            var list1 = new List<Entity>() { entity1, entity2 };
            var list2 = new List<Entity>() { entity3 };

            var battlefield = new Board(list1, list2);
            var actual = battlefield.GetHexInDirectionOfClosestEnemy(entity1);
            Assert.AreEqual(new HexCoords(1, 0), actual.coords);

            actual = battlefield.GetHexInDirectionOfClosestEnemy(entity2);
            Assert.AreEqual(new HexCoords(1, 1), actual.coords);
        }
    }
}
