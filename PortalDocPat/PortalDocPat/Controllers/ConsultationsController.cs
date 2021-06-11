using Microsoft.AspNet.Identity;
using PortalDocPat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace PortalDocPat.Controllers
{
    public class ConsultationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Consultations

        public ActionResult New(int docId)
        {
            Debug.WriteLine(docId);
            ViewBag.DoctorId = docId;
            var userCurent = User.Identity.GetUserId();
            Patient pat = db.Patients.Where(i => i.UserId == userCurent).First();
            ViewBag.PatientId = pat.PatiendId;

            var consultatii = db.Consultations.Where(i => i.DoctorId == docId);
            List<DateTime> lista_consultatii = new List<DateTime>();
            foreach(Consultation c in consultatii)
            {
                lista_consultatii.Add(c.StartDate);
            }
            ViewBag.ListaConsultatii = lista_consultatii;
            return View();
        }

        [HttpPost]
        public ActionResult New(Consultation c)
        {
            try
            {
                db.Consultations.Add(c);
                db.SaveChanges();
                return Redirect("/Doctors/Index");
            }
            catch (Exception e)
            {
                return View();
            }
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