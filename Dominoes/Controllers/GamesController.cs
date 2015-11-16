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
    public class GamesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Games
        public ActionResult Index()
        {
            UserHandler UserHandler = new UserHandler();
            var UserProfileInfoID = UserHandler.GetUserLogged().UserProfileInfoID;
            var game = db.Game.Include(g => g.GameSerie)
                              .Where( g => g.GameSerie.UserProfileInfoID == UserProfileInfoID);
            return View(game.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Game.Find(id);
            UserHandler UserHandler = new UserHandler();
            ViewBag.NamesInGame = UserHandler.GetNamesInGame(game);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            UserHandler userHandler = new UserHandler();
            UserProfileInfo user = userHandler.GetUserLogged();
            ViewBag.GameSerieID = new SelectList(user.GameSeries, "GameSerieID", "Name");
            ViewBag.Players = new SelectList(user.Groups
                                                 .Where(i => i.DominoesGroupID == user.GroupAdministered)
                                                 .Select(i => i.Users).First(),
                                             "UserProfileInfoID",
                                             "FirstName");

            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GameID,Notes,WinningScore,GameComplete,WinningTeam,Date,Player1,Player2,Player3,Player4,GameSerieID")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Game.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            UserHandler userHandler = new UserHandler();
            UserProfileInfo user = userHandler.GetUserLogged();
            ViewBag.GameSerieID = new SelectList(user.GameSeries, "GameSerieID", "Name");
            ViewBag.Players = new SelectList(user.Groups
                                                 .Where(i => i.DominoesGroupID == user.GroupAdministered)
                                                 .Select(i => i.Users).First(),
                                             "UserProfileInfoID",
                                             "FirstName");
            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserHandler userHandler = new UserHandler();
            UserProfileInfo user = userHandler.GetUserLogged();
            Game game = db.Game.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameSerieID = new SelectList(user.GameSeries, "GameSerieID", "Name");
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameID,Notes,WinningScore,GameComplete,WinningTeam,Date,Player1,Player2,Player3,Player4,GameSerieID")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameSerieID = new SelectList(db.GameSerie, "GameSerieID", "Name", game.GameSerieID);
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Game.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Game.Find(id);
            db.Game.Remove(game);
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
