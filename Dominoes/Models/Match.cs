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


        public int calculateScore( int GameID)
        {
            using (var db = new ApplicationDbContext())
            {
                var TeamAB_Score = db.Match
                                     .Where(i => i.GameID == GameID)
                                     .GroupBy(k => 1)
                                     .Select(j => new
                                     {
                                         TeamA = j.Sum(item => item.ScoreTeamA),
                                         TeamB = j.Sum(item => item.ScoreTeamB)
                                     }).First();

                int WinningScore = db.Game
                                     .Where(i => i.GameID == GameID)
                                     .Select(i => i.WinningScore)
                                     .Single();

                if (TeamAB_Score.TeamA >= WinningScore)
                {
                    return 1;
                }
                else if (TeamAB_Score.TeamB >= WinningScore)
                {
                    return 2;
                }
                
            }

            return 0;
        }

    }
}