using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static poker_game.MainWindow;
using System.Data.Entity;
using System.Windows.Threading;
using System.Reflection;
using poker_game.Pages;

namespace poker_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string PlayerName { get; set; }
        public int ChipCount { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text) || string.IsNullOrWhiteSpace(txtChipCount.Text))
            {
                MessageBox.Show("Please enter a player name and chip count.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PlayerName = txtPlayerName.Text;
            ChipCount = int.Parse(txtChipCount.Text);

            Frame frame = new Frame();
            frame.Navigate(new GamesLobby(PlayerName, ChipCount));
            Content = frame;
        }
    }
}