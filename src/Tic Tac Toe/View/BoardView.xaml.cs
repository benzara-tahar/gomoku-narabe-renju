using AIEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tic_Tac_Toe.View
{

    public delegate void GomokuEventHandler(object sender, GomokuEventArgs args);

    public partial class BoardView : UserControl, INotifyPropertyChanged
    {
        #region Constructor
        public BoardView()
        {
            InitializeComponent();
            this.MouseLeftButtonUp += ProcessMouseClick;
            LineLength = 4;
            BoardSize = 10;
            SearchDepth = 2;
            WinAfter = 1;
            MinMaxSearch.HeuristicFunction = Heuristic.EstimateCost;
            AlphaBetaSearch.HeuristicFunction = Heuristic.EstimateCost;

        }
        #endregion

        #region events
        public event GomokuEventHandler PlayerTurnEvent = null;
        public event GomokuEventHandler ComputerTurnEvent = null;
        public event GomokuEventHandler GameOverEvent = null;


        private void OnPlayerTurn(object sender, GomokuEventArgs e)
        {
            var handler = PlayerTurnEvent;
            if (handler != null)
            {
                handler(sender, e);
            }
        }


        private void OnComputerTurn(object sender, GomokuEventArgs e)
        {
            var handler = ComputerTurnEvent;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        private void OnGameOver(object sender, GomokuEventArgs e)
        {
            var handler = GameOverEvent;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        //constants
        const int WIDTH = 546;


        #endregion


        #region game parameters        

        private int _ab_score;
        public int AlphaBetaScore
        {
            get { return _ab_score; }
            set
            {
                if (_ab_score != value)
                {
                    _ab_score = value;
                    OnPropertyChange("AlphaBetaScore");
                }
            }
        }

        private int _searchDepth;
        public int SearchDepth
        {
            get { return _searchDepth; }
            set
            {
                if (_searchDepth != value)
                {
                    _searchDepth = value;
                    OnPropertyChange("SearchDepth");
                }
            }
        }

        public void OnPropertyChange(string s)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(s));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private Board board = null;
        //indicates whether it is the computer turn to play
        public bool ComputerTurn { get; set; }

        public int BoardSize { get; set; }

        public int LineLength { get; set; }

        public bool IsPlayerBlack { get; set; } // the color of the player         

        public int WinAfter { get; set; }// the winner is the one who can aligne the first "WinAfter" Line

        //to keep track of the remained time for the computer and the player
        public TimeSpan Time { get; set; }

        private TimeSpan playerRemainedTime;

        private TimeSpan computerRemainedTime;

        private bool stopThreadFlag = false;

        private int emptySquaresCount = 0;

        private Move lastMove;

        private int _playerScore = 0;
        public int PlayerScore
        {
            get { return _playerScore; }
            set
            {
                if (_playerScore != value)
                {
                    _playerScore = value;
                    OnPropertyChange("PlayerScore");
                }
            }
        }

        private int _computerScore = 0;
        public int ComputerScore
        {
            get { return _computerScore; }
            set
            {
                if (_computerScore != value)
                {
                    _computerScore = value;
                    OnPropertyChange("ComputerScore");
                }
            }
        }


        private List<GomokuSequence> sequencesList = new List<GomokuSequence>();


        #endregion

        #region Methods

        #region Events methods

        //handle the player moves via the mouse
        void ProcessMouseClick(object sender, MouseButtonEventArgs e)
        {
            double squareSize = WIDTH / BoardSize;
            int x = (int)Math.Truncate(e.GetPosition(this).X / squareSize);
            int y = (int)Math.Truncate(e.GetPosition(this).Y / squareSize);

            DrawCircle(Gomoku.PLAYER, x, y);

        }

        #endregion

        #region Canvas drawing methods
        /// <summary>
        /// draw the grid 
        /// </summary>
        public void InitializeGrid()
        {

            canvas.Children.Clear();
            int WIDTH = 546;
            var step = WIDTH / BoardSize;
            for (int i = 1; i < BoardSize; i++)
            {

                var line = new Rectangle();
                var column = new Rectangle();

                line.Width = 0.70;
                line.Height = WIDTH;

                column.Width = WIDTH;
                column.Height = 0.70;

                line.Fill = Brushes.White;
                column.Fill = Brushes.White;

                canvas.Children.Add(line);
                Canvas.SetLeft(line, i * step);
                Canvas.SetTop(line, 0);

                canvas.Children.Add(column);
                Canvas.SetLeft(column, 0);
                Canvas.SetTop(column, i * step);
            }
        }



        /// <summary>
        /// update the UI 
        /// </summary>
        /// <param name="state">the state to draw (computer or player)</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        private void DrawCircle(char state, int x, int y)
        {
            //if (board[x, y] != 0) return;// if the square is not empty, quit

            Ellipse elipse = new Ellipse();
            double squareSize = WIDTH / BoardSize;
            elipse.Width = elipse.Height = squareSize - squareSize * 0.25;
            elipse.StrokeThickness = 2;

            if (state == Gomoku.PLAYER && !ComputerTurn)
            {
                if (board[x, y] != (int)Gomoku.NONE) return;
                OnComputerTurn(this, null);
                if (IsPlayerBlack)
                {
                    elipse.Fill = Brushes.Black;
                    elipse.Stroke = Brushes.White;
                }
                else
                {
                    elipse.Fill = Brushes.White;
                    elipse.Stroke = Brushes.Gray;
                }

                board[x, y] = Gomoku.PLAYER;
                emptySquaresCount--;
                lastMove = new Move(x, y, state);
                UpdateBoard(lastMove);
                ComputerTurn = true;

            }
            else if (ComputerTurn && Gomoku.COMPUTER == state)
            {

                OnPlayerTurn(this, null);

                if (IsPlayerBlack)//player plays with black
                {
                    elipse.Fill = Brushes.White;
                    elipse.Stroke = Brushes.Gray;
                }
                else
                {
                    elipse.Fill = Brushes.Black;
                    elipse.Stroke = Brushes.White;
                }

                board[x, y] = Gomoku.COMPUTER;
                emptySquaresCount--;
                lastMove = new Move(x, y, state);
                UpdateBoard(lastMove);
                ComputerTurn = false;

            }

            canvas.Children.Add(elipse);
            Canvas.SetLeft(elipse, (x + 0.125) * squareSize);
            Canvas.SetTop(elipse, (y + 0.125) * squareSize);

        }

        /// <summary>
        /// Draw the aligned sequeces
        /// </summary>
        /// <param nafme="move"> last played move</param>
        private void UpdateBoard(Move move)
        {

            char c = move.State;


            //cheking herizontal 4 sequences
            int s_len = 0,
                i = move.X,
                j = move.Y;
            char state = move.State;
            int p_start,
                p_end;
            sequencesList.Clear();

            //horizontal/*---------------------

            while (board[i, j] == state)
            {
                j++;
                s_len++;
                if (j == BoardSize)
                {
                    break;
                }
            }
            p_end = j - 1;
            j = move.Y;
            s_len--;
            while (board[i, j] == state)
            {
                j--;
                s_len++;
                if (j == -1)
                {
                    break;
                }
            }
            p_start = j + 1;

            //testing 
            if (s_len == LineLength)
            {
                sequencesList.Add(new GomokuSequence(new Move(p_start, i, state), new Move(p_end, i, state), state));
                if (state == Gomoku.COMPUTER)
                    ComputerScore++;
                else
                    PlayerScore++;

                //else
            }

            ///vertical--------------------------------
            s_len = 0;
            i = move.X;
            j = move.Y;
            while (board[i, j] == state)
            {
                i--;
                s_len++;
                if (i == -1)
                {
                    break;
                }
            }
            p_start = i + 1;
            i = move.X;
            s_len--;
            while (board[i, j] == state)
            {
                i++;
                s_len++;
                if (i == BoardSize)
                {
                    break;
                }
            }
            p_end = i - 1;

            //testing 
            if (s_len == LineLength)
            {
                sequencesList.Add(new GomokuSequence(new Move(j, p_start, state), new Move(j, p_end, state), state));
                if (state == Gomoku.COMPUTER)
                    ComputerScore++;
                else
                    PlayerScore++;

                //else
            }

            //diagonal-------------------------
            ///vertical--------------------------------
            int p_start_i;
            int p_start_j;
            int p_end_i;
            int p_end_j;
            s_len = 0;
            i = move.X;
            j = move.Y;
            while (board[i, j] == state)
            {
                i--;
                j--;
                s_len++;
                if (i == -1 || j == -1)
                {
                    break;
                }
            }
            //p_start_i = (s_len == LineLength) ? i + 1 : i;
            //p_start_j = (s_len == LineLength) ? j + 1 : j; 
            p_start_i = i + 1;
            p_start_j = j + 1;
            i = move.X;
            j = move.Y;
            s_len--;
            while (board[i, j] == state)
            {
                i++;
                j++;
                s_len++;
                if (i == BoardSize || j == BoardSize)
                {
                    break;
                }
            }

            //p_end_i =  (s_len == LineLength)? i-1: i;
            //p_end_j = (s_len == LineLength) ? j-1 : j;
            p_end_i = i - 1;
            p_end_j = j - 1;

            //testing 
            if (s_len == LineLength)
            {
                sequencesList.Add(new GomokuSequence(new Move(p_start_i, p_start_j, state), new Move(p_end_i, p_end_j, state), state));
                if (state == Gomoku.COMPUTER)
                    ComputerScore++;
                else
                    PlayerScore++;

            }
            ///2--------------------------------

            s_len = 0;
            i = move.X;
            j = move.Y;
            while (board[i, j] == state)
            {
                i--;
                j++;
                s_len++;
                if (i == -1 || j == BoardSize)
                {
                    break;
                }
            }
            //p_start_i = (s_len == LineLength) ? i + 1 : i;
            //p_start_j = (s_len == LineLength) ? j + 1 : j; 
            p_start_i = i + 1;
            p_start_j = j - 1;
            i = move.X;
            j = move.Y;
            s_len--;
            while (board[i, j] == state)
            {
                i++;
                j--;
                s_len++;
                if (i == BoardSize || j == -1)
                {
                    break;
                }
            }

            //p_end_i =  (s_len == LineLength)? i-1: i;
            //p_end_j = (s_len == LineLength) ? j-1 : j;
            p_end_i = i - 1;
            p_end_j = j + 1;

            //testing 
            if (s_len == LineLength)
            {
                sequencesList.Add(new GomokuSequence(new Move(p_start_i, p_start_j, state), new Move(p_end_i, p_end_j, state), state));
                if (state == Gomoku.COMPUTER)
                    ComputerScore++;
                else
                    PlayerScore++;

            }



            DrawSequences();
        }


        private void DrawSequences()
        {

            foreach (var sequence in sequencesList)
            {
                var rect = new Rectangle();
                //  MessageBox.Show(sequence.ToString());
                if (sequence.FirstCoord.Y == sequence.SecondCoord.Y) // vertical line 
                {
                    rect.Width = (WIDTH / BoardSize) * 0.25;
                    rect.Height = (WIDTH / BoardSize) * LineLength;
                    rect.StrokeThickness = 1;

                    rect.RadiusX = rect.RadiusY = 4;

                    if ((IsPlayerBlack && sequence.Owner == Gomoku.PLAYER) || ((!IsPlayerBlack && sequence.Owner == Gomoku.COMPUTER)))
                    {
                        rect.Fill = Brushes.Black;
                        rect.Stroke = Brushes.White;
                    }
                    else
                    {
                        rect.Fill = Brushes.White;
                        rect.Stroke = Brushes.Black;
                    }

                    canvas.Children.Add(rect);
                    Canvas.SetTop(rect, (WIDTH / BoardSize) * sequence.FirstCoord.X);
                    Canvas.SetLeft(rect, (WIDTH / BoardSize) * sequence.FirstCoord.Y + (WIDTH / BoardSize) * 0.385);
                    Canvas.SetZIndex(rect, 0);
                }
                else if (sequence.FirstCoord.X == sequence.SecondCoord.X)// horizantal line
                {
                    rect.Width = (WIDTH / BoardSize) * LineLength;
                    rect.Height = (WIDTH / BoardSize) * 0.25;
                    rect.StrokeThickness = 1;

                    rect.RadiusX = rect.RadiusY = 4;

                    if ((IsPlayerBlack && sequence.Owner == Gomoku.PLAYER) || ((!IsPlayerBlack && sequence.Owner == Gomoku.COMPUTER)))
                    {
                        rect.Fill = Brushes.Black;
                        rect.Stroke = Brushes.White;
                    }
                    else
                    {
                        rect.Fill = Brushes.White;
                        rect.Stroke = Brushes.Black;
                    }

                    canvas.Children.Add(rect);
                    Canvas.SetTop(rect, (WIDTH / BoardSize) * sequence.FirstCoord.X + (WIDTH / BoardSize) * 0.385);
                    Canvas.SetLeft(rect, (WIDTH / BoardSize) * sequence.FirstCoord.Y);
                    Canvas.SetZIndex(rect, 0);
                }
                else
                    // diagonal
                    if (sequence.FirstCoord.Y - sequence.SecondCoord.Y < 0)
                {
                    rect.Width = (WIDTH / BoardSize) * LineLength * Math.Sqrt(2);
                    rect.Height = (WIDTH / BoardSize) * 0.25;
                    rect.StrokeThickness = 1;

                    if ((IsPlayerBlack && sequence.Owner == Gomoku.PLAYER) || ((!IsPlayerBlack && sequence.Owner == Gomoku.COMPUTER)))
                    {
                        rect.Fill = Brushes.Black;
                        rect.Stroke = Brushes.White;
                    }
                    else
                    {
                        rect.Fill = Brushes.White;
                        rect.Stroke = Brushes.Black;
                    }

                    canvas.Children.Add(rect);
                    Canvas.SetTop(rect, (WIDTH / BoardSize) * sequence.FirstCoord.Y - (WIDTH / BoardSize) * 0.125);
                    Canvas.SetLeft(rect, (WIDTH / BoardSize) * sequence.FirstCoord.X);
                    Canvas.SetZIndex(rect, 1000);
                    //MessageBox.Show(sequence.ToString());
                    rect.RenderTransformOrigin = new Point(0, 0);
                    rect.RenderTransform = new RotateTransform(45);

                }
                else
                {
                    rect.Width = (WIDTH / BoardSize) * LineLength * Math.Sqrt(2);
                    rect.Height = (WIDTH / BoardSize) * 0.25;
                    rect.StrokeThickness = 1;

                    if ((IsPlayerBlack && sequence.Owner == Gomoku.PLAYER) || ((!IsPlayerBlack && sequence.Owner == Gomoku.COMPUTER)))
                    {
                        rect.Fill = Brushes.Black;
                        rect.Stroke = Brushes.White;
                    }
                    else
                    {
                        rect.Fill = Brushes.White;
                        rect.Stroke = Brushes.Black;
                    }

                    canvas.Children.Add(rect);

                    Canvas.SetTop(rect, (WIDTH / BoardSize) * sequence.FirstCoord.Y + (WIDTH / BoardSize) * 0.875);
                    Canvas.SetLeft(rect, (WIDTH / BoardSize) * sequence.FirstCoord.X);
                    Canvas.SetZIndex(rect, 1000);
                    rect.RenderTransformOrigin = new Point(0, 0);
                    rect.RenderTransform = new RotateTransform(-45);

                }
            }
        }

        #endregion


        /// <summary>
        /// initialize all the parameters and Start a new Game.
        /// </summary>
        public void Start()
        {



            ComputerTurn = !IsPlayerBlack;

            emptySquaresCount = BoardSize * BoardSize;

            lastMove = null;

            ComputerScore = PlayerScore = 0;
            //true ==> computer turn to paly
            board = new Board(BoardSize, true);

            if (IsPlayerBlack)//player starts first  
            {
                OnPlayerTurn(this, null);
            }
            else
            {
                OnComputerTurn(this, null);
            }

            //initializing time
            if (Time != TimeSpan.Zero)
            {
                playerRemainedTime = TimeSpan.FromMinutes(Time.Minutes);
                computerRemainedTime = TimeSpan.FromMinutes(Time.Minutes);
            }

            //set heuristic parameters
            Heuristic.BoardSize = BoardSize;
            Heuristic.LineLenght = LineLength;

            stopThreadFlag = false;


            //looping the game over a thread
            Task.Factory.StartNew(GameLoop);
        }

        private void GameLoop()
        {

            while (!GameOver())
            {

                Thread.Sleep(500);
                //wating for player to play
                while (!ComputerTurn) Thread.Sleep(500);

                if (GameOver()) break;

                //Computer Turn to make a move
                MakeMove();

            }
            if (ComputerScore > PlayerScore)
                Dispatcher.Invoke((Action)(() => OnGameOver(this, new GomokuEventArgs("YOU LOSE :(", Gomoku.LOSE))));

            else
                if (ComputerScore < PlayerScore)
                Dispatcher.Invoke((Action)(() => OnGameOver(this, new GomokuEventArgs("YOU WIN :)", Gomoku.WIN))));
            else
                Dispatcher.Invoke((Action)(() => OnGameOver(this, new GomokuEventArgs("DRAW GAME -_-", Gomoku.DRAW))));



        }
        /// <summary>
        /// return true if the game is over, false otherwise
        /// </summary>
        /// <returns></returns>
        private bool GameOver()
        {
            if (emptySquaresCount == 0 || WinAfter == ComputerScore || WinAfter == PlayerScore || stopThreadFlag)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stop the running thread and the game
        /// </summary>
        public void Stop()
        {
            stopThreadFlag = true;
            board.ClearSuccessors();
            board = null;

        }



        /// <summary>
        /// Called when it is the Computer Turn  to play
        /// </summary>
        private void MakeMove()
        {

            if (emptySquaresCount == 0) return;
            if (emptySquaresCount >= BoardSize * BoardSize - 1)
            {
                if (board[BoardSize / 2, BoardSize / 2] == Gomoku.NONE)
                {
                    Dispatcher.Invoke((Action)(() => DrawCircle(Gomoku.COMPUTER, BoardSize / 2, BoardSize / 2)));
                }
                else
                {
                    Dispatcher.Invoke((Action)(() => DrawCircle(Gomoku.COMPUTER, BoardSize / 2, (BoardSize / 2) - 1)));
                }
                return;
            }

            //int score = AlphaBetaSearch.Search(board, 1, -9999999, 9999999, true);
            //board = ((Board)AlphaBetaSearch.GetAlphaBetaMax(board)) as Board;

            MinMaxSearch.Search(board, true, SearchDepth);
            board = ((Board)MinMaxSearch.MaxMove(board)) as Board;

            AlphaBetaScore = board.GetValue();
            board.ClearSuccessors();
            board.SetValue(0);

            board.ComputerTurn = true;

            Dispatcher.Invoke((Action)(() => DrawCircle(board.CurrentMove.State, board.CurrentMove.X, board.CurrentMove.Y)));
        }

        #endregion


    }
}
