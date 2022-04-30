namespace Tic_Tac_Toe
{
    public class Move
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char State { get; set; }

        public Move(int x, int y, char c)
        {

            this.X = x;
            this.Y = y;
            this.State = c;
        }
    }
}
