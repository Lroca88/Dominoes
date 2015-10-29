using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class GameHandler
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool calculateScore(int GameID)
        {

            var Game = db.Game.Find(GameID);

            int NumberMatchA = Game.Matches.Count(i => i.ScoreTeamA > 0);
            int NumberMatchB = Game.Matches.Count(i => i.ScoreTeamB > 0);

            int TotalA = Game.Matches.Sum(i => i.ScoreTeamA);
            int TotalB = Game.Matches.Sum(i => i.ScoreTeamB);

            int WinningScore = Game.WinningScore;

            if (TotalA >= WinningScore)
            {
                string Category = CalculatePoints(NumberMatchA, NumberMatchB);
                Game = populateGame(Game, 1, Category);
            }
            else if (TotalB >= WinningScore)
            {
                string Category = CalculatePoints(NumberMatchB, NumberMatchB);
                Game = populateGame(Game, 2, Category);
            }
            else
            {
                return false;
            }

            db.Entry(Game).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public string CalculatePoints(int a1, int a2)
        {
            if (a1 == 1 && a2 == 0)         // Viajero 
            {
                return "Viajero";
            }
            else if (a1 > 1 && a2 == 0)     // Pollona
            {
                return "Pollona";
            }
            else                            // GameWinner
            {
                return "GameWinner";
            }
        }

        public Game populateGame(Game Game, byte WinningTeam, string Category)
        {
            Game.GameComplete = true;
            Game.Date = DateTime.Now;
            Game.WinningTeam = WinningTeam;
            Game.WinningCategory = Category;
            return Game;
        }
    }
}