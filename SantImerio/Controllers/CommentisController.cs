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
    public class CommentisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Commentis
        public ActionResult Index()
        {
            return View(db.Commentis.ToList().OrderByDescending(c=>c.Data));
        }

        // GET: Commentis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commenti commenti = db.Commentis.Find(id);
            if (commenti == null)
            {
                return HttpNotFound();
            }
            return View(commenti);
        }

        // GET: Commentis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Commentis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Commento_Id,Data,Commento,UId,Evento_Id,Utente")] Commenti commenti)
        {
            if (ModelState.IsValid)
            {
                var uid = User.Identity.GetUserId();
                commenti.Utente = User.Identity.Name;
                commenti.Data = DateTime.Now;
                commenti.UId = uid;
                commenti.Evento_Id = Convert.ToInt32(Request.QueryString["EId"]);
                db.Commentis.Add(commenti);
                db.SaveChanges();
                return RedirectToAction("Evento", "Eventis", new {id = Request.QueryString["EId"] });
            }

            return View(commenti);
        }

        // GET: Commentis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commenti commenti = db.Commentis.Find(id);
            if (commenti == null)
            {
                return HttpNotFound();
            }
            return View(commenti);
        }

        // POST: Commentis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Commento_Id,Data,Commento,UId,Evento_Id")] Commenti commenti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commenti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Evento", "Eventis", new { id = Request.QueryString["EId"] });
            }
            return View(commenti);
        }

        // GET: Commentis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commenti commenti = db.Commentis.Find(id);
            if (commenti == null)
            {
                return HttpNotFound();
            }
            return View(commenti);
        }

        // POST: Commentis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commenti commenti = db.Commentis.Find(id);
            db.Commentis.Remove(commenti);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteUt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commenti commenti = db.Commentis.Find(id);
            if (commenti == null)
            {
                return HttpNotFound();
            }
            return View(commenti);

        }

        [HttpPost, ActionName("DeleteUt")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUtConfirmed(int id)
        {
            Commenti commenti = db.Commentis.Find(id);
            db.Commentis.Remove(commenti);
            db.SaveChanges();
            return RedirectToAction("Evento", "Eventis", new {id = Request.QueryString["EId"] });
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
