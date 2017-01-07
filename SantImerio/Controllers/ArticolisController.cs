using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SantImerio.Models;
using System.IO;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace SantImerio.Controllers
{
    public class ArticolisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articolis
        public ActionResult Index()
        {
            ViewBag.ArticoliCount = db.Articolis.Count();
            var articoli = db.Articolis.OrderByDescending(d => d.Data);
            return View(articoli);
        }
        public ActionResult IndexUt()
        {
            var articoli = db.Articolis.Where(a=>a.Pubblica == true).OrderByDescending(a=>a.Data).ToList();
            return View(articoli);
        }


        public ActionResult Articolo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Articoli/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Articoli articoli = db.Articolis.Find(id);
            if (articoli == null)
            {
                return HttpNotFound();
            }

            return View(articoli);

        }



        // GET: Articolis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articoli articoli = db.Articolis.Find(id);
            if (articoli == null)
            {
                return HttpNotFound();
            }
            return View(articoli);
        }

        // GET: Articolis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articolis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Collaboratore")]

        public ActionResult Create([Bind(Include = "Articolo_Id,Titolo,SottoTitolo,Data,Pubblica,Autore", Exclude ="Testo")] Articoli articoli)
        {
            FormCollection collection = new FormCollection(Request.Unvalidated().Form);
            articoli.Testo = collection["Testo"];
            if (ModelState.IsValid)
            {
                db.Articolis.Add(articoli);
                db.SaveChanges();
                Directory.CreateDirectory(Server.MapPath("~/Content/Immagini/Articoli/" + articoli.Articolo_Id));
                return RedirectToAction("Index");
            }

            return View(articoli);
        }

        // GET: Articolis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articoli articoli = db.Articolis.Find(id);
            if (articoli == null)
            {
                return HttpNotFound();
            }
            return View(articoli);
        }

        // POST: Articolis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult Edit([Bind(Include = "Articolo_Id,Titolo,SottoTitolo,Data,Pubblica,Autore", Exclude ="Testo")] Articoli articoli)
        {
            FormCollection collection = new FormCollection(Request.Unvalidated().Form);
            articoli.Testo = collection["Testo"];
            if (ModelState.IsValid)
            {
                db.Entry(articoli).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(articoli);
        }

        //Gestione immagine evento
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult EditImgP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Articoli/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Articoli articoli = db.Articolis.Find(id);
            if (articoli == null)
            {
                return HttpNotFound();
            }
            return View(articoli);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult EditImgP(HttpPostedFileBase file, int? id)
        {
            if (file != null)
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Immagini/Articoli/" + id + "/"), id + ".jpg");
                    WebImage img = new WebImage(file.InputStream);
                    var larghezza = img.Width;
                    var altezza = img.Height;
                    var rapportoO = larghezza / altezza;
                    var rapportoV = altezza / larghezza;
                    if (altezza > 1900 | larghezza > 1900)
                    {
                        if (rapportoO >= 1)
                        {
                            ViewBag.Message = "Attendi la fine del download...";
                            img.Resize(1900, 1900 / rapportoO);
                            img.Save(path);
                            ViewBag.Message = "Download immagine orizzontale avvenuto con successo. Dimensione immagine originale: larghezza " + larghezza + " Altezza " + altezza;
                        }
                        else
                        {
                            img.Resize(800 / rapportoV, 800);
                            img.Save(path);
                            ViewBag.Message = "Download immagine verticale avvenuto con successo. Dimensione immagine: larghezza " + larghezza + "Altezza" + altezza;
                        }
                    }
                    else
                    {
                        if (rapportoO >= 1)
                        {
                            ViewBag.Message = "Attendi la fine del download...";
                            img.Save(path);
                            ViewBag.Message = "Download immagine orizzontale avvenuto con successo. Dimensione immagine originale: larghezza " + larghezza + " Altezza " + altezza;
                        }
                        else
                        {
                            ViewBag.Message = "Attendi la fine del download...";
                            img.Save(path);
                            ViewBag.Message = "Download immagine verticale avvenuto con successo. Dimensione immagine: larghezza " + larghezza + "Altezza" + altezza;
                        }
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
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Articoli/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Articoli articoli = db.Articolis.Find(id);
            if (articoli == null)
            {
                return HttpNotFound();
            }
            return View(articoli);

        }


        // GET: Articolis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var files = Directory.EnumerateFiles(Server.MapPath("/Content/Immagini/Articoli/" + id));
            ViewBag.Files = files;
            ViewBag.FilesCount = files.Count();
            Articoli articoli = db.Articolis.Find(id);
            if (articoli == null)
            {
                return HttpNotFound();
            }
            return View(articoli);
        }

        // POST: Articolis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("/Content/Immagini/Articoli/" + id + "/"));
            foreach (string filePath in filePaths)
            {
                System.IO.File.Delete(filePath);
            }
            Directory.Delete(Server.MapPath("/Content/Immagini/Articoli/" + id));
            Articoli articoli = db.Articolis.Find(id);
            db.Articolis.Remove(articoli);
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
