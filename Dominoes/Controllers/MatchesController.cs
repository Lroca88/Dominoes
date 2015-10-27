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
    public class MatchesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Matches
        public ActionResult Index()
        {
            var match = db.Match.Include(m => m.Game);
            return View(match.ToList());
        }

        // GET: Matches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Match.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // GET: Matches/Create
        public ActionResult Create()
        {
            ViewBag.GameID = new SelectList(db.Game, "GameID", "Notes");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MatchID,ScoreTeamA,ScoreTeamB,Notes,GameID")] Match match)
        {
            if (ModelState.IsValid)
            {
                db.Match.Add(match);
                db.SaveChanges();

                int winner = match.calculateScore(match.GameID);

                return RedirectToAction("Index");
            }

            ViewBag.GameID = new SelectList(db.Game, "GameID", "Notes", match.GameID);
            return View(match);
        }

        // GET: Matches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Match.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameID = new SelectList(db.Game, "GameID", "Notes", match.GameID);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MatchID,ScoreTeamA,ScoreTeamB,Notes,GameID")] Match match)
        {
            if (ModelState.IsValid)
            {
                db.Entry(match).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameID = new SelectList(db.Game, "GameID", "Notes", match.GameID);
            return View(match);
        }

        // GET: Matches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Match.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Match match = db.Match.Find(id);
            db.Match.Remove(match);
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
