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

namespace poker_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Card> deck;
        private List<Player> players;
        private List<Card> communityCards;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            DealCards();
        }

        private void InitializeGame()
        {
            deck = CreateDeck();
            tblDeck.Text = "\uD83C\uDCCF";

            players = new List<Player>
            {
                new Player("Player 1", 1000),
                new Player("Player 2", 1000),
                new Player("Player 3", 1000),
                new Player("Player 4", 1000),
                new Player("User", 1000)
            };

            communityCards = new List<Card>();
        }

        private List<Card> CreateDeck()
        {
            var ranks = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            var suits = new[] { '\u2665', '\u2666', '\u2663', '\u2660' }; // Hearts, Diamonds, Clubs, Spades

            var deck = new List<Card>();
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(new Card(rank, suit));
                }
            }

            return deck;
        }

        private void ShuffleDeck()
        {
            Random random = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        private void DealCards()
        {
            ShuffleDeck();

            foreach (var player in players)
            {
                player.Hand = deck.Take(2).ToList();
                deck.RemoveRange(0, 2);

                player.Card1 = player.Hand[0];
                player.Card2 = player.Hand[1];

            }

            communityCards.AddRange(deck.Take(5));
            deck.RemoveRange(0, 5);

            UpdateUI();
        }

        private void UpdateUI()
        {
            tblUserCard1.Text = players[4].Card1.ToString();
            tblUserCard2.Text = players[4].Card2.ToString();

            for (int i = 0; i <= 4; i++)
            {
                TextBlock stackTextBlock = FindName($"tblStackPlayer{i + 1}") as TextBlock;

                stackTextBlock.Text = players[i].Stack.ToString();
            }
        }
    }
}
