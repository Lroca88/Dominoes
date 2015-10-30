using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominoes.Models;

namespace Dominoes.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboard
        [Authorize]
        public ActionResult Index()
        {
            UserHandler UserHandler = new UserHandler();
            UserProfileInfo User = UserHandler.GetUserLogged();
            var group = UserHandler.GetGroupAdministered();
            var GameSeriesUser = UserHandler.GetGameSeriesScore(group.Users);
            var GroupUser = UserHandler.GetGroupsScore(group.Users);
            var MonthUser = UserHandler.GetMonthsScore(group.Users);
            List<DashboardUserView>[] ResultSet = new List<DashboardUserView>[] { GameSeriesUser, GroupUser, MonthUser };
            return View(ResultSet);
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
