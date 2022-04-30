using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Tic_Tac_Toe.View
{
    /// <summary>
    /// Logique d'interaction pour MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {

        public MainView()
        {
            InitializeComponent();

            //this.MouseLeftButtonDown += (m, M) => DragMove();
        }

        private void StartGame(object sender, System.Windows.RoutedEventArgs e)
        {

            board.PlayerTurnEvent += (s, ee) => (Resources["playerturn"] as Storyboard).Begin(this);
            board.ComputerTurnEvent += (s, ee) => (Resources["computerturn"] as Storyboard).Begin(this);
            board.GameOverEvent += (s, ee) =>
                {

                    var sb =
                    (Resources["gameOver"] as Storyboard);
                    sb.Completed += (f, h) => (Resources["backToMenu"] as Storyboard).Begin();
                    sb.Begin(this);
                    tbMessage.Text = ee.Message;
                    if (ee.Status == Gomoku.WIN)
                        tbMessage.Foreground = Brushes.Green;
                    else
                    if (ee.Status == Gomoku.LOSE)
                        tbMessage.Foreground = Brushes.Red;
                    else
                        tbMessage.Foreground = Brushes.White;

                };

            board.InitializeGrid();

            if (!toggleButton.IsChecked.GetValueOrDefault())
            {
                computer_time.Visibility = System.Windows.Visibility.Visible;
                player_time.Visibility = System.Windows.Visibility.Visible;
                board.Time = TimeSpan.FromMinutes(Convert.ToInt32(toggleButton.Tag.ToString().Split(' ')[0]));
            }
            else
            {
                computer_time.Visibility = System.Windows.Visibility.Hidden;
                player_time.Visibility = System.Windows.Visibility.Hidden;
                board.Time = TimeSpan.Zero;
            }
            board.Start();
        }



        private void Close(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void IncreaseTime(object sender, System.Windows.RoutedEventArgs e)
        {
            toggleButton.Tag = (Convert.ToInt32(toggleButton.Tag.ToString().Split(' ')[0]) + 1) + " mn";
        }

        private void DecreaseTime(object sender, System.Windows.RoutedEventArgs e)
        {
            var i = Convert.ToInt32(toggleButton.Tag.ToString().Split(' ')[0]);
            if (i <= 3) return;
            toggleButton.Tag = (i - 1) + " mn";
        }

        private void Pause(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void BackToManu(object sender, System.Windows.RoutedEventArgs e)
        {
            board.Stop();
            (Resources["backToMenu"] as Storyboard).Begin();
        }

        private void SetLevel(object sender, System.Windows.RoutedEventArgs e)
        {
            var radioBtn = sender as RadioButton;
            if (radioBtn.Content.ToString().ToUpper() == "EASY")
                board.SearchDepth = 2;
            if (radioBtn.Content.ToString().ToUpper() == "MEDIUM")
                board.SearchDepth = 4;
            if (radioBtn.Content.ToString().ToUpper() == "HARD")
                board.SearchDepth = 6;
        }

    }

}
