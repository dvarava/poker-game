using poker_game;
using System.Data.Entity;
public class GameData : DbContext
{
    public GameData() : base("PokerGameData")
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .Property(g => g.GameName)
            .HasMaxLength(100);

        modelBuilder.Entity<Game>()
            .Property(g => g.GameImage)
            .HasMaxLength(255);

        modelBuilder.Entity<Game>()
        .HasMany(g => g.Players)
        .WithRequired(p => p.Game)
        .HasForeignKey(p => p.GameId)
        .WillCascadeOnDelete(false);
    }
}
