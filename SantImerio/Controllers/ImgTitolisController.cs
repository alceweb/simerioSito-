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

namespace SantImerio.Controllers
{
    [Authorize(Roles ="Admin,Fotografo")]
    public class ImgTitolisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ImgTitolis
        public ActionResult Index()
        {
            var imgTitolis = db.ImgTitolis.Include(i => i.Titolo);
            return View(imgTitolis.ToList());
        }

        // GET: ImgTitolis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImgTitoli imgTitoli = db.ImgTitolis.Find(id);
            if (imgTitoli == null)
            {
                return HttpNotFound();
            }

            return View(imgTitoli);
        }

        // GET: ImgTitolis/Create
        public ActionResult Create(int id)
        {
            ViewBag.Evento_Id = new SelectList(db.Eventis, "Evento_Id", "Titolo");
            return View();
        }

        // POST: ImgTitolis/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImgTitolo_Id,Img,ImgTitolo,Evento_Id")] ImgTitoli imgTitoli, HttpPostedFileBase file, int id)
        {
            if (ModelState.IsValid)
            {
                    if (file != null)
                        try
                        {
                            imgTitoli.Evento_Id = id;
                            var fileName = Path.GetFileName(file.FileName);
                            imgTitoli.Img = Path.GetFileName(file.FileName);
                            db.ImgTitolis.Add(imgTitoli);
                            db.SaveChanges();
                           
                            var path = Path.Combine(Server.MapPath("~/Content/Immagini/Eventi/" + imgTitoli.Evento_Id + "/"), fileName);
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
                            return RedirectToAction("Evento", "Eventis", new {id = id });

                        }
                    catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                    else
                    {
                        ViewBag.Message = "Devi scegliere un file";
                        return View();
                }
           }

            ViewBag.Evento_Id = new SelectList(db.Eventis, "Evento_Id", "Titolo", imgTitoli.Evento_Id);
            return View(imgTitoli);
        }

        // GET: ImgTitolis/Edit/5
        public ActionResult Edit(int? id, int evento)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImgTitoli imgTitoli = db.ImgTitolis.Find(id);
            if (imgTitoli == null)
            {
                return HttpNotFound();
            }
            ViewBag.Evento_Id = new SelectList(db.Eventis, "Evento_Id", "Titolo", imgTitoli.Evento_Id);
            return View(imgTitoli);
        }

        // POST: ImgTitolis/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImgTitolo_Id,Img,ImgTitolo,Evento_Id")] ImgTitoli imgTitoli, int evento)
        {
            if (ModelState.IsValid)
            {
                imgTitoli.Evento_Id = evento;
                db.Entry(imgTitoli).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Evento", "Eventis", new { id = evento });
            }
            ViewBag.Evento_Id = new SelectList(db.Eventis, "Evento_Id", "Titolo", imgTitoli.Evento_Id);
            return View(imgTitoli);
        }

        // GET: ImgTitolis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImgTitoli imgTitoli = db.ImgTitolis.Find(id);
            if (imgTitoli == null)
            {
                return HttpNotFound();
            }
            return View(imgTitoli);
        }

        // POST: ImgTitolis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Collaboratore")]
        public ActionResult DeleteConfirmed(int id)
        {
            ImgTitoli imgTitoli = db.ImgTitolis.Find(id);
            db.ImgTitolis.Remove(imgTitoli);
            db.SaveChanges();
            var file = "~/Content/Immagini/Eventi/" + imgTitoli.Evento_Id + "/" + imgTitoli.Img;
            System.IO.File.Delete(Server.MapPath(file));

            return RedirectToAction("EditImg", "Eventis", new { id = imgTitoli.Evento_Id });
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
