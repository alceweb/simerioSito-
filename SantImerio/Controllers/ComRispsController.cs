using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SantImerio.Models;
using Microsoft.AspNet.Identity;

namespace SantImerio.Controllers
{
    [Authorize]
    public class ComRispsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ComRisps
        public ActionResult Index()
        {
            var comRisps = db.ComRisps.Include(c => c.Commento);
            return View(comRisps.ToList());
        }

        // GET: ComRisps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComRisp comRisp = db.ComRisps.Find(id);
            if (comRisp == null)
            {
                return HttpNotFound();
            }
            return View(comRisp);
        }

        // GET: ComRisps/Create
        public ActionResult Create()
        {
            ViewBag.Commento_Id = new SelectList(db.Commentis, "Commento_Id", "Commento");
            return View();
        }

        // POST: ComRisps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include = "ComRisp_Id,Data,Commento_Id,Risposta,UId,Utente")] ComRisp comRisp)
        {
            if (ModelState.IsValid)
            {
                var uid = User.Identity.GetUserId();
                comRisp.Utente = User.Identity.Name;
                comRisp.Data = DateTime.Now;
                comRisp.UId = uid;
                comRisp.Commento_Id = id;
                db.ComRisps.Add(comRisp);
                db.SaveChanges();
                return RedirectToAction("Evento", "Eventis", new { id = Request.QueryString["EId"] });
            }

            ViewBag.Commento_Id = new SelectList(db.Commentis, "Commento_Id", "Commento", comRisp.Commento_Id);
            return View(comRisp);
        }

        // GET: ComRisps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComRisp comRisp = db.ComRisps.Find(id);
            if (comRisp == null)
            {
                return HttpNotFound();
            }
            ViewBag.Commento_Id = new SelectList(db.Commentis, "Commento_Id", "Commento", comRisp.Commento_Id);
            return View(comRisp);
        }

        // POST: ComRisps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComRisp_Id,Data,Commento_Id,Risposta,UId,Utente")] ComRisp comRisp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comRisp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Evento", "Eventis", new { id = Request.QueryString["EId"] });
            }
            ViewBag.Commento_Id = new SelectList(db.Commentis, "Commento_Id", "Commento", comRisp.Commento_Id);
            return View(comRisp);
        }

        // GET: ComRisps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComRisp comRisp = db.ComRisps.Find(id);
            if (comRisp == null)
            {
                return HttpNotFound();
            }
            return View(comRisp);
        }

        // POST: ComRisps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComRisp comRisp = db.ComRisps.Find(id);
            db.ComRisps.Remove(comRisp);
            db.SaveChanges();
            return RedirectToAction("Evento", "Eventis", new { id = Request.QueryString["EId"] });
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
