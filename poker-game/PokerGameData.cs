using poker_game;
using System.Data.Entity;
public class GameData : DbContext
{
    public GameData() : base("PokerGameData")
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
}
