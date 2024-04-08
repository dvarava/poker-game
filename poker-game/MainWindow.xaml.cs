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
        private Deck deck;
        private Card[] communityCards;
        private Player[] players;
        private int pot;
        private int currentPlayerIndex;
        private int smallBlind;
        private int bigBlind;
        private int currentBet;

        public MainWindow()
        {
            InitializeComponent();
            communityCards = new Card[5];
            players = new Player[]
            {
                new Player("Player 1", 1000),
                new Player("Player 2", 1000),
                new Player("Player 3", 1000),
                new Player("Player 4", 1000),
                new Player("You", 1000)
            };
            pot = 0;
            smallBlind = 10;
            bigBlind = 20;
            currentPlayerIndex = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            deck = new Deck();
            deck.Shuffle();
            DealCards();
            UpdatePlayerDisplays();
            UpdateChipDisplays();
            UpdateCurrentPlayerDisplay();
            UpdateDeckDisplay();
        }

        private void UpdateDeckDisplay()
        {
            tblDeck.Text = $"Cards in Deck: {deck.Cards.Count}";
        }

        private void DealCards()
        {
            // Deal community cards
            for (int i = 0; i < 5; i++)
            {
                communityCards[i] = deck.DealCard();
                UpdateCommunityCardDisplay(i, communityCards[i]);
            }

            // Deal player cards
            foreach (var player in players)
            {
                player.DealCard(deck.DealCard());
                player.DealCard(deck.DealCard());
                UpdatePlayerCardDisplay(player);
            }
        }

        private TextBlock GetCommunityCardTextBlock(int index)
        {
            switch (index)
            {
                case 0: return tblCommunityCard1;
                case 1: return tblCommunityCard2;
                case 2: return tblCommunityCard3;
                case 3: return tblCommunityCard4;
                case 4: return tblCommunityCard5;
                default: throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 4.");
            }
        }

        private void UpdateCommunityCardDisplay(int index, Card card)
        {
            TextBlock cardTextBlock = GetCommunityCardTextBlock(index);
            cardTextBlock.Text = card.UnicodeImage;
        }

        private TextBlock GetPlayerCardTextBlock(Player player, int cardIndex)
        {
            switch (player.Name)
            {
                case "Player 1": return cardIndex == 0 ? tblPlayerCard1 : tblPlayerCard2;
                case "Player 2": return cardIndex == 0 ? tblPlayerCard3 : tblPlayerCard4;
                case "Player 3": return cardIndex == 0 ? tblPlayerCard5 : tblPlayerCard6;
                case "Player 4": return cardIndex == 0 ? tblPlayerCard7 : tblPlayerCard8;
                case "You": return cardIndex == 0 ? tblYourCard1 : tblYourCard2;
                default: throw new ArgumentException("Invalid player name.", nameof(player));
            }
        }

        private TextBlock GetPlayerTextBlock(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0: return tblPlayer1;
                case 1: return tblPlayer2;
                case 2: return tblPlayer3;
                case 3: return tblPlayer4;
                case 4: return tblPlayer5;
                default: throw new ArgumentOutOfRangeException(nameof(playerIndex), "Index must be between 0 and 4.");
            }
        }

        private TextBlock GetPlayerChipTextBlock(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0: return tblStackPlayer1Chips;
                case 1: return tblStackPlayer2Chips;
                case 2: return tblStackPlayer3Chips;
                case 3: return tblStackPlayer4Chips;
                case 4: return tblStackPlayer5Chips;
                default: throw new ArgumentOutOfRangeException(nameof(playerIndex), "Index must be between 0 and 4.");
            }
        }

        private void UpdatePlayerCardDisplay(Player player)
        {
            for (int i = 0; i < player.Hand.Count; i++)
            {
                TextBlock cardTextBlock = GetPlayerCardTextBlock(player, i);
                cardTextBlock.Text = player.Hand[i].UnicodeImage;
            }
        }

        private void UpdatePlayerDisplays()
        {
            for (int i = 0; i < players.Length; i++)
            {
                TextBlock playerTextBlock = GetPlayerTextBlock(i);
                playerTextBlock.Text = players[i].Name;

                TextBlock chipTextBlock = GetPlayerChipTextBlock(i);
                chipTextBlock.Text = $"{players[i].Chips}";
            }
        }

        private void UpdateChipDisplays()
        {
            for (int i = 0; i < players.Length; i++)
            {
                TextBlock chipTextBlock = GetPlayerChipTextBlock(i);
                chipTextBlock.Text = $"{players[i].Chips}";
            }
        }

        private void UpdateCurrentPlayerDisplay()
        {
            TextBlock currentPlayerTextBlock = tblCurrentPlayer;
            currentPlayerTextBlock.Text = $"Current Player: {players[currentPlayerIndex].Name}";
        }

        private void BtnCall_Click(object sender, RoutedEventArgs e)
        {
            int callAmount = GetCallAmount();
            Player currentPlayer = players[currentPlayerIndex];
            currentPlayer.PlaceBet(callAmount);
            pot += callAmount;
            UpdateChipDisplays();
            UpdatePotDisplay();
            NextPlayer();
        }

        private void BtnRaise_Click(object sender, RoutedEventArgs e)
        {
            RaiseAmountDialog dialog = new RaiseAmountDialog();
            if (dialog.ShowDialog() == true)
            {
                int raiseAmount = dialog.RaiseAmount;
                int callAmount = GetCallAmount();
                if (raiseAmount >= callAmount && raiseAmount <= players[currentPlayerIndex].Chips)
                {
                    Player currentPlayer = players[currentPlayerIndex];
                    currentPlayer.PlaceBet(raiseAmount);
                    pot += raiseAmount;
                    UpdateChipDisplays();
                    UpdatePotDisplay();
                    NextPlayer();
                }
                else
                {
                    MessageBox.Show("Invalid raise amount. It must be at least as much as the current bet and not more than your chips.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnFold_Click(object sender, RoutedEventArgs e)
        {
            Player currentPlayer = players[currentPlayerIndex];
            currentPlayer.Fold();
            NextPlayer();
        }

        private int GetCallAmount()
        {
            Player currentPlayer = players[currentPlayerIndex];
            int callAmount = currentBet - currentPlayer.TotalBet;
            return callAmount > 0 ? callAmount : 0;
        }

        private void NextPlayer()
        {
            do
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
            } while (players[currentPlayerIndex].Folded);
        }

        private void UpdatePotDisplay()
        {
            tblPot.Text = $"Pot: {pot}";
        }
    }
}
