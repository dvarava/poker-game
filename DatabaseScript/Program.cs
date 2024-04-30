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
                game.GameName = "Game 1";
                game.GameImage = "../Images/GameImages/game1.jpg";

                var p1 = new Player("Player 1", 1000);
                var p2 = new Player("Player 2", 1000);
                var p3 = new Player("Player 3", 1000);
                var p4 = new Player("Player 4", 1000);
                var p5 = new Player("Player 5", 1000);

                p1.Game = game;
                p2.Game = game;
                p3.Game = game;
                p4.Game = game;
                p5.Game = game;

                // Add the objects to the DbContext
                context.Games.Add(game);

                context.Players.Add(p1);
                context.Players.Add(p2);
                context.Players.Add(p3);
                context.Players.Add(p4);
                context.Players.Add(p5);

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