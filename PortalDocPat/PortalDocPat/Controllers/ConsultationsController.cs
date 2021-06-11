using PortalDocPat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalDocPat.Controllers
{
    public class ConsultationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Consultations

        public ActionResult New()
        {
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        { 
            Consultation c = db.Consultations.Find(id);
            db.Consultations.Remove(c);
            db.SaveChanges();
            return Redirect("/Patients/Show");
        }
    }
}