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
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SantImerio.Controllers
{
    public class EventisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Eventis
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult Index()
        {
            ViewBag.EventiCount = db.Eventis.Count();
            var eventi = db.Eventis.OrderByDescending(d => d.Data);
            return View(eventi);
        }
        public ActionResult IndexUt([Bind(Include = "Statistiche_Id,Data,Ip,Pagina,UId,UName")] Statistiche statistiche)
        {
            if (ModelState.IsValid)
            {
                statistiche.Data = DateTime.Now;
                statistiche.Ip = Request.UserHostAddress;
                statistiche.Pagina = "Eventi";
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
            return View(db.Eventis.Where(g=>g.Galleria == true).OrderByDescending(g=>g.Data).ToList());
        }

        public ActionResult IndexComm()
        {
            var eventi = db.Eventis.OrderByDescending(d => d.Data).ToList();
            return View(eventi);
        }
        public ActionResult Evento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Eventi/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Eventi eventi = db.Eventis.Find(id);
            var commenti = db.Commentis.Where(e => e.Evento_Id == id);
            ViewBag.Commenti = commenti;
            ViewBag.CommentiCount = commenti.Count();
            if (eventi == null)
            {
                return HttpNotFound();
            }

            return View(eventi);

        }
        // GET: Eventis/Details/5
        [Authorize(Roles = "Admin,Collaboratore")]
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
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Eventis/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult Create([Bind(Include = "Evento_Id,Titolo,Data,DataI,DataF,Pubblica,Galleria,Home,SottoTitolo,Volantino,Pastorale", Exclude = "Descrizione")] Eventi eventi)
        {
            FormCollection collection = new FormCollection(Request.Unvalidated().Form);
            eventi.Descrizione = collection["Descrizione"];
            if (ModelState.IsValid)
            {
                db.Eventis.Add(eventi);
                db.SaveChanges();
                Directory.CreateDirectory(Server.MapPath("~/Content/Immagini/Eventi/" + eventi.Evento_Id));
                return RedirectToAction("Index");
            }

            return View(eventi);
        }

        // GET: Eventis/Edit/5
        [Authorize(Roles = "Admin,Collaboratore")]
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
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult Edit([Bind(Include = "Evento_Id,Titolo,Data,DataI,DataF,Pubblica,Galleria,Home,SottoTitolo,Volantino,Pastorale", Exclude ="Descrizione")] Eventi eventi)
        {
            FormCollection collection = new FormCollection(Request.Unvalidated().Form);
            eventi.Descrizione = collection["Descrizione"];
            if (ModelState.IsValid)
            {
                db.Entry(eventi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventi);
        }

        //Gestione immagine evento
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult EditImgP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Eventi/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult EditImgP(HttpPostedFileBase file, int? id)
        {
            if (file != null)
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Immagini/Eventi/" + id + "/"), id + ".jpg");
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
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Eventi/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);

        }

        public ActionResult EditVol(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Eventi/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult EditVol(HttpPostedFileBase file, int? id)
        {
            if (file != null)
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Immagini/Eventi/" + id + "/"), id + "v.jpg");
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
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Eventi/" + id + "/"), "*v.jpg");
            ViewBag.Immagini = immagini.ToList();
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);

        }


        //Gestione immagini galleria fotografica
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult EditImg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Eventi/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult EditImg(IEnumerable<HttpPostedFileBase> files, int? id)
        {
            foreach (var file in files)
            {
                if (file != null)
                    try
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Immagini/Eventi/" + id + "/"), fileName);
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
                                ViewBag.Message = "Attendi la fine del download...";
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
            }
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/Eventi/" + id + "/"));
            ViewBag.Immagini = immagini.ToList();
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);

        }



        // GET: Eventis/Delete/5
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var files = Directory.EnumerateFiles(Server.MapPath("/Content/Immagini/Eventi/" + id));
            ViewBag.Files = files;
            ViewBag.FilesCount = files.Count();
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
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult DeleteConfirmed(int id)
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("/Content/Immagini/Eventi/" + id + "/"));
            foreach (string filePath in filePaths)
            {
                System.IO.File.Delete(filePath);
            } 
            Directory.Delete(Server.MapPath("/Content/Immagini/Eventi/" + id));
            Eventi eventi = db.Eventis.Find(id);
            db.Eventis.Remove(eventi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult DeleteImg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.File = Request.QueryString["file"];
            Eventi eventi = db.Eventis.Find(id);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        [HttpPost, ActionName("DeleteImg")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult DeleteImgConfirmed(int id)
        {
            var file = "~/Content/Immagini/Eventi/" + id + "/" + Request.QueryString["file"];
            System.IO.File.Delete(Server.MapPath(file));
            return RedirectToAction("EditImg", "Eventis", new { id = id });

        }
        // POST: Eventis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AvvisiProgrammati()
        {
            ViewBag.EventiCount = db.Eventis.Where(e => e.DataI < DateTime.Now && e.DataF > DateTime.Now).Count();
            var eventi = db.Eventis.Where(e => e.DataI < DateTime.Now && e.DataF > DateTime.Now).OrderByDescending(d => d.Data);
            return View(eventi);
        }
        public ActionResult AvvisiObsoletii()
        {
            ViewBag.EventiCount = db.Eventis.Where(e => e.DataF < DateTime.Now).Count();
            var eventi = db.Eventis.Where(e => e.DataF < DateTime.Now).OrderByDescending(d => d.Data);
            return View(eventi);
        }

    }
}
