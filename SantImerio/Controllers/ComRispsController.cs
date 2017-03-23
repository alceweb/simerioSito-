using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using SantImerio.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Mail;

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
            ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
            ViewBag.NomeUtente = user.Nome + " " + user.Cognome;
            ViewBag.Commento_Id = new SelectList(db.Commentis, "Commento_Id", "Commento");
            return View();
        }

        // POST: ComRisps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int id, [Bind(Include = "ComRisp_Id,Data,Commento_Id,Risposta,UId,Utente,Email")] ComRisp comRisp)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                var uid = User.Identity.GetUserId();
                var eid = Convert.ToInt32(Request.QueryString["EId"]);
                var email = Request.QueryString["email"];
                string link = "http://www.santimerio.it/Eventis/Evento/" + eid;
                //valorizzo l'utente che sta rispondendo al commento
                string Utente = user.Nome + " " + user.Cognome;
                //valorizzo l'utente proprietario del commento
                string utente = Request.QueryString["utente"];
                comRisp.Utente = Utente;
                comRisp.Data = DateTime.Now;
                comRisp.UId = uid;
                comRisp.Email = User.Identity.Name;
                comRisp.Commento_Id = id;
                db.ComRisps.Add(comRisp);
                db.SaveChanges();
                // Invio la mail alert al proprietario del commento
                MailMessage message = new MailMessage(
                    "webservice@santimerio.it",
                    email,
                    "Nuovo commento dal sito santimerio.it",
                    "Il giorno <strong>" + DateTime.Now + "<br/>" + Utente + "</strong><br/> ha pubblicato una risposta ad un commento di " + utente + "<hr/><p>" + comRisp.Risposta + "</p><h3><a href=" + link + ">vai al sito</a></h3>");
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                }
                // Invio la mail alert ad ogni utente che ha risposto al commento
                var elenco = db.ComRisps.Where(r => r.Commento_Id == id && r.Email != email).ToList();
                foreach (var itemrisp in elenco)
                {
                    MailMessage message1 = new MailMessage(
                        "webservice@santimerio.it",
                        itemrisp.Email,
                        "Nuovo commento dal sito santimerio.it",
                        "Il giorno <strong>" + DateTime.Now + "<br/>" + Utente + "</strong><br/> ha pubblicato una risposta ad un commento di " + utente + "<hr/><p>" + comRisp.Risposta + "</p><h3><a href=" + link + ">vai al sito</a></h3>");
                    message1.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message1);
                    }
                }

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
