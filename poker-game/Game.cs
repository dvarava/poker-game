using poker_game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Game
{
    public int GameId { get; set; }
    public string GameName { get; set; }
    public string GameImage { get; set; }
    public virtual ICollection<Player> Players { get; set; }
}