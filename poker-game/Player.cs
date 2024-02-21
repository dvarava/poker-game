using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_game
{
    public class Player
    {
        public string Name { get; }
        public int Stack { get; set; }
        public List<Card> Hand { get; set; }
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }

        public Player(string name, int initialStack)
        {
            Name = name;
            Hand = new List<Card>();
            Stack = initialStack;
        }
    }
}
