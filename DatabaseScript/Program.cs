using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using poker_game;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new GameData())
        {
            try
            {
                // Create new objects
                var game = new Game();
                var player = new Player("Player 1", 1000);
                player.Game = game;
                var card = new Card { Suit = Suit.Hearts, Face = Face.Ace };
                card.Player = player;

                // Add the objects to the DbContext
                context.Games.Add(game);
                context.Players.Add(player);

                // Save the changes to the database
                context.SaveChanges();

                Console.WriteLine("Objects added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}