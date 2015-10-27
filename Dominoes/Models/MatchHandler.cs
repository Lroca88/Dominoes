using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class MatchHandler
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int calculateScore(int GameID)
        {

            var Game = db.Game.Find(GameID);

            int cantA = Game.Matches.Count(i => i.ScoreTeamA > 0);
            int cantB = Game.Matches.Count(i => i.ScoreTeamB > 0);

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

            return 0;
        }
    }
}