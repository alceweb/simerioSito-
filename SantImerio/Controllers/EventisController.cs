using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SantImerio.Models;

namespace SantImerio.Controllers
{
    public class EventisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Eventis
        public ActionResult Index()
        {
            ViewBag.EventiCount = db.Eventis.Count();
            var eventi = db.Eventis.OrderByDescending(d => d.Data);
            return View(eventi);
        }

        // GET: Eventis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        // GET: Eventis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Eventis/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Evento_Id,Titolo,Descrizione,Data,DataI,DataF,Pubblica")] Eventi eventi)
        {
            if (ModelState.IsValid)
            {
                db.Eventis.Add(eventi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventi);
        }

        // GET: Eventis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        // POST: Eventis/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Evento_Id,Titolo,Descrizione,Data,DataI,DataF,Pubblica")] Eventi eventi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventi);
        }

        // GET: Eventis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        // POST: Eventis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Eventi eventi = db.Eventis.Find(id);
            db.Eventis.Remove(eventi);
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
