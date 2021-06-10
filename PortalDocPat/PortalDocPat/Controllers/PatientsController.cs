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
    public class PatientsController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();

        
		public ActionResult Show()
		{
            try
            {
                var userCurent = User.Identity.GetUserId();
                Patient pat = db.Patients.Where(i => i.UserId == userCurent).First();

                
                ViewBag.Patient = pat;
                ViewBag.Name = pat.Name;
                ViewBag.Sex = pat.Sex;
                ViewBag.BirthDay = pat.BirthDay;

                return View();
            }
            catch (Exception e)
            {
                
                return RedirectToAction("New");
            }

		}

		public ActionResult New()
		{
			Patient pat = new Patient();
            
			return View(pat);
		}

		[HttpPost]
		public ActionResult New(Patient patient)
		{
			try
			{
                patient.UserId = User.Identity.GetUserId();
                db.Patients.Add(patient);
				db.SaveChanges();
				TempData["message"] = "Informatiile au fost adaugate!";
				return RedirectToAction("Show");
			}
			catch (Exception e)
			{
				return View();
			}
		}

		[Authorize(Roles = "User,Admin")]
		public ActionResult Edit(int id)
		{

			Patient patient = db.Patients.Find(id);
			ViewBag.Patient = patient;
			ViewBag.Name = patient.Name;
			ViewBag.utilizatorCurent = User.Identity.GetUserId();
			return View(patient);
		}


		[HttpPut]
		[Authorize(Roles = "User,Admin")]
		public ActionResult Edit(int id, Patient requestPatient)
		{
			try
			{
				Patient pat = db.Patients.Find(id);
				if (pat.PatiendId == int.Parse(User.Identity.GetUserId()) || pat.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
				{
					pat = requestPatient;
					db.SaveChanges();
					TempData["message"] = "Pacientul a fost modificat!";
					return RedirectToAction("Index");
				}
				else
				{
					TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui pacient";
					return RedirectToAction("Index");
				}
			}
			catch (Exception e)
			{
				return View(e.ToString());
			}
		}

		[HttpDelete]
		[Authorize(Roles = "User,Admin")]
		public ActionResult Delete(int id)
		{
			Patient pat = db.Patients.Find(id);

			if (pat.PatiendId == int.Parse(User.Identity.GetUserId()) || pat.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
			{
				db.Patients.Remove(pat);
				db.SaveChanges();
				TempData["message"] = "Pacientul a fost sters!";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["message"] = "Nu aveti dreptul sa stergeti un pacient";
				return RedirectToAction("Index");
			}
		}
	}
}