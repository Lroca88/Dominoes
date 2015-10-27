using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class Match
    {
        public int MatchID { get; set; }
        public virtual byte ScoreTeamA { get; set; }
        public virtual byte ScoreTeamB { get; set; }
        public string Notes { get; set; }
        public int GameID { get; set; }

        public Game Game { get; set; }
    }
}