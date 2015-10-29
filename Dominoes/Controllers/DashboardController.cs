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
        public ActionResult Index()
        {
            UserHandler UserHandler = new UserHandler();
            UserProfileInfo User = UserHandler.GetUserLogged();
            var group = UserHandler.GetGroupAdministered();
            //var GameSeriesUser = UserHandler.GetGameSeriesScore(group.Users);
            var GroupUser = UserHandler.GetGroupsScore(group.Users);
            return View(db.UserProfileInfo.ToList());
        }

        // GET: Dashboard/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfileInfo userProfileInfo = db.UserProfileInfo.Find(id);
            if (userProfileInfo == null)
            {
                return HttpNotFound();
            }
            return View(userProfileInfo);
        }

        // GET: Dashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserProfileInfoID,FirstName,Email,GroupAdministered")] UserProfileInfo userProfileInfo)
        {
            if (ModelState.IsValid)
            {
                db.UserProfileInfo.Add(userProfileInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userProfileInfo);
        }

        // GET: Dashboard/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfileInfo userProfileInfo = db.UserProfileInfo.Find(id);
            if (userProfileInfo == null)
            {
                return HttpNotFound();
            }
            return View(userProfileInfo);
        }

        // POST: Dashboard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserProfileInfoID,FirstName,Email,GroupAdministered")] UserProfileInfo userProfileInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProfileInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userProfileInfo);
        }

        // GET: Dashboard/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfileInfo userProfileInfo = db.UserProfileInfo.Find(id);
            if (userProfileInfo == null)
            {
                return HttpNotFound();
            }
            return View(userProfileInfo);
        }

        // POST: Dashboard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfileInfo userProfileInfo = db.UserProfileInfo.Find(id);
            db.UserProfileInfo.Remove(userProfileInfo);
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
