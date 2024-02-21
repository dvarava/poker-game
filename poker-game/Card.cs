using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_game
{
    public class Card
    {
        public string Rank { get; }
        public char Suit { get; }

        public Card(string rank, char suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public override string ToString()
        {
            return $"{Rank}({Suit})";
        }
    }
}
