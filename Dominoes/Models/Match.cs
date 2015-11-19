using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class Match
    {
        public int MatchID { get; set; }
        public virtual byte Score { get; set; }
        public virtual byte TeamWinner { get; set; }
        public string Notes { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You need to add a Game first")]
        public int GameID { get; set; }

        public Game Game { get; set; }
    }
}