using AIEngine;
using System;
using System.Collections.Generic;

namespace Tic_Tac_Toe
{
    public class Board : INode, ICloneable
    {
        #region Constructor
        public Board(int size, bool machineTurn)
        {
            this.Size = size;
            this.CurrentMove = new Move(0, 0, Gomoku.NONE);
            ComputerTurn = machineTurn;
            _board = new char[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //initialize the board
                    this[i, j] = Gomoku.NONE;
                }

            }
        }
        #endregion

        #region Properties
        public int Size { get; set; }
        public bool ComputerTurn { get; set; }
        public Move CurrentMove { get; set; }
        #endregion

        #region Indexors
        private char[,] _board;
        public char this[int x, int y]
        {
            set
            {
                _board[x, y] = value;
                successors = null;
            }
            get
            {
                return _board[x, y];
            }
        }
        #endregion

        #region Interface implementation
        List<INode> successors = null;
        public IEnumerable<INode> GetSccessors()
        {
            if (successors != null)
                return successors;
            successors = new List<INode>();



            // generate only intersting moves 
            //int delta = Size / 3;
            //int x = CurrentMove.X
            //    ,y = CurrentMove.Y;

            //while(delta>0 && x < Size-1)
            //{
            //    delta--;
            //    x++;
            //    if (this[x, y] == Gomoku.NONE)
            //    {
            //        var b = this.Clone() as Board;
            //        if (ComputerTurn)
            //        {
            //            b.CurrentMove = new Move(x, y, Gomoku.COMPUTER);
            //            b[x, y] = Gomoku.COMPUTER;
            //        }
            //        else
            //        {
            //            b.CurrentMove = new Move(y, y, Gomoku.PLAYER);
            //            b[x, y] = Gomoku.PLAYER;
            //        }
            //        b.ComputerTurn = !ComputerTurn;
            //        successors.Add(b);
            //    }
            //}

            //delta = Size / 3;
            //x = CurrentMove.X;

            //while (delta > 0 && x > 0)
            //{
            //    delta--;
            //    x--;
            //    if (this[x, y] == Gomoku.NONE)
            //    {
            //        var b = this.Clone() as Board;
            //        if (ComputerTurn)
            //        {
            //            b.CurrentMove = new Move(x, y, Gomoku.COMPUTER);
            //            b[x, y] = Gomoku.COMPUTER;
            //        }
            //        else
            //        {
            //            b.CurrentMove = new Move(y, y, Gomoku.PLAYER);
            //            b[x, y] = Gomoku.PLAYER;
            //        }
            //        b.ComputerTurn = !ComputerTurn;
            //        successors.Add(b);
            //    }
            //}

            // delta = Size / 3;
            // x = CurrentMove.X;
            //while (delta > 0 && y < Size-1)
            //{
            //    delta--;
            //    y++;
            //    if (this[x, y] == Gomoku.NONE)
            //    {
            //        var b = this.Clone() as Board;
            //        if (ComputerTurn)
            //        {
            //            b.CurrentMove = new Move(x, y, Gomoku.COMPUTER);
            //            b[x, y] = Gomoku.COMPUTER;
            //        }
            //        else
            //        {
            //            b.CurrentMove = new Move(y, y, Gomoku.PLAYER);
            //            b[x, y] = Gomoku.PLAYER;
            //        }
            //        b.ComputerTurn = !ComputerTurn;
            //        successors.Add(b);
            //    }
            //}

            //delta = Size / 3;

            //y = CurrentMove.Y;


            //while (delta > 0 && y > 0)
            //{
            //    delta--;
            //    y--;
            //    if (this[x, y] == Gomoku.NONE)
            //    {
            //        var b = this.Clone() as Board;
            //        if (ComputerTurn)
            //        {
            //            b.CurrentMove = new Move(x, y, Gomoku.COMPUTER);
            //            b[x, y] = Gomoku.COMPUTER;
            //        }
            //        else
            //        {
            //            b.CurrentMove = new Move(y, y, Gomoku.PLAYER);
            //            b[x, y] = Gomoku.PLAYER;
            //        }
            //        b.ComputerTurn = !ComputerTurn;
            //        successors.Add(b);
            //    }
            //}

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (this[i, j] != (int)Gomoku.NONE) continue;
                    var b = this.Clone() as Board;
                    if (ComputerTurn)
                    {
                        b.CurrentMove = new Move(i, j, Gomoku.COMPUTER);
                        b[i, j] = Gomoku.COMPUTER;
                    }
                    else
                    {
                        b.CurrentMove = new Move(i, j, Gomoku.PLAYER);
                        b[i, j] = Gomoku.PLAYER;
                    }
                    b.ComputerTurn = !ComputerTurn;

                    successors.Add(b);
                }
            }

            return successors;
        }

        public void ClearSuccessors()
        {
            if (successors != null)
            {
                foreach (var s in successors)
                {
                    s.ClearSuccessors();
                }
                this.successors.Clear();
                this.successors = null;
            }
        }

        public bool IsTerminal()
        {
            return false;
        }
        private int _value;
        public int GetValue()
        {
            return _value;
        }
        public void SetValue(int value)
        {
            _value = value;
        }

        public object Clone()
        {
            var b = new Board(Size, ComputerTurn);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    b[i, j] = this[i, j];
                }
            }
            b.CurrentMove = new Move(CurrentMove.X, CurrentMove.Y, CurrentMove.State);

            return b;
        }
        #endregion


        public void Show()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (this[i, j] == (int)Gomoku.NONE) Console.Write(" |");
                    if (this[i, j] == Gomoku.COMPUTER) Console.Write("O|");
                    if (this[i, j] == Gomoku.PLAYER) Console.Write("X|");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n--------------");
        }
    }
}
