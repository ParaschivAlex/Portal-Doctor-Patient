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

        public ActionResult New(int id)
        {
            Consultation cons = new Consultation();
            Doctor doctor = db.Doctors.Find(id);
            cons.DoctorId = doctor.DoctorId;
            Debug.WriteLine(cons.DoctorId);
            var userCurent = User.Identity.GetUserId();
            Patient pat = db.Patients.Where(i => i.UserId == userCurent).First();
            cons.PatientId = pat.PatiendId;

            var consultatii = db.Consultations.Where(i => i.DoctorId == id);
            List<DateTime> lista_consultatii = new List<DateTime>();
            foreach(Consultation c in consultatii)
            {
                lista_consultatii.Add(c.StartDate);
            }
            ViewBag.ListaConsultatii = lista_consultatii;
            ViewBag.DoctorId = cons.DoctorId;
            return View(cons);
        }

        [HttpPost]
        public ActionResult New(Consultation c)
        {
            try
            {
                Debug.WriteLine("Doctor id " + c.DoctorId);
                Debug.WriteLine("Patient id " + c.PatientId);
                Debug.WriteLine("Zi " + c.StartDate);
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