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
        private Deck deck;
        private Card[] communityCards;
        private List<Player> players;
        private int pot;
        private int currentPlayerIndex;
        private int smallBlind;
        private int bigBlind;
        private int currentBet;
        private PlayerAction playerAction;
        private int checkCount;
        private int smallBlindIndex;
        private int bigBlindIndex;
        private GameState currentState;

        public enum PlayerAction
        {
            None,
            Check,
            Call,
            Raise,
            Fold
        }

        private enum GameState
        {
            Preflop,
            Flop,
            Turn,
            River,
            Showdown
        }

        public MainWindow()
        {
            InitializeComponent();
            communityCards = new Card[5];
            using (var db = new GameData())
            {
                players = db.Players.ToList();
            }
            pot = 0;
            smallBlind = 10;
            bigBlind = 20;
            currentPlayerIndex = 0;
            smallBlindIndex = 0;
            bigBlindIndex = 1;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            deck = new Deck();
            deck.Shuffle();
            DealCards();
            UpdatePlayerDisplays();
            UpdateChipDisplays();
            UpdateCurrentPlayerDisplay();

            await StartGame();
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

            using (var context = new GameData())
            {
                context.SaveChanges();
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
                cardTextBlock.Text = player.Hand.ElementAt(i).UnicodeImage;
            }
        }

        private void UpdatePlayerDisplays()
        {
            for (int i = 0; i < players.Count; i++)
            {
                TextBlock playerTextBlock = GetPlayerTextBlock(i);
                playerTextBlock.Text = players[i].Name;
            }
        }

        private void UpdateChipDisplays()
        {
            for (int i = 0; i < players.Count; i++)
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
            TextBlock actionTextBlock = GetPlayerActionTextBlock(players.IndexOf(player));
            actionTextBlock.Text = action;
        }

        private void ClearPlayerActionTextBlocks()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].Folded)
                {
                    TextBlock actionTextBlock = GetPlayerActionTextBlock(i);
                    actionTextBlock.Text = string.Empty;
                }
            }
        }

        private void ClearCommunityCards()
        {
            for (int i = 0; i < communityCards.Length; i++)
            {
                TextBlock actionTextBlock = GetCommunityCardTextBlock(i);
                actionTextBlock.Text = string.Empty;
            }

            Array.Clear(communityCards, 0, communityCards.Length);

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
            // Check if all players have folded
            if (players.All(p => p.Folded))
            {
                ResetGame(); // Restart the game with new cards
                return;
            }

            do
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
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

        private async Task PerformBettingRound()
        {
            bool roundEnded = false;

            do
            {
                // Wait until player makes a move (clicks a button)
                while (playerAction == default(PlayerAction))
                {
                    await Task.Delay(100);
                }

                // Make a move
                await TakeTurn(currentPlayerIndex);
                if (IsBettingRoundOver())
                {
                    roundEnded = true;
                }

                // Reset player action to default
                playerAction = default(PlayerAction);
            } while (!roundEnded);

            ResetPlayerCurrentBet();
            await Task.Delay(500);
            ClearPlayerActionTextBlocks();
            NextPlayer();
            currentBet = 0;
            checkCount = 0;
        }

        private async Task TakeTurn(int currentPlayerIndex)
        {
            Player player = players[currentPlayerIndex];

            // Determine the call amount for the player
            int callAmount = GetCallAmount();

            switch (playerAction)
            {
                case PlayerAction.Check:
                    if (currentBet == 0)
                    {
                        UpdatePlayerAction(player, "Checked");
                        checkCount++;
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
                    if (!players.All(p => p.Folded))
                    {
                        UpdatePlayerAction(player, "Folded");
                    }

                    NextPlayer();
                    break;
            }
        }

        private bool IsBettingRoundOver()
        {
            int currentBetAmount = currentBet;
            bool allCalledOrChecked = true;
            int activePlayers = players.Count(p => !p.Folded);

            if (currentBetAmount == 0)
            {
                // When the current bet is 0, check if all active players have checked
                allCalledOrChecked = checkCount == activePlayers;
            }
            else
            {
                foreach (Player player in players)
                {
                    if (!player.Folded)
                    {
                        if (playerAction == PlayerAction.Check && checkCount != players.Count())
                        {
                            allCalledOrChecked = false;
                        }
                        else
                        {
                            if (player.CurrentBet < currentBetAmount)
                            {
                                // If at least one player hasn't called the current bet
                                allCalledOrChecked = false;
                                break;
                            }
                            // if BigBlind player called, the pot updates properly
                            if (player.BigBlind == true)
                            {
                                allCalledOrChecked = false;
                                player.BigBlind = false;
                                pot -= bigBlind;
                            }
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
                return callAmount;
            }
        }

        private async Task StartGame()
        {
            // Collect small blind and big blind
            CollectBlinds();

            // Set the current player to the player after the big blind
            currentPlayerIndex = (bigBlindIndex + 1) % players.Count;

            // Start the game loop
            currentState = GameState.Preflop;
            await GameLoop();
        }

        // Assign Small and Big blinds to players
        private void CollectBlinds()
        {
            players[smallBlindIndex].PlaceBet(smallBlind);
            UpdatePlayerAction(players[smallBlindIndex], $"Small Blind: {smallBlind}");
            pot += smallBlind;

            players[bigBlindIndex].PlaceBet(bigBlind);
            UpdatePlayerAction(players[bigBlindIndex], $"Big Blind: {bigBlind}");
            pot += bigBlind;
            currentBet = bigBlind;
            players[bigBlindIndex].BigBlind = true;
        }

        // Game logic
        private async Task GameLoop()
        {
            while (currentState != GameState.Showdown)
            {
                switch (currentState)
                {
                    case GameState.Preflop:
                        await PerformBettingRound();
                        AdvanceToNextState();
                        break;
                    case GameState.Flop:
                        RevealCommunityCards(0, 3);
                        await PerformBettingRound();
                        AdvanceToNextState();
                        break;
                    case GameState.Turn:
                        RevealCommunityCards(3, 1);
                        await PerformBettingRound();
                        AdvanceToNextState();
                        break;
                    case GameState.River:
                        RevealCommunityCards(4, 1);
                        await PerformBettingRound();
                        AdvanceToNextState();
                        break;
                }
            }

            // Determine the winner and update the UI
            Player winner = DetermineWinner();

            await UpdateWinnerDisplay(winner);
            winner.Chips += pot;
            UpdateChipDisplays();

            await Task.Delay(1000);

            // Reset the game
            ResetGame();
        }

        // Update winner display
        private async Task UpdateWinnerDisplay(Player winner)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                tblWinner.Text = $"Winner: {winner.Name}";
            }, DispatcherPriority.Normal);
        }

        // Advance to the next game state
        private void AdvanceToNextState()
        {
            currentState++;
        }

        // Reset the game when all players have folded or the game is over
        private async void ResetGame()
        {
            // Reset player action, each player's current bet, pot
            playerAction = default(PlayerAction);
            ResetPlayerCurrentBet();
            pot = 0;

            // Reset each player's folded status, hand, action text
            for (int i = 0; i < players.Count; i++)
            {
                TextBlock actionTextBlock = GetPlayerActionTextBlock(i);
                actionTextBlock.Text = string.Empty;
            }
            foreach (var player in players)
            {
                player.Folded = false;
                player.Hand.Clear();
            }

            // Rotate small blind, big blind indexes
            smallBlindIndex = (smallBlindIndex + 1) % players.Count;
            bigBlindIndex = (bigBlindIndex + 1) % players.Count;

            // Clear community cards displays, list
            ClearCommunityCards();

            // Initialize a new deck and shuffle it
            deck = new Deck();
            deck.Shuffle();

            // Deal new cards to players, community cards
            DealCards();

            // Update UI
            UpdatePotDisplay();

            // Start a new game
            await StartGame();
        }

        // Evaluate the hands of each player and determine the winner
        private Player DetermineWinner()
        {
            HandEvaluator evaluator = new HandEvaluator();
            Player winner = null;
            int highestScore = -1;

            foreach (Player player in players)
            {
                if (!player.Folded)
                {
                    List<Card> playerCards = new List<Card>(player.Hand);
                    playerCards.AddRange(communityCards);
                    int score = evaluator.EvaluateHand(playerCards);

                    if (score > highestScore)
                    {
                        highestScore = score;
                        winner = player;
                    }
                }
            }

            return winner;
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