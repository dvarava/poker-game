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

// things to fix:
// add DetermineWinner()

namespace poker_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            Pages.Content = new Pages.GamesLobby();
        }
    }
}