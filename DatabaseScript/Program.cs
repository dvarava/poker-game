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
                var game1 = new Game();
                game1.GameName = "Game 1";
                game1.GameImage = "../Images/GameImages/game1.jpg";

                var game2 = new Game();
                game2.GameName = "Game 2";
                game2.GameImage = "../Images/GameImages/game2.jpg";

                var p1 = new Player("Player 1", 1000);
                var p2 = new Player("Player 2", 1400);
                var p3 = new Player("Player 3", 1000);
                var p4 = new Player("Player 4", 2200);

                var p5 = new Player("Player 5", 1000);
                var p6 = new Player("Player 6", 1200);
                var p7 = new Player("Player 7", 2000);
                var p8 = new Player("Player 8", 1600);

                p1.Game = game1;
                p2.Game = game1;
                p3.Game = game1;
                p4.Game = game1;

                p5.Game = game2;
                p6.Game = game2;
                p7.Game = game2;
                p8.Game = game2;

                // Add the objects to the DbContext
                context.Games.Add(game1);
                context.Games.Add(game2);

                context.Players.Add(p1);
                context.Players.Add(p2);
                context.Players.Add(p3);
                context.Players.Add(p4);

                context.Players.Add(p5);
                context.Players.Add(p6);
                context.Players.Add(p7);
                context.Players.Add(p8);

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