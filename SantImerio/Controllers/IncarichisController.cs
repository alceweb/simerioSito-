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
using System.IO;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;

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

        public async Task<ActionResult> IndexUt([Bind(Include = "Statistiche_Id,Data,Ip,Pagina,UId,UName")] Statistiche statistiche)
        {
            ViewBag.Title = "La nostra comunità";
            if (ModelState.IsValid)
            {
                statistiche.Data = DateTime.Now;
                statistiche.Ip = Request.UserHostAddress;
                statistiche.Pagina = ViewBag.Title;
                if (User.Identity.IsAuthenticated)
                {
                    ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                    statistiche.UName = user.Nome + user.Cognome;
                    statistiche.UId = User.Identity.GetUserId();
                }
                else
                {
                    statistiche.UName = "anonimous";
                }
                db.Statistiches.Add(statistiche);
                db.SaveChanges();
            }
            var incarichi = db.Incarichis.OrderBy(i=>i.Incarichi_Id).ToList();
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

        //Gestione immagine persona incarico
        public ActionResult EditImg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Incarichi/"));
            ViewBag.Immagini = immagini.ToList();
            Incarichi incarichi = db.Incarichis.Find(id);
            if (incarichi == null)
            {
                return HttpNotFound();
            }
            return View(incarichi);
        }

        [HttpPost]
        public ActionResult EditImg(HttpPostedFileBase file, int? id)
        {
            if (file != null)
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Immagini/Incarichi/"), id + ".jpg");
                    WebImage img = new WebImage(file.InputStream);
                    var larghezza = img.Width;
                    var altezza = img.Height;
                    var rapportoO = larghezza / altezza;
                    var rapportoV = altezza / larghezza;
                        if (rapportoO >= 1)
                        {
                            ViewBag.Message = "Attendi la fine del download...";
                            img.Resize(400, 400 / rapportoO);
                            img.Save(path);
                            ViewBag.Message = "Download immagine orizzontale avvenuto con successo. Dimensione immagine originale: larghezza " + larghezza + " Altezza " + altezza;
                        }
                        else
                        {
                            img.Resize(200 / rapportoV, 200);
                            img.Save(path);
                            ViewBag.Message = "Download immagine verticale avvenuto con successo. Dimensione immagine: larghezza " + larghezza + "Altezza" + altezza;
                        }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "Devi scegliere un file";
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Incarichi/"));
            ViewBag.Immagini = immagini.ToList();
            Incarichi incarichi = db.Incarichis.Find(id);
            if (incarichi == null)
            {
                return HttpNotFound();
            }
            return View(incarichi);

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
