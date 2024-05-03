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
using System.Data.Entity;

namespace poker_game.Pages
{
    /// <summary>
    /// Interaction logic for GamesLobby.xaml
    /// </summary>
    public partial class GamesLobby : Page
    {
        public ObservableCollection<GameViewModel> Games { get; set; }
        public static int SelectedGameId { get; set; }
        public string PlayerName { get; set; }
        public int ChipCount { get; set; }

        public GamesLobby(string playerName, int chipCount)
        {
            InitializeComponent();
            Games = new ObservableCollection<GameViewModel>();
            LoadGamesFromDatabase();
            DataContext = this;
            PlayerName = playerName;
            ChipCount = chipCount;
        }

        private void LoadGamesFromDatabase()
        {
            using (var db = new GameData())
            {
                var games = db.Games
                    .Include(g => g.Players)
                    .ToList()
                    .Select(g => new GameViewModel
                    {
                        GameId = g.GameId,
                        GameName = g.GameName,
                        GameImage = g.GameImage,
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
            var selectedGame = (GameViewModel)GameListView.SelectedItem;
            SelectedGameId = selectedGame.GameId;

            using (var dbContext = new GameData())
            {
                var game = dbContext.Games
                    .Include(g => g.Players)
                    .FirstOrDefault(g => g.GameId == selectedGame.GameId);

                if (game != null)
                {
                    if (game.Players.Count >= 5)
                    {
                        MessageBox.Show("The selected game is already full. Please choose another game.", "Game Full", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var player = new Player
                    {
                        Name = PlayerName,
                        Chips = ChipCount,
                        GameId = selectedGame.GameId
                    };

                    dbContext.Players.Add(player);
                    dbContext.SaveChanges();

                    List<Player> gamePlayers = game.Players.ToList();
                    Game gamePage = new Game(selectedGame.GameId, selectedGame.GameName, gamePlayers);
                    NavigationService.Navigate(gamePage);
                }
                else
                {
                    MessageBox.Show("The selected game was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
