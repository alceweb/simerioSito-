using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SantImerio.Models;
using System.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SantImerio.Controllers
{
         [Authorize]
   public class StatistichesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DateFormatString = "dd/MMM/yy"};
        JsonSerializerSettings _jsonSetting1 = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DateFormatString = "MMM yy" };

        // GET: Statistiches
        public ActionResult Index()
        {
            var statistiche = db.Statistiches.ToList();
            //DataView per grafico registrati
            ViewBag.DataPoints = JsonConvert.SerializeObject(db.Statistiches
                .Where(u => u.UName != "CesareRocchetti" && u.UName != "DonMicheleRocchetti" && u.UName != "anonimous")
                .GroupBy(d => d.UName)
                .Select(s => new { x = s.Key, y = s.Count() })
                .OrderBy(s => s.x)
                .ToList(), _jsonSetting);
            //DataView per grafico mensile
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(db.Statistiches
                .GroupBy(d => d.Data.Month)
                .Select(s => new { x = s.Key, y = s.Count() })
                .OrderBy(s =>s.x)
                .ToList(), _jsonSetting1);
            //DataView per grafico giornaliero
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(db.Statistiches
                .GroupBy(d => DbFunctions.TruncateTime(d.Data))
                .Select(s => new { x = s.Key, y = s.Count()})
                .OrderBy(s => s.x)
                .ToList(), _jsonSetting);
            
            return View(statistiche);
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
