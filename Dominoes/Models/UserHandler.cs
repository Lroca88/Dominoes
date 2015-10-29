using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
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
                                         i => (i.WinningTeam == 1 && (i.Player1 == Id || i.Player2 == Id))
                                         || (i.WinningTeam == 2 && (i.Player3 == Id || i.Player4 == Id))                                
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
                var key = GameSerieData.First().GameSerieID;
                var total = 0;
                string SerieName = GameSerieData.First().GameSerieName;

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

                GameSeriesUser.ID.Add(key);
                GameSeriesUser.Name.Add(SerieName);
                GameSeriesUser.TotalPoints.Add(total);

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
                                         i => (i.WinningTeam == 1 && (i.Player1 == Id || i.Player2 == Id))
                                         || (i.WinningTeam == 2 && (i.Player3 == Id || i.Player4 == Id))
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
                var key = GroupData.First().GameSerieID;
                var total = 0;
                var GroupID = GroupData.First().GroupID;

                foreach (var GData in GroupData)
                {
                    if (GroupID == GData.GroupID)
                    {
                        total += GetCategoryValue(GData.WinningCategory, key);
                    }
                    else
                    {

                        GroupUser.ID.Add(GroupID);
                        GroupUser.TotalPoints.Add(total);

                        key = GData.GameSerieID;
                        total = GetCategoryValue(GData.WinningCategory, key);
                        GroupID = GData.GroupID;
                    }
                }

                GroupUser.ID.Add(GroupID);
                GroupUser.TotalPoints.Add(total);

                list.Add(GroupUser);
            }
            return list;
        }


    }
}