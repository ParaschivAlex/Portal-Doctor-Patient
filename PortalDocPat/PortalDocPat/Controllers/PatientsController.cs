using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalDocPat.Controllers
{
    public class PatientsController : Controller
    {
        // GET: Patients
        public ActionResult Index()
        {
            return View();
        }
    }
}using Microsoft.AspNet.Identity;
using PortalDocPat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalDocPat.Controllers
{
	public class PatientsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// [Authorize(Roles = "User,Doctor,Admin")]
		public ActionResult Show(int id)
		{
			Patient pat = db.Patients.Find(id);
			ViewBag.Patient = pat;
			ViewBag.Name = pat.Name;
			ViewBag.Sex = pat.Sex;
			ViewBag.BirthDay = pat.BirthDay;
			ViewBag.Age = pat.Age;
			ViewBag.utilizatorCurent = User.Identity.GetUserId();
			return View(pat);

		}

		[Authorize(Roles = "Admin")]
		public ActionResult New()
		{
			Patient pat = new Patient();
			return View(pat);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public ActionResult New(Patient patient)
		{
			try
			{
				db.Patients.Add(patient);
				db.SaveChanges();
				TempData["message"] = "Pacientul a fost adaugat!";
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				return View(e.ToString());
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