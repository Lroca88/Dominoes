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
    public class GameSeriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GameSeries
        public ActionResult Index()
        {

            var gameSerie = db.GameSerie.Include(g => g.UserProfileInfo);
            return View(gameSerie.ToList());
        }

        // GET: GameSeries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameSerie gameSerie = db.GameSerie.Find(id);
            if (gameSerie == null)
            {
                return HttpNotFound();
            }
            return View(gameSerie);
        }

        // GET: GameSeries/Create
        public ActionResult Create()
        {
            ViewBag.UserProfileInfoID = new SelectList(db.UserProfileInfo, "UserProfileInfoID", "FirstName");
            return View();
        }

        // POST: GameSeries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GameSerieID,Name,Notes,GameWinner,PollonaValue,ViajeroValue,UserProfileInfoID")] GameSerie gameSerie)
        {
            //gameSerie.UserProfileInfo = db.UserProfileInfo.First();  // No needed

            if (ModelState.IsValid)
            {
                db.GameSerie.Add(gameSerie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserProfileInfoID = new SelectList(db.UserProfileInfo, "UserProfileInfoID", "FirstName", gameSerie.UserProfileInfoID);
            return View(gameSerie);
        }

        // GET: GameSeries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameSerie gameSerie = db.GameSerie.Find(id);
            if (gameSerie == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserProfileInfoID = new SelectList(db.UserProfileInfo, "UserProfileInfoID", "FirstName", gameSerie.UserProfileInfoID);
            return View(gameSerie);
        }

        // POST: GameSeries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameSerieID,Name,Notes,GameWinner,PollonaValue,ViajeroValue,UserProfileInfoID")] GameSerie gameSerie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gameSerie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserProfileInfoID = new SelectList(db.UserProfileInfo, "UserProfileInfoID", "FirstName", gameSerie.UserProfileInfoID);
            return View(gameSerie);
        }

        // GET: GameSeries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameSerie gameSerie = db.GameSerie.Find(id);
            if (gameSerie == null)
            {
                return HttpNotFound();
            }
            return View(gameSerie);
        }

        // POST: GameSeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GameSerie gameSerie = db.GameSerie.Find(id);
            db.GameSerie.Remove(gameSerie);
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
