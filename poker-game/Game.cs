using poker_game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Game
{
    public int GameId { get; set; }

    public ICollection<Player> Players { get; set; }
}
