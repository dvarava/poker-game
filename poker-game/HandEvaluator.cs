using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_game
{
    public class HandEvaluator
    {
        public int EvaluateHand(List<Card> cards)
        {
            cards.Sort((x, y) => y.Face.CompareTo(x.Face)); // Sort cards by face in descending order

            if (IsRoyalFlush(cards)) return 10000;
            if (IsStraightFlush(cards)) return 9000 + (int)cards[0].Face;
            if (IsFourOfAKind(cards)) return 8000 + (int)cards[1].Face;
            if (IsFullHouse(cards)) return 7000 + (int)cards[2].Face;
            if (IsFlush(cards)) return 6000 + (int)cards[0].Face;
            if (IsStraight(cards)) return 5000 + (int)cards[0].Face;
            if (IsThreeOfAKind(cards)) return 4000 + (int)cards[2].Face;
            if (IsTwoPair(cards)) return 3000 + (int)cards[1].Face + (int)cards[3].Face;
            if (IsPair(cards)) return 2000 + (int)cards[1].Face;
            return 1000 + (int)cards[0].Face; // High card
        }

        private bool IsRoyalFlush(List<Card> cards)
        {
            // Check if all cards are the same suit and the faces are 10, J, Q, K, A
            return IsFlush(cards) && cards[0].Face == Face.Ace && cards[1].Face == Face.King && cards[2].Face == Face.Queen && cards[3].Face == Face.Jack && cards[4].Face == Face.Ten;
        }

        private bool IsStraightFlush(List<Card> cards)
        {
            // Check if all cards are the same suit and the faces are in sequence
            return IsFlush(cards) && IsStraight(cards);
        }

        private bool IsFourOfAKind(List<Card> cards)
        {
            // Check if four cards have the same face
            return cards[0].Face == cards[1].Face && cards[1].Face == cards[2].Face && cards[2].Face == cards[3].Face ||
                   cards[1].Face == cards[2].Face && cards[2].Face == cards[3].Face && cards[3].Face == cards[4].Face;
        }

        private bool IsFullHouse(List<Card> cards)
        {
            // Check if three cards have the same face and the remaining two cards have the same face
            return cards[0].Face == cards[1].Face && cards[1].Face == cards[2].Face && cards[3].Face == cards[4].Face ||
                   cards[0].Face == cards[1].Face && cards[2].Face == cards[3].Face && cards[3].Face == cards[4].Face;
        }

        private bool IsFlush(List<Card> cards)
        {
            // Check if all cards have the same suit
            return cards.All(c => c.Suit == cards[0].Suit);
        }

        private bool IsStraight(List<Card> cards)
        {
            // Check if all cards are in sequence
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].Face - cards[i + 1].Face != 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsThreeOfAKind(List<Card> cards)
        {
            // Check if three cards have the same face
            return cards[0].Face == cards[1].Face && cards[1].Face == cards[2].Face ||
                   cards[1].Face == cards[2].Face && cards[2].Face == cards[3].Face ||
                   cards[2].Face == cards[3].Face && cards[3].Face == cards[4].Face;
        }

        private bool IsTwoPair(List<Card> cards)
        {
            // Check if there are two pairs of cards with the same face
            return cards[0].Face == cards[1].Face && cards[2].Face == cards[3].Face ||
                   cards[0].Face == cards[1].Face && cards[3].Face == cards[4].Face ||
                   cards[1].Face == cards[2].Face && cards[3].Face == cards[4].Face;
        }

        private bool IsPair(List<Card> cards)
        {
            // Check if two cards have the same face
            return cards[0].Face == cards[1].Face ||
                   cards[1].Face == cards[2].Face ||
                   cards[2].Face == cards[3].Face ||
                   cards[3].Face == cards[4].Face ||
                   cards[4].Face == cards[5].Face ||
                   cards[5].Face == cards[6].Face;
        }
    }

}
