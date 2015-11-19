using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dominoes.Models
{
    public class GameHandler
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool calculateScore(int GameID, byte Winner)
        {

            var Game = db.Game.Find(GameID);

            // Total amount of points eraned by TeamWinner
            int TotalPoints = Game.Matches.Where(i => i.TeamWinner == Winner).Sum(i => i.Score);
            
            // Score needed to win a game
            int WinningScore = Game.WinningScore;

            // Comparing the current total amount with the Score needed
            if (TotalPoints >= WinningScore)
            {
                // Including all teams of this particular game in a List
                List<int> TeamList = new List<int> {Winner};
                checkTeam(TeamList, Game.TeamPlayer1);
                checkTeam(TeamList, Game.TeamPlayer2);
                checkTeam(TeamList, Game.TeamPlayer3);
                checkTeam(TeamList, Game.TeamPlayer4);

                // Looking for amount of matches that every team has won 
                List<int> TeamMatches = new List<int>();
                foreach (var Team in TeamList)
                {
                    TeamMatches.Add(Game.Matches.Count(i => i.TeamWinner == Team));
                }

                // Defining winning category (Viajero, Pollona, GameWinner)
                string Category = CalculatePoints(TeamMatches);
                Game = populateGame(Game, Winner, Category);
            }
            else
            {
                return false;
            }

            db.Entry(Game).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public void checkTeam(List<int> TeamList, int TeamID)
        {
            if (!TeamList.Contains(TeamID) && TeamID != 0)
            {
                TeamList.Add(TeamID);
            }
        }

        public string CalculatePoints(List<int> TeamMatches)
        {
            var WinnerMatches = TeamMatches.First();
            TeamMatches.RemoveAt(0);

            if (WinnerMatches == 1 && !TeamMatches.Any(z => z > 0))         // Viajero 
            {
                return "Viajero";
            }
            else if (WinnerMatches > 1 && !TeamMatches.Any(z => z > 0))     // Pollona
            {
                return "Pollona";
            }
            else                                                            // GameWinner
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

        public IEnumerable<SelectListItem> SelectGameSeries(UserProfileInfo user, Game game)
        {
            var GameSeries = user.GameSeries.ToList();
            IEnumerable<SelectListItem> selectList =
                from g in GameSeries
                select new SelectListItem
                {
                    Selected = (g.GameSerieID == game.GameSerieID),
                    Text = g.Name,
                    Value = g.GameSerieID.ToString()
                };

            return selectList;
        }


        public IEnumerable<SelectListItem> SelectGames(int UserProfileInfoID, int GameID)
        {
            var Games = db.Game.Where(i => (i.GameSerie.UserProfileInfoID == UserProfileInfoID) && (i.GameComplete == false));
            IEnumerable<SelectListItem> selectList =
                from g in Games
                select new SelectListItem
                {
                    Selected = (g.GameID == GameID),
                    Text = g.Notes,
                    Value = g.GameID.ToString()
                };
            return selectList;
        }
    }
}