using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class Game
    {
        public Game()
        {
            this.Matches = new HashSet<Match>();
        }

        public int GameID { get; set; }
        public virtual string Notes { get; set; }
        public virtual byte WinningScore { get; set; }
        public virtual bool GameComplete { get; set; }
        public virtual byte WinningTeam { get; set; }
        public virtual string WinningCategory { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Player1 { get; set; }
        public virtual int Player2 { get; set; }
        public virtual int Player3 { get; set; }
        public virtual int Player4 { get; set; }
        public int GameSerieID { get; set; }

        public virtual GameSerie GameSerie { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
    }
}