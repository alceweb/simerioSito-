using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SantImerio.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace SantImerio.Controllers
{
    public class IncarichisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Incarichis
        public ActionResult Index()
        {
            return View(db.Incarichis.ToList());
        }

        public async Task<ActionResult> IndexUt()
        {
            var incarichi = db.Incarichis.ToList();
            var iscritti = await UserManager.Users.ToListAsync();
            ViewBag.Iscritti = iscritti;
            return View(incarichi);
        }

        // GET: Incarichis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incarichi incarichi = db.Incarichis.Find(id);
            if (incarichi == null)
            {
                return HttpNotFound();
            }
            return View(incarichi);
        }

        // GET: Incarichis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Incarichis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Incarichi_Id,Incarico,Nome,Cognome,Telefono,Email,Indirizzo,Città,Cap")] Incarichi incarichi)
        {
            if (ModelState.IsValid)
            {
                db.Incarichis.Add(incarichi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(incarichi);
        }

        // GET: Incarichis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incarichi incarichi = db.Incarichis.Find(id);
            if (incarichi == null)
            {
                return HttpNotFound();
            }
            return View(incarichi);
        }

        // POST: Incarichis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Incarichi_Id,Incarico,Nome,Cognome,Telefono,Email,Indirizzo,Città,Cap")] Incarichi incarichi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incarichi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexUt");
            }
            return View(incarichi);
        }

        // GET: Incarichis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incarichi incarichi = db.Incarichis.Find(id);
            if (incarichi == null)
            {
                return HttpNotFound();
            }
            return View(incarichi);
        }

        // POST: Incarichis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incarichi incarichi = db.Incarichis.Find(id);
            db.Incarichis.Remove(incarichi);
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
