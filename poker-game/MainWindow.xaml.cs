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

// things to fix:
// when only 1 player checked, next round starts
// current player is not working propely(because of too many NextPlayer(), which messes up who is Current Player)
// after MessageBox error, the betting round ends
// add DetermineWinner()

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
        private PlayerAction playerAction;
        private int checkCount;

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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            deck = new Deck();
            deck.Shuffle();
            DealCards();
            UpdatePlayerDisplays();
            UpdateChipDisplays();
            UpdateCurrentPlayerDisplay();
            //UpdateDeckDisplay();

            await StartGame();
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
            }

            // Deal and display  player cards
            foreach (var player in players)
            {
                player.DealCard(deck.DealCard());
                player.DealCard(deck.DealCard());
                UpdatePlayerCardDisplay(player);
            }
        }

        // Methods that are selecting the TextBlocks and updates their values
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

        private TextBlock GetPlayerActionTextBlock(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0: return tblPlayer1Action;
                case 1: return tblPlayer2Action;
                case 2: return tblPlayer3Action;
                case 3: return tblPlayer4Action;
                case 4: return tblPlayer5Action;
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

        private void UpdatePlayerAction(Player player, string action)
        {
            TextBlock actionTextBlock = GetPlayerActionTextBlock(Array.IndexOf(players, player));
            actionTextBlock.Text = action;
        }

        private void ClearPlayerActionTextBlocks()
        {
            for (int i = 0; i < 5; i++)
            {
                if (!players[i].Folded)
                {
                    TextBlock actionTextBlock = GetPlayerActionTextBlock(i);
                    actionTextBlock.Text = string.Empty;
                }
            }
        }

        private void UpdatePotDisplay()
        {
            tblPot.Text = $"Pot: {pot}";
        }

        private void UpdateCommunityCardDisplay(int index, Card card)
        {
            TextBlock cardTextBlock = GetCommunityCardTextBlock(index);
            cardTextBlock.Text = card.UnicodeImage;
        }

        private void RevealCommunityCards(int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                UpdateCommunityCardDisplay(i, communityCards[i]);
            }
        }

        private void NextPlayer()
        {
            do
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
            } while (players[currentPlayerIndex].Folded);

            UpdateCurrentPlayerDisplay();
        }

        private void ResetPlayerCurrentBet()
        {
            foreach (Player player in players)
            {
                player.CurrentBet = 0;
            }
        }

        public enum PlayerAction
        {
            None,
            Check,
            Call,
            Raise,
            Fold
        }

        private async Task PerformBettingRound()
        {
            bool roundEnded = false;

            do
            {
                Player currentPlayer = players[currentPlayerIndex];

                // Wait until player makes a move (clicks a button)
                while (playerAction == default(PlayerAction))
                {
                    await Task.Delay(100);
                }

                // Make a move
                await TakeTurn(currentPlayer);
                if (IsBettingRoundOver())
                {
                    roundEnded = true;
                }

                // Reset player action to default
                playerAction = default(PlayerAction);
            } while (!roundEnded);

            ResetPlayerCurrentBet();
            await Task.Delay(1000);
            ClearPlayerActionTextBlocks();
            NextPlayer();
            currentBet = 0;
            checkCount = 0;
        }

        private async Task TakeTurn(Player player)
        {
            // Determine the call amount for the player
            int callAmount = GetCallAmount();

            switch (playerAction)
            {
                case PlayerAction.Check:
                    if (currentBet == 0)
                    {
                        UpdatePlayerAction(player, "Checked");
                        checkCount++;
                        // idea is to add checking for 'check' in is betting round if bet is -1
                        NextPlayer();
                    }
                    else
                    {
                        MessageBox.Show("You cannot check. Someone has raised the bet.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case PlayerAction.Call:
                    while (true)
                    {
                        if (currentBet > 0)
                        {
                            if (player.CurrentBet == smallBlind)
                            {
                                player.PlaceBet(callAmount - smallBlind);
                                pot -= smallBlind;
                            }
                            else
                            {
                                player.PlaceBet(callAmount - player.CurrentBet);
                            }
                            pot += callAmount;
                            UpdatePlayerAction(player, $"Called {callAmount}");
                            UpdateChipDisplays();
                            UpdatePotDisplay();
                            NextPlayer();
                            break;
                        }
                        else
                        {
                            MessageBox.Show("You cannot call. The current bet is 0. Check instead", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            playerAction = PlayerAction.Check;
                            break;
                        }
                    }
                    break;

                case PlayerAction.Raise:
                    int raiseAmount = GetRaiseAmount(player, callAmount);
                    if (raiseAmount > callAmount && raiseAmount <= player.Chips)
                    {
                        player.PlaceBet(raiseAmount-player.CurrentBet);
                        pot += raiseAmount;
                        currentBet = raiseAmount; // Update the current bet
                        UpdatePlayerAction(player, $"Raised to {raiseAmount}");
                        UpdateChipDisplays();
                        UpdatePotDisplay();
                        NextPlayer();
                    }
                    else
                    {
                        MessageBox.Show("Invalid raise amount. It must be more than the current bet and less than your chips.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case PlayerAction.Fold:
                    player.Fold();
                    UpdatePlayerAction(player, "Folded");
                    NextPlayer();
                    break;
            }
        }

        private bool IsBettingRoundOver()
        {
            int currentBetAmount = currentBet;
            bool allCalledOrChecked = true;

            // I need to add if all players checked functionality here
            foreach (Player player in players)
            {
                if (!player.Folded)
                {
                    if (playerAction == PlayerAction.Check)
                    {
                        if (checkCount != 5)
                        {
                            allCalledOrChecked = false;
                            break;
                        }
                    }
                    else
                    {
                        if (player.CurrentBet < currentBetAmount)
                        {
                            // If at least one player hasn't called the current bet
                            allCalledOrChecked = false;
                            break;
                        }
                        // if BigBlind player called, the pot updates correctly
                        if (player.BigBlind == true)
                        {
                            allCalledOrChecked = false;
                            player.BigBlind = false;
                            pot -= bigBlind;
                        }
                    }
                }
            }

            return allCalledOrChecked;
        }

        private int GetCallAmount()
        {
            int callAmount = currentBet;
            return callAmount > 0 ? callAmount : 0;
        }

        private int GetRaiseAmount(Player player, int callAmount)
        {
            RaiseAmountDialog dialog = new RaiseAmountDialog();
            if (dialog.ShowDialog() == true)
            {
                return dialog.RaiseAmount;
            }
            else
            {
                return callAmount; // Set it to calling the current bet as a default for now
            }
        }

        private async Task StartGame()
        {
            // Collect small blind and big blind
            players[currentPlayerIndex].PlaceBet(smallBlind);
            UpdatePlayerAction(players[currentPlayerIndex], $"Small Blind: {smallBlind}");
            pot += smallBlind;
            NextPlayer();

            players[currentPlayerIndex].PlaceBet(bigBlind);
            UpdatePlayerAction(players[currentPlayerIndex], $"Big Blind: {bigBlind}");
            pot += bigBlind;
            currentBet = bigBlind;
            players[currentPlayerIndex].BigBlind = true;
            NextPlayer();

            // Pre-flop betting round
            await PerformBettingRound();

            // Reveal the Flop
            RevealCommunityCards(0, 3);

            // Flop betting round
            await PerformBettingRound();

            // Reveal the Turn
            RevealCommunityCards(3, 1);

            // Turn betting round
            await PerformBettingRound();

            // Reveal the River
            RevealCommunityCards(4, 1);

            // River betting round
            await PerformBettingRound();

            // Determine the winner, add pot to his balance and  start again
        }

        // Buttons click events
        private void BtnCall_Click(object sender, RoutedEventArgs e)
        {
            playerAction = PlayerAction.Call;
        }

        private void BtnRaise_Click(object sender, RoutedEventArgs e)
        {
            playerAction = PlayerAction.Raise;
        }

        private void BtnFold_Click(object sender, RoutedEventArgs e)
        {
            playerAction = PlayerAction.Fold;
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            playerAction = PlayerAction.Check;
        }
    }
}