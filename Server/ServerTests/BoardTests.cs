using NUnit.Framework;
using Server.Game;

namespace ServerTests {
    public class BoardTests {
        [Test]
        public void Should_return_true_if_board_contains_the_character_at_the_coords() {
            var board = new Board();
            var character = new Mage();
            var coords = new HexCoords(0, 1);

            board.AddUnit(character, coords);
            var actual = board.Contains(character, coords);
            Assert.IsTrue(actual);
        }

        [Test]
        public void Should_return_false_if_the_board_does_not_contain_the_character_at_the_coords() {
            var board = new Board();
            var character1 = new Mage();
            var character2 = new Paladin();
            var coords = new HexCoords(0, 1);

            board.AddUnit(character1, coords);
            var actual = board.Contains(character2, coords);
            Assert.IsFalse(actual);
        }

        [Test]
        public void Should_return_true_if_board_contains_the_coords() {
            var board = new Board();
            var coords = new HexCoords(0, 1);

            Assert.IsTrue(board.Contains(coords));
        }

        [Test]
        public void Should_return_false_if_board_does_not_contain_the_coords() {
            var board = new Board();
            var coords = new HexCoords(99, 99);

            Assert.IsFalse(board.Contains(coords));
        }

        [Test]
        public void Should_remove_the_unit_at_the_coords() {
            var board = new Board();
            var character = new Mage();
            var coords = new HexCoords(0, 1);

            board.AddUnit(character, coords);
            var actual = board.RemoveUnit(coords);
            Assert.AreEqual(character, actual);
            Assert.IsFalse(board.Contains(character, coords));
        }

        [Test]
        public void Should_return_true_if_the_hex_is_empty_at_the_coords() {
            var board = new Board();
            var coords = new HexCoords(0, 1);
            Assert.IsTrue(board.IsHexEmpty(coords));
        }

        [Test]
        public void Should_return_false_if_the_hex_contains_a_unit_at_the_coords() {
            var board = new Board();
            var character = new Mage();
            var coords = new HexCoords(0, 1);

            board.AddUnit(character, coords);
            Assert.IsFalse(board.IsHexEmpty(coords));
        }

        [Test]
        public void Should_move_a_unit_from_one_coord_to_another() {
            var board = new Board();
            var character = new Mage();
            var coords1 = new HexCoords(0, 1);
            var coords2 = new HexCoords(3, 3);

            board.AddUnit(character, coords1);
            Assert.IsTrue(board.Contains(character, coords1));
            Assert.IsTrue(board.IsHexEmpty(coords2));

            board.MoveUnit(coords1, coords2);
            Assert.IsTrue(board.Contains(character, coords2));
            Assert.IsTrue(board.IsHexEmpty(coords1));
        }

        [Test]
        public void Should_add_the_unit_to_the_board_at_the_coords() {
            var board = new Board();
            var character = new Mage();
            var coords = new HexCoords(0, 1);

            board.AddUnit(character, coords);
            Assert.IsTrue(board.Contains(character, coords));

            var actual = board.RemoveUnit(coords);
            Assert.AreEqual(character, actual);
        }
    }
}
