using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_game
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Chips { get; set; }
        public int CurrentBet { get; set; }
        public bool Folded { get; set; }
        public bool BigBlind { get; set; }
        [NotMapped]
        public List<Card> Hand { get; } = new List<Card>();

        public int GameId { get; set; } // Foreign key
        public virtual Game Game { get; set; } // Navigation property

        public Player() { }
        public Player(string name, int chips)
        {
            Name = name;
            Hand = new List<Card>();
            Chips = chips;
            CurrentBet = 0;
            Folded = false;
            BigBlind = false;
        }

        public void DealCard(Card card)
        {
            Hand.Add(card);
        }

        public void ResetHand()
        {
            Hand.Clear();
            CurrentBet = 0;
            Folded = false;
        }

        public void PlaceBet(int amount)
        {
            if (amount <= Chips)
            {
                Chips -= amount;
                CurrentBet += amount;
            }
        }

        public void Fold()
        {
            Folded = true;
        }
    }
}
