using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;



namespace Dominoes.Models
{
    public class UserHandler
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public UserProfileInfo GetUserLogged()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentuser = manager.FindById(HttpContext.Current.User.Identity.GetUserId());
            return currentuser.UserProfileInfo;
        }

        public DominoesGroup GetGroupAdministered()
        {
            var currentuser = GetUserLogged();
            var group = db.DominoesGroup.Where(i => i.DominoesGroupID == currentuser.GroupAdministered).Single();
            return group;
        }

        public List<string> GetNamesInGame(Game game)
        {
            List<string> Names = new List<string>();
            if (game.Player1 > 0) 
            {
                Names.Add(db.UserProfileInfo.Where(i => i.UserProfileInfoID == game.Player1).Select(i => i.FirstName).Single());
            }

            if (game.Player2 > 0)
            {
                Names.Add(db.UserProfileInfo.Where(i => i.UserProfileInfoID == game.Player2).Select(i => i.FirstName).Single());
            }

            if (game.Player3 > 0)
            {
                Names.Add(db.UserProfileInfo.Where(i => i.UserProfileInfoID == game.Player3).Select(i => i.FirstName).Single());
            }

            if (game.Player4 > 0)
            {
                Names.Add(db.UserProfileInfo.Where(i => i.UserProfileInfoID == game.Player4).Select(i => i.FirstName).Single());
            }

            return Names;
        }

        public int GetCategoryValue( string category, int Id)
        {
            switch (category)
            {
                case "Viajero":
                    return db.GameSerie.Where(i => i.GameSerieID == Id).Select(i => i.ViajeroValue).Single();
                case "Pollona":
                    return db.GameSerie.Where(i => i.GameSerieID == Id).Select(i => i.PollonaValue).Single();
                default:
                    return db.GameSerie.Where(i => i.GameSerieID == Id).Select(i => i.GameWinner).Single();
            }
        }

        public List<DashboardUserView> GetGameSeriesScore(ICollection<UserProfileInfo> users)
        {

            List<DashboardUserView> list = new List<DashboardUserView>();

            foreach(var user in users)
            {
                var Id = user.UserProfileInfoID;
            
                var GameSerieData = db.Game
                                      .Where
                                      (
                                         i => (i.GameComplete == true)
                                           && (i.WinningTeam == i.TeamPlayer1 && i.Player1 == Id)
                                           || (i.WinningTeam == i.TeamPlayer2 && i.Player2 == Id)
                                           || (i.WinningTeam == i.TeamPlayer3 && i.Player3 == Id)
                                           || (i.WinningTeam == i.TeamPlayer4 && i.Player4 == Id)
                                      )
                                      .OrderBy( i => i.GameSerieID)
                                      .Select
                                      (n => new 
                                         {
                                             GameSerieID = n.GameSerieID,
                                             WinningCategory = n.WinningCategory,
                                             GameSerieName = n.GameSerie.Name
                                         }
                                      );

                DashboardUserView GameSeriesUser = new DashboardUserView();
                GameSeriesUser.UserName = user.FirstName;
                GameSeriesUser.UserProfileInfoID = Id;
                var key = 0;
                var total = 0;
                string SerieName = "";
                
                if (GameSerieData.Any()) 
                {
                    key = GameSerieData.First().GameSerieID;
                    SerieName = GameSerieData.First().GameSerieName;
                }  

                foreach(var GameData in GameSerieData)
                {
                    if (key == GameData.GameSerieID)
                    {
                        total += GetCategoryValue(GameData.WinningCategory, key);
                    }
                    else
                    {

                        GameSeriesUser.ID.Add(key);
                        GameSeriesUser.Name.Add(SerieName);
                        GameSeriesUser.TotalPoints.Add(total);

                        key = GameData.GameSerieID;
                        total = GetCategoryValue(GameData.WinningCategory, key);
                        SerieName = GameData.GameSerieName;
                    }
                }

                if (GameSerieData.Any())
                {
                    GameSeriesUser.ID.Add(key);
                    GameSeriesUser.Name.Add(SerieName);
                    GameSeriesUser.TotalPoints.Add(total);
                }
                list.Add(GameSeriesUser);
            }  
            return list;
        }

        public List<DashboardUserView> GetGroupsScore(ICollection<UserProfileInfo> users)
        {
            List<DashboardUserView> list = new List<DashboardUserView>();
            foreach (var user in users)
            {
                var Id = user.UserProfileInfoID;
                var GroupData = db.Game
                                      .Where
                                      (
                                         i => (i.GameComplete == true)
                                           && (i.WinningTeam == i.TeamPlayer1 && i.Player1 == Id)
                                           || (i.WinningTeam == i.TeamPlayer2 && i.Player2 == Id)
                                           || (i.WinningTeam == i.TeamPlayer3 && i.Player3 == Id)
                                           || (i.WinningTeam == i.TeamPlayer4 && i.Player4 == Id)
                                      )
                                      .OrderBy(i => i.GameSerie.UserProfileInfo.GroupAdministered)
                                      .Select
                                      (n => new
                                      {
                                          GroupID = n.GameSerie.UserProfileInfo.GroupAdministered,
                                          GameSerieID = n.GameSerieID,
                                          WinningCategory = n.WinningCategory
                                      });

                DashboardUserView GroupUser = new DashboardUserView();
                GroupUser.UserName = user.FirstName;
                GroupUser.UserProfileInfoID = Id;
                var total = 0;
                var key = 0;
                var GroupID = 0;


                if (GroupData.Any())
                {
                    key = GroupData.First().GameSerieID;
                    GroupID = GroupData.First().GroupID;
                }
                

                foreach (var GData in GroupData)
                {
                    if (GroupID == GData.GroupID)
                    {
                        total += GetCategoryValue(GData.WinningCategory, key);
                        key = GData.GameSerieID;
                    }
                    else
                    {

                        GroupUser.ID.Add(GroupID);
                        GroupUser.TotalPoints.Add(total);
                        GroupUser.Name.Add(db.DominoesGroup
                                              .Where(i => i.DominoesGroupID == GroupID)
                                              .Select(i => i.Name).Single());
                        key = GData.GameSerieID;
                        total = GetCategoryValue(GData.WinningCategory, key);
                        GroupID = GData.GroupID;
                    }
                }

                if (GroupData.Any())
                {
                    GroupUser.ID.Add(GroupID);
                    GroupUser.TotalPoints.Add(total);
                    GroupUser.Name.Add(db.DominoesGroup
                                                  .Where(i => i.DominoesGroupID == GroupID)
                                                  .Select(i => i.Name).Single());
                }

                list.Add(GroupUser);
            }
            return list;
        }


        public List<DashboardUserView> GetMonthsScore(ICollection<UserProfileInfo> users)
        {
            List<DashboardUserView> list = new List<DashboardUserView>();
            foreach (var user in users)
            {
                var Id = user.UserProfileInfoID;
                var MonthData = db.Game
                                     .Where
                                     (
                                        i => (i.GameComplete == true)
                                           && (i.WinningTeam == i.TeamPlayer1 && i.Player1 == Id)
                                           || (i.WinningTeam == i.TeamPlayer2 && i.Player2 == Id)
                                           || (i.WinningTeam == i.TeamPlayer3 && i.Player3 == Id)
                                           || (i.WinningTeam == i.TeamPlayer4 && i.Player4 == Id)
                                     )
                                     .OrderBy(i => i.GameSerie.GameSerieID)
                                     .Select
                                     (n => new
                                     {
                                         Month = n.Date.Value.Month,
                                         GameSerieID = n.GameSerieID,
                                         WinningCategory = n.WinningCategory
                                     });

                DashboardUserView MonthUser = new DashboardUserView();
                MonthUser.UserName = user.FirstName;
                MonthUser.UserProfileInfoID = Id;
                var key = 0;
                int[] Month = new int[12];

                foreach (var MData in MonthData)
                {
                    key = MData.GameSerieID;
                    Month[MData.Month - 1] += GetCategoryValue(MData.WinningCategory, key);
                }

                MonthUser.TotalPoints = Month.ToList();
                MonthUser.Name = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();
                MonthUser.ID = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                list.Add(MonthUser);
            }

            return list;
        }


    }
}