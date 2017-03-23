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
        public async  Task<ActionResult> Create([Bind(Include = "Commento_Id,Data,Commento,UId,Evento_Id,Utente,Email")] Commenti commenti)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                var uid = User.Identity.GetUserId();
                var email = User.Identity.GetUserName();
                commenti.Utente = user.Nome + " " + user.Cognome;
                commenti.Data = DateTime.Now;
                commenti.UId = uid;
                commenti.Email = email;
                var eid = Convert.ToInt32(Request.QueryString["EId"]);
                string link = "http://www.santimerio.it/Eventis/Evento/" + eid;
                commenti.Evento_Id = eid;
                db.Commentis.Add(commenti);
                db.SaveChanges();
                // Invio la mail per avvisare che si è stato scritto un commento
                MailMessage message = new MailMessage(
                    "webservice@santimerio.it",
                    "cesare@cr-consult.eu,mik.rock@hotmail.it", 
                    "Nuovo commento dal sito santimerio.it",
                    "Il giorno <strong>" + DateTime.Now + "<br/>" + user.Nome + " " + user.Cognome + "</strong><br/> ha pubblicato un commento ad un evento<hr/><p>" + commenti.Commento + "</p><h3><a href=" + link + ">vai al sito</a></h3>");
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                }

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

    internal class MailAddress
    {
        private string v;

        public MailAddress(string v)
        {
            this.v = v;
        }
    }
}
