using SantImerio.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.IO;


namespace SantImerio.Controllers
{
    public class HomeController : Controller
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
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Index()
        {
            var eventiH = db.Eventis.Where(p => p.Home == true).OrderByDescending(d => d.Data);
            var eventi = db.Eventis.Where(g => g.Pastorale == true).OrderByDescending(d => d.Data);
            ViewBag.Eventi = eventi;
            return View(eventiH);
        }

        public ActionResult Index1()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Appuntamenti()
        {
            ViewBag.Message = "Appuntamenti";
            var oggi = DateTime.Today;
            var orari = db.OrariMesseBars;
            ViewBag.Orari = orari.Where(o=>o.Messe_Id == 1);
            ViewBag.Orari1 = orari.Where(o => o.Messe_Id == 2);
            var eventi = db.Eventis.Where(d => d.DataI < oggi && d.DataF > oggi && d.Data > oggi && d.Pubblica==true).OrderBy(d => d.Data);
            if (eventi == null)
            {
                return HttpNotFound();
            }
            return View(eventi);
        }

        public async Task<ActionResult> Comunità()
        {
            ViewBag.UtentiCount = UserManager.Users.Count();
            return View(await UserManager.Users.ToListAsync());
        }

        public ActionResult OrariMesse()
        {
            var orari = db.OrariMesseBars.Where(o => o.Messe_Id == 1);
            return View(orari);
        }

        [Authorize(Roles ="Admin,Bar")]
        public ActionResult EditOrariMesse(int? id)
        {
            OrariMesseBar orari = db.OrariMesseBars.Find(id);
            return View(orari);
        }

        [Authorize(Roles = "Admin,Bar")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditOrariMesse([Bind(Include = "Messe_Id,Tipo,Descrizione")] OrariMesseBar orari)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orari).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Appuntamenti", "Home");
            }
            return View(orari);
        }

        public ActionResult Sfondo()
        {
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/GalleriaHome/")).ToList();
            return View(immagini);
        }

        [HttpPost]
        public ActionResult Sfondo(HttpPostedFileBase file)
        {
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/GalleriaHome/")).ToList();
            if (file != null)
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Immagini/GalleriaHome/" + fileName));
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
                        }
                        else
                        {
                            img.Resize(800 / rapportoV, 800);
                            img.Save(path);
                        }
                    }
                    else
                    {
                        if (rapportoO >= 1)
                        {
                            ViewBag.Message = "Attendi la fine del download...";
                            img.Save(path);
                        }
                        else
                        {
                            ViewBag.Message = "Attendi la fine del download...";
                            img.Save(path);
                        }
                    }

                    ViewBag.Message = "Immagine caricata correttamente";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "Devi scegliere un file";
            }

            return RedirectToAction("Sfondo", "Home");
        }

        public ActionResult DeleteSfondo()
        {
            ViewBag.File = Request.QueryString["nomefile"];
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSfondo(string nomefile)
        {
            var file = "~/Content/Immagini/GalleriaHome/" + Request.QueryString["nomefile"];
            System.IO.File.Delete(Server.MapPath(file));
            return RedirectToAction("Sfondo", "Home");

        }

        public ActionResult ConfermaSfondo()
        {
            var immagini = Directory.GetFiles(Server.MapPath("/Content/Immagini/GalleriaHome/")).ToList();
            return View(immagini);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [HttpPost]
        public ActionResult ConfermaSfondo(string nomefile)
        {
            var file = "/Content/Immagini/GalleriaHome/" + Request.QueryString["nomefile"];
            var file1 = "/Content/Immagini/GalleriaHome/Sfondo/Home.jpg";
            System.IO.File.Delete(Server.MapPath(file1));
            System.IO.File.Copy(Server.MapPath(file), Server.MapPath(file1));
            return RedirectToAction("Sfondo", "Home");
        }

        public ActionResult Download()
        {
            ViewBag.DocumentiCount = db.Documentis.Count();
            var documenti = db.Documentis.OrderByDescending(d => d.Titolo);
            return View(documenti);
        }

        public ActionResult InfoCookie()
        {
            return View();
        }
        public ActionResult Storia()
        {
            return View();
        }
    }
}