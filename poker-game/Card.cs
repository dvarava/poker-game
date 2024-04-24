using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_game
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Face
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public class Card
    {
        public int CardId { get; set; }
        public Suit Suit { get; set; }
        public Face Face { get; set; }
        public string UnicodeImage { get; set; }

        public int PlayerId { get; set; } // Foreign key
        public Player Player { get; set; } // Navigation property


        // Unicode images for each card
        private Dictionary<string, string> unicodeImages = new Dictionary<string, string>()
        {
            {"AceSpades", "\uD83C\uDCA1" },
            {"TwoSpades", "\uD83C\uDCA2" },
            {"ThreeSpades", "\uD83C\uDCA3" },
            {"FourSpades", "\uD83C\uDCA4" },
            {"FiveSpades", "\uD83C\uDCA5" },
            {"SixSpades", "\uD83C\uDCA6" },
            {"SevenSpades", "\uD83C\uDCA7" },
            {"EightSpades", "\uD83C\uDCA8" },
            {"NineSpades", "\uD83C\uDCA9" },
            {"TenSpades", "\uD83C\uDCAA" },
            {"JackSpades", "\uD83C\uDCAB" },
            {"QueenSpades", "\uD83C\uDCAD" },
            {"KingSpades", "\uD83C\uDCAE" },

            {"AceHearts", "\ud83c\uDCB1" },
            {"TwoHearts", "\ud83c\uDCB2" },
            {"ThreeHearts", "\uD83C\uDCB3" },
            {"FourHearts", "\uD83C\uDCB4" },
            {"FiveHearts", "\uD83C\uDCB5" },
            {"SixHearts", "\uD83C\uDCB6" },
            {"SevenHearts", "\uD83C\uDCB7" },
            {"EightHearts", "\uD83C\uDCB8" },
            {"NineHearts", "\uD83C\uDCB9" },
            {"TenHearts", "\uD83C\uDCBA" },
            {"JackHearts", "\uD83C\uDCBB" },
            {"QueenHearts", "\uD83C\uDCBD" },
            {"KingHearts", "\ud83c\uDCBE" },

            {"AceDiamonds", "\uD83C\uDCC1" },
            {"TwoDiamonds", "\uD83C\uDCC2" },
            {"ThreeDiamonds", "\uD83C\uDCC3" },
            {"FourDiamonds", "\uD83C\uDCC4" },
            {"FiveDiamonds", "\ud83c\uDCC5" },
            {"SixDiamonds", "\ud83c\uDCC6" },
            {"SevenDiamonds", "\uD83C\uDCC7" },
            {"EightDiamonds", "\uD83C\uDCC8" },
            {"NineDiamonds", "\uD83C\uDCC9" },
            {"TenDiamonds", "\uD83C\uDCCA" },
            {"JackDiamonds", "\uD83C\uDCCB" },
            {"QueenDiamonds", "\ud83c\uDCCD" },
            {"KingDiamonds", "\ud83c\uDCCE" },

            {"AceClubs", "\ud83c\uDCD1" },
            {"TwoClubs", "\ud83c\uDCD2" },
            {"ThreeClubs", "\ud83c\uDCD3" },
            {"FourClubs", "\ud83c\uDCD4" },
            {"FiveClubs", "\uD83C\uDCD5" },
            {"SixClubs", "\uD83C\uDCD6" },
            {"SevenClubs", "\uD83C\uDCD7" },
            {"Eightclubs", "\ud83c\uDCD8" },
            {"NineClubs", "\uD83C\uDCD9" },
            {"TenClubs", "\ud83c\uDCDA" },
            {"JackClubs", "\ud83c\uDCDB" },
            {"QueenClubs", "\ud83c\uDCDD" },
            {"KingClubs", "\uD83C\uDCDE" }
        };

        public Card() { }

        public Card(Suit suit, Face face)
        {
            Suit = suit;
            Face = face;
            UnicodeImage = GetUnicodeImage();
        }

        public override string ToString()
        {
            return $"Face {Face} of Suit {Suit}";
        }

        private string GetUnicodeImage()
        {
            string key = Face.ToString() + Suit.ToString();
            if (unicodeImages.ContainsKey(key))
            {
                return unicodeImages[key];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
