using System;

namespace Tic_Tac_Toe
{
    public class GomokuSequence
    {

        public Move FirstCoord { get; set; }
        public Move SecondCoord { get; set; }
        public char Owner { get; set; }
        public bool Drawed { get; set; }


        public GomokuSequence(Move first, Move second, char owner)
        {
            FirstCoord = first;
            SecondCoord = second;
            Owner = owner;
            Drawed = false;

        }

        public GomokuSequence()
        {
            FirstCoord = new Move(0, 0, Gomoku.NONE);
            SecondCoord = new Move(0, 0, Gomoku.NONE);
            Drawed = false;

        }

        public override string ToString()
        {
            return String.Format("x1 = {0} y1 = {1}\nx2 = {2} y2 = {3}",
                FirstCoord.X, FirstCoord.Y, SecondCoord.X, SecondCoord.Y);
        }

    }
}
