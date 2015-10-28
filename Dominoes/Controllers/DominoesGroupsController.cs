using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominoes.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dominoes.Controllers
{
    public class DominoesGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DominoesGroups
        public ActionResult Index()
        {
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //var currentuser = manager.FindById(User.Identity.GetUserId());
            // Get the users list for Administered Group
            //var GroupUsersList = db.DominoesGroup                  
            //                       .Include(i => i.Users)
            //                       .Where(i => i.DominoesGroupID == currentuser.UserProfileInfo.GroupAdministered)
            //                       .Single();

            UserHandler UserHandler = new UserHandler();
            return View(new[] { UserHandler.GetGroupAdministered() });
        }

        // GET: DominoesGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DominoesGroup dominoesGroup = db.DominoesGroup.Find(id);
            if (dominoesGroup == null)
            {
                return HttpNotFound();
            }
            return View(dominoesGroup);
        }

        // GET: DominoesGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DominoesGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DominoesGroupID,Name,Admin")] DominoesGroup dominoesGroup)
        {
            if (ModelState.IsValid)
            {
                db.DominoesGroup.Add(dominoesGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dominoesGroup);
        }

        // GET: DominoesGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DominoesGroup dominoesGroup = db.DominoesGroup.Find(id);
            var model = new UserViewModel();
            var AllUsers = db.UserProfileInfo.Select( 
                u => new
                {
                    UserID = u.UserProfileInfoID,
                    Name = u.FirstName
                }).ToList();

            model.DominoesGroupID = id.Value;
            model.UsersSelect = new MultiSelectList (AllUsers, "UserID", "Name");
            model.UserIDs = dominoesGroup.Users.Select(u => u.UserProfileInfoID).ToArray();

            if (dominoesGroup == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: DominoesGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel UserViewModel)
        {
            var dominoesGroup = db.DominoesGroup
                          .Include(i => i.Users)
                          .Where(i => i.DominoesGroupID == UserViewModel.DominoesGroupID)
                          .Single();

            if (ModelState.IsValid)
            {
                db.Entry(dominoesGroup).State = EntityState.Modified;
                UpdateGroupUsers(UserViewModel.UserIDs, dominoesGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(UserViewModel.UserIDs);
        }

        private void UpdateGroupUsers(int[] SelectedUsers, DominoesGroup dominoesGroup)
        {
            if (SelectedUsers == null) 
            {
                dominoesGroup.Users = new List<UserProfileInfo>();
            }

            var SelectedUsersHS = new HashSet<int>(SelectedUsers);
            var GroupUsersHS = new HashSet<int>(dominoesGroup.Users.Select(i => i.UserProfileInfoID));

            foreach(var user in db.UserProfileInfo)
            {
                if (SelectedUsersHS.Contains(user.UserProfileInfoID))
                {
                    if (!GroupUsersHS.Contains(user.UserProfileInfoID))
                    {
                        dominoesGroup.Users.Add(user);
                    }
                }
                else
                {
                    if (GroupUsersHS.Contains(user.UserProfileInfoID))
                    {
                        dominoesGroup.Users.Remove(user);
                    }
                }
            }
        }

        // GET: DominoesGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DominoesGroup dominoesGroup = db.DominoesGroup.Find(id);
            if (dominoesGroup == null)
            {
                return HttpNotFound();
            }
            return View(dominoesGroup);
        }

        // POST: DominoesGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DominoesGroup dominoesGroup = db.DominoesGroup.Find(id);
            db.DominoesGroup.Remove(dominoesGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
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
