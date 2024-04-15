﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_game
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public int Chips { get; set; }
        public int CurrentBet { get; set; }
        public int TotalBet { get; set; }
        public bool Folded { get; set; }

        public Player(string name, int chips)
        {
            Name = name;
            Hand = new List<Card>();
            Chips = chips;
            CurrentBet = 0;
            Folded = false;
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