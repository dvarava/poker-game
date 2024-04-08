using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_game
{
    public class Deck
    {
        private List<Card> cards;
        public List<Card> Cards { get { return cards; } }

        public Deck()
        {
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Face face in Enum.GetValues(typeof(Face)))
                {
                    cards.Add(new Card(suit, face));
                }
            }
        }

        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public Card DealCard()
        {
            if (cards.Count > 0)
            {
                Card card = cards.Last();
                cards.RemoveAt(cards.Count - 1);
                return card;
            }
            else
            {
                throw new InvalidOperationException("No more cards in the deck.");
            }
        }
    }
}
