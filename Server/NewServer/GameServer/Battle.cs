using System.Collections.Generic;

namespace Server.Game {
    public class Battle {
        private List<IUpdateable> units;
        private Board board;

        // TODO: 
        // Should contain the two players that are battling
        // Should contain a tilemap of the board
        // Should simulate each character on the board's turn each tick
        // Should report the outcome

        // DONOT:
        // Should not handle choosing who should be fighting
        // Should not handle sending out the packets

        //public Battle(PlayerData p1, PlayerData p2) {
        //    board = new Board(p1.Board, p2.Board);
        //}

        //public void Update(double time, double deltaTime) {
        //    // TODO: Do cool shit here
        //    foreach (var unit in board.units) {
        //        unit.Update(time, deltaTime);
        //    }
        //}
    }

    public interface IUpdateable {
        void Update(double time, double deltaTime);
    }
}
