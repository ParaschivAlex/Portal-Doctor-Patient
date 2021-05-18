using Microsoft.AspNet.Identity;
using PortalDocPat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalDocPat.Controllers
{
    public class DoctorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Doctors
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult New()
        {
            Doctor doctor = new Doctor();

            doctor.Spec = GetAllSpecializations();

            return View(doctor);
        }

        [HttpPost]
        public ActionResult New(Doctor doctor)
        {
           
            try
            {
                if (ModelState.IsValid)
                {
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    doctor.Spec = GetAllSpecializations();
                    return View(doctor);
                }
            }
            catch (Exception e)
            {
                doctor.Spec = GetAllSpecializations();
                return View(doctor);
            }
        }

        public ActionResult Edit(int id)
        {

            Doctor doctor = db.Doctors.Find(id);
            doctor.Spec = GetAllSpecializations();
            if (doctor.DoctorId.ToString() == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(doctor);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HttpPut]
        public ActionResult Edit(int id, Doctor requestDoctor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Doctor doctor = db.Doctors.Find(id);

                    if (doctor.DoctorId.ToString() == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(doctor))
                        {
                            doctor = requestDoctor;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    requestDoctor.Spec = GetAllSpecializations();
                    return View(requestDoctor);
                }
            }
            catch (Exception e)
            {
                requestDoctor.Spec = GetAllSpecializations();
                return View(requestDoctor);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor.DoctorId.ToString() == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Doctors.Remove(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllSpecializations()
        {
            var selectList = new List<SelectListItem>();

            var specs = from spec in db.Specialziations
                             select spec;

            foreach (var spec in specs)
            {
                selectList.Add(new SelectListItem
                {
                    Value = spec.SpecializationId.ToString(),
                    Text = spec.SpecializationName.ToString()
                });
            }
            return selectList;
        }

        [NonAction]
        public void CalculeazaRating(Doctor doc)
        {
            float rating_val = 0;
            int nr_reviews = 0;
            var reviews = db.Reviews.Where(a => a.DoctorId == doc.DoctorId.ToString());
            foreach (var rev in reviews)
            {
                rating_val = rating_val + rev.Grade;
                nr_reviews++;
            }
            rating_val = rating_val / nr_reviews;

            doc.Rating = rating_val;

        }
    }

}
