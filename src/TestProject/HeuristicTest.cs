using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tic_Tac_Toe;

namespace TestProject
{


    /// <summary>
    ///Classe de test pour HeuristicTest, destinée à contenir tous
    ///les tests unitaires HeuristicTest
    ///</summary>
    [TestClass()]
    public class HeuristicTest
    {



        /// <summary>
        ///Test pour EstimateCost
        ///</summary>
        [TestMethod()]
        public void EstimateCostTest()
        {


            Board b = new Board(6, true);
            Heuristic.BoardSize = 6;
            Heuristic.LineLenght = 4;
            char O = Gomoku.COMPUTER;
            char X = Gomoku.PLAYER;

            b[0, 0] = Gomoku.NONE;
            b[0, 1] = Gomoku.NONE;
            b[0, 2] = Gomoku.NONE;
            b[0, 3] = Gomoku.NONE;
            b[0, 4] = Gomoku.NONE;
            b[0, 5] = Gomoku.NONE;


            b[1, 0] = Gomoku.NONE;
            b[1, 1] = Gomoku.NONE;
            b[1, 2] = Gomoku.NONE;
            b[1, 3] = Gomoku.NONE;
            b[1, 4] = Gomoku.NONE;
            b[1, 5] = Gomoku.NONE;

            b[2, 0] = Gomoku.NONE;
            b[2, 1] = Gomoku.NONE;
            b[2, 2] = Gomoku.NONE;
            b[2, 3] = Gomoku.NONE;
            b[2, 4] = Gomoku.NONE;
            b[2, 5] = Gomoku.NONE;

            b[3, 0] = Gomoku.NONE;
            b[3, 1] = Gomoku.NONE;
            b[3, 2] = Gomoku.NONE;
            b[3, 3] = Gomoku.NONE;
            b[3, 4] = Gomoku.NONE;
            b[3, 5] = Gomoku.NONE;

            b[4, 0] = Gomoku.NONE;
            b[4, 1] = Gomoku.NONE;
            b[4, 2] = O;
            b[4, 3] = O;
            b[4, 4] = Gomoku.NONE;
            b[4, 5] = Gomoku.NONE;

            b[5, 0] = Gomoku.NONE;
            b[5, 1] = Gomoku.NONE;
            b[5, 2] = Gomoku.NONE;
            b[5, 3] = X;
            b[5, 4] = Gomoku.NONE;
            b[5, 5] = Gomoku.NONE;

            //|---|---|---|---|---|
            //|   | O | O | O |   | +600
            //|---|---|---|---|---|
            //|   |   | O | O |   | +5
            //|---|---|---|---|---|
            //| X | X | X |   | O | -40
            //|---|---|---|---|---|
            //| X |   | X | O | O | 0
            //|---|---|---|---|---|
            //|   | O | O | O | O | +1000
            //|---|---|---|---|---|
            //        -5          1000  +60

            //const int FULL_THREAT_SEQUENCE = 1000;  //N           O O O O
            //const int OPENED_THREAT_SEQUENCE = 600;  //N-1        . O O O . 
            //const int BROKEN_THREAT_SEQUENCE = 40;  //N-1         O O . O   or O . O O
            //const int HALF_OPENED_THREAT_SEQUENCE = 60;  //N-1    X O O O . or . O O O X     
            //const int WEACK_THREAT_SEQUENCE1 = 20;  //N-2         . . O O . . 
            //const int WEACK_THREAT_SEQUENCE2 = 5;  //N-2          . O O . X    or   X . O O .


            //

            int actualh = Heuristic.GetHorizantalThreatSequences(b);
            int expectedh = 40;

            int actualv = Heuristic.GetVerticalThreatSequences(b);
            int expectedv = 350;
            Assert.AreEqual(expectedh, actualh);

            Assert.AreEqual(expectedv, actualv);


        }

    }
}
