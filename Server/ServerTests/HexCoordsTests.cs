using NUnit.Framework;
using Server.Game;
using System.Numerics;

namespace ServerTests {
    public class HexCoordsTests {
        [Test]
        public void Should_add_hexcoords_correctly() {
            HexCoords coords1 = new HexCoords(1, 1);
            HexCoords coords2 = new HexCoords(2, 1);
            HexCoords actual = coords1 + coords2;
            Assert.AreEqual(actual.q, 3);
            Assert.AreEqual(actual.r, 2);
        }

        [Test]
        public void Should_add_offset_correctly() {
            HexCoords coords1 = new HexCoords(1, 1);
            HexCoords actual = coords1 + 2;
            Assert.AreEqual(actual.q, 3);
            Assert.AreEqual(actual.r, 3);
        }

        [Test]
        public void Should_subtract_hexcoords_correctly() {
            HexCoords coords1 = new HexCoords(2, 1);
            HexCoords coords2 = new HexCoords(1, 1);
            HexCoords actual = coords1 - coords2;
            Assert.AreEqual(actual.q, 1);
            Assert.AreEqual(actual.r, 0);
        }

        [Test]
        public void Should_subtract_offset_correctly() {
            HexCoords coords1 = new HexCoords(2, 1);
            HexCoords actual = coords1 - 1;
            Assert.AreEqual(actual.q, 1);
            Assert.AreEqual(actual.r, 0);
        }

        [Test]
        public void Should_offset_offset_correctly() {
            HexCoords coords1 = new HexCoords(2, 1);
            HexCoords actual = coords1 * 2;
            Assert.AreEqual(actual.q, 4);
            Assert.AreEqual(actual.r, 2);
        }

        [Test]
        public void Should_assert_equality_correctly() {
            HexCoords coords1 = new HexCoords(1, 1);
            HexCoords coords2 = new HexCoords(1, 1);
            HexCoords coords3 = new HexCoords(2, 1);
            Assert.IsTrue(coords1 == coords2);
            Assert.IsTrue(coords1.Equals(coords2));
            Assert.IsTrue(coords1.Equals((object)coords2));
            Assert.IsTrue(coords1.Equals(coords1, coords2));

            Assert.IsFalse(coords1 == coords3);
            Assert.IsFalse(coords1.Equals(coords3));
            Assert.IsFalse(coords1.Equals(5));
            Assert.IsFalse(coords1.Equals(coords1, coords3));
        }

        [Test]
        public void Should_assert_inequality_correctly() {
            HexCoords coords1 = new HexCoords(1, 1);
            HexCoords coords2 = new HexCoords(1, 1);
            HexCoords coords3 = new HexCoords(2, 1);

            Assert.IsFalse(coords1 != coords2);
            Assert.IsTrue(coords1 != coords3);
        }

        [Test]
        public void Should_to_string_with_array_notation() {
            HexCoords coords = new HexCoords(1, 2);
            Assert.AreEqual(coords.ToString(), "[1,2]");
        }

        [Test]
        public void Should_return_zerod_out_hex_coord() {
            HexCoords coords = HexCoords.Zero;
            Assert.AreEqual(coords.q, 0);
            Assert.AreEqual(coords.r, 0);
        }

        [Test]
        public void Should_return_the_right_neighbor() {
            HexCoords NECoord = new HexCoords(1, -1);
            HexCoords ECoord = new HexCoords(1, 0);
            HexCoords SECoord = new HexCoords(0, 1);
            HexCoords SWCoord = new HexCoords(-1, 1);
            HexCoords WCoord = new HexCoords(-1, 0);
            HexCoords NWCoord = new HexCoords(0, -1);
            HexCoords coords = HexCoords.Zero;
            var neighbor = coords.Neighbor(HexDirection.NE);
            Assert.AreEqual(NECoord, neighbor);

            neighbor = coords.Neighbor(HexDirection.E);
            Assert.AreEqual(ECoord, neighbor);

            neighbor = coords.Neighbor(HexDirection.SE);
            Assert.AreEqual(SECoord, neighbor);

            neighbor = coords.Neighbor(HexDirection.SW);
            Assert.AreEqual(SWCoord, neighbor);

            neighbor = coords.Neighbor(HexDirection.W);
            Assert.AreEqual(WCoord, neighbor);

            neighbor = coords.Neighbor(HexDirection.NW);
            Assert.AreEqual(NWCoord, neighbor);
        }

        [Test]
        public void Should_return_the_right_direction() {
            HexCoords NECoord = new HexCoords(1, -1);
            HexCoords ECoord = new HexCoords(1, 0);
            HexCoords SECoord = new HexCoords(0, 1);
            HexCoords SWCoord = new HexCoords(-1, 1);
            HexCoords WCoord = new HexCoords(-1, 0);
            HexCoords NWCoord = new HexCoords(0, -1);
            HexCoords coords = HexCoords.Zero;
            var neighbor = coords.Direction(HexDirection.NE);
            Assert.AreEqual(NECoord, neighbor);

            neighbor = coords.Direction(HexDirection.E);
            Assert.AreEqual(ECoord, neighbor);

            neighbor = coords.Direction(HexDirection.SE);
            Assert.AreEqual(SECoord, neighbor);

            neighbor = coords.Direction(HexDirection.SW);
            Assert.AreEqual(SWCoord, neighbor);

            neighbor = coords.Direction(HexDirection.W);
            Assert.AreEqual(WCoord, neighbor);

            neighbor = coords.Direction(HexDirection.NW);
            Assert.AreEqual(NWCoord, neighbor);
        }
        
        [Test]
        public void Should_convert_to_offset_coordinates_correctly() {
            var coord = new HexCoords(8, 5);
            Assert.AreEqual(new Vector2(10, 5), coord.ToOffset());
            coord = new HexCoords(7, 3);
            Assert.AreEqual(new Vector2(8, 3), coord.ToOffset());
            coord = new HexCoords(-1, 2);
            Assert.AreEqual(new Vector2(0, 2), coord.ToOffset());
        }
    }
}