using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using poker_game.Pages;

namespace poker_game.Pages
{
    /// <summary>
    /// Interaction logic for GamesLobby.xaml
    /// </summary>
    public partial class GamesLobby : Page
    {
        public ObservableCollection<GameViewModel> Games { get; set; }

        public GamesLobby()
        {
            InitializeComponent();
            Games = new ObservableCollection<GameViewModel>();
            LoadGamesFromDatabase();
            DataContext = this;
        }

        private void LoadGamesFromDatabase()
        {
            using (var dbContext = new GameData())
            {
                var games = dbContext.Games
                    .ToList()
                    .Select(g => new GameViewModel
                    {
                        GameId = g.GameId,
                        Players = string.Join(", ", g.Players.Select(p => p.Name))
                    });

                foreach (var game in games)
                {
                    Games.Add(game);
                }
            }
        }

        private void JoinGame_Click(object sender, RoutedEventArgs e)
        {
            Pages.Content = new Pages.Game();
        }

        public class GameViewModel
        {
            public int GameId { get; set; }
            public string GameName { get; set; }
            public string GameImage { get; set; }
            public string Players { get; set; }
        }
    }
}
