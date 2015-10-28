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
    }
}