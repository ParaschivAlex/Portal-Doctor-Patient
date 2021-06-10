using Microsoft.AspNet.Identity;
using PortalDocPat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalDocPat.Controllers
{
	public class DoctorsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: Doctors
		private int _perPage = 9;

		private List<Doctor> doctoriPtSortare;
		// GET: Products
		public ActionResult Index()
		{
			var doctors = db.Doctors.Include("Specialization").Include("User").OrderBy(a => a.Name);
			if (doctoriPtSortare == null)
			{
				doctoriPtSortare = doctors.ToList();
			}

			if (TempData.ContainsKey("Doctori"))
			{
				doctoriPtSortare = TempData["Doctori"] as List<Doctor>;
			}


			var search = "";

			if (Request.Params.Get("search") != null)
			{
				search = Request.Params.Get("search").Trim();
				List<int> doctorsIds = db.Doctors.Where(pr => pr.Name.Contains(search)).Select(p => p.DoctorId).ToList();

				List<int> reviewIds = db.Reviews.Where(rev => rev.Comment.Contains(search)).Select(rev => rev.DoctorId).ToList();
				List<int> mergedIds = doctorsIds.Union(reviewIds).ToList();

				doctors = db.Doctors.Where(doc => mergedIds.Contains(doc.DoctorId)).Include("Specialization").Include("User").OrderBy(p => p.Name);
				doctoriPtSortare = doctors.ToList();
			}

			foreach (var doc in doctoriPtSortare)
			{
				CalculeazaRating(doc);
			}

			var totalDocs = doctoriPtSortare.Count();

			var currentPage = Convert.ToInt32(Request.Params.Get("page"));

			var offset = 0;

			if (!currentPage.Equals(0))
			{
				offset = (currentPage - 1) * this._perPage;
			}
			var paginatedDocs = doctoriPtSortare.Skip(offset).Take(this._perPage);

			if (TempData.ContainsKey("message"))
			{
				ViewBag.message = TempData["message"].ToString();
			}

			//ViewBag.perPage = this._perPage;
			ViewBag.total = totalDocs;
			ViewBag.lastPage = Math.Ceiling((float)totalDocs / (float)this._perPage);
			ViewBag.Doctors = paginatedDocs;

			ViewBag.SearchString = search;

			return View();
		}

		public ActionResult SortareDoctori(int id)
		{
			switch (id)
			{
				case 1:
					doctoriPtSortare = db.Doctors.Include("Specialization").Include("User").OrderBy(a => a.PriceRate).ToList();
					break;
				case 2:
					doctoriPtSortare = db.Doctors.Include("Specialization").Include("User").OrderBy(a => a.PriceRate).ToList();
					doctoriPtSortare.Reverse();
					break;
				case 3:
					doctoriPtSortare = db.Doctors.Include("Specialization").Include("User").OrderBy(a => a.Rating).ToList();
					break;
				case 4:
					doctoriPtSortare = db.Doctors.Include("Specialization").Include("User").OrderBy(a => a.Rating).ToList();
					doctoriPtSortare.Reverse();
					break;
				case 5:
					doctoriPtSortare = db.Doctors.Include("Specialization").Include("User").OrderBy(a => a.Name).ToList();
					break;
				default:
					break;
			}
			TempData["Doctori"] = doctoriPtSortare;
			return Redirect("/Doctors/Index");
		}

		public ActionResult Show(int id)
		{
			Doctor doctor = db.Doctors.Find(id);
			CalculeazaRating(doctor);
			SetAccessRights();
			var reviews = db.Reviews.Where(a => a.DoctorId == id);
			if (reviews.Count() != 0)
			{
				db.SaveChanges();
			}
			else
			{
				doctor.Rating = 0;
				db.SaveChanges();
			}
			if (TempData.ContainsKey("message"))
			{
				ViewBag.message = TempData["message"].ToString();
			}
			return View(doctor);

		}

		[HttpPost]
		public ActionResult NewReview(Review rev)
		{
			rev.Date = DateTime.Now;
			rev.UserId = User.Identity.GetUserId();
			try
			{
				if (ModelState.IsValid)
				{
					db.Reviews.Add(rev);
					db.SaveChanges();
					return Redirect("/Doctors/Show/" + rev.DoctorId);
				}
				else
				{
					Doctor d = db.Doctors.Find(rev.DoctorId);
					SetAccessRights();
					TempData["message"] = "Campul nu poate fi necompletat!";
					return Redirect("/Doctors/Show/" + rev.DoctorId);

				}
			}

			catch (Exception e)
			{
				Doctor d = db.Doctors.Find(rev.DoctorId);
				SetAccessRights();
				return View(d);
			}

		}

		[Authorize(Roles = "Admin")]
		public ActionResult New()
		{
			Doctor doc = new Doctor
			{
				Spec = GetAllSpecializations()
			};
			return View(doc);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public ActionResult New(Doctor doctor)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Doctors.Add(doctor);
					db.SaveChanges();
					TempData["message"] = "Doctorul a fost adaugat!";
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

		[Authorize(Roles = "Doctor,Admin")]
		public ActionResult Edit(int id)
		{

			Doctor doctor = db.Doctors.Find(id);
			ViewBag.Doctor = doctor;
			ViewBag.Specialization = doctor.Specialization;
			ViewBag.utilizatorCurent = User.Identity.GetUserId();
			doctor.Spec = GetAllSpecializations();
			return View(doctor);
		}


		[HttpPut]
		[Authorize(Roles = "Doctor,Admin")]
		public ActionResult Edit(int id, Doctor requestDoctor)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Doctor doctor = db.Doctors.Find(id);

					if (doctor.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
					{
						if (TryUpdateModel(doctor))
						{
							doctor = requestDoctor;
							db.SaveChanges();
							TempData["message"] = "Doctorul a fost modificat!";
						}
						return RedirectToAction("Index");
					}
					else
					{
						TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui doctor";
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
		[Authorize(Roles = "Doctor,Admin")]
		public ActionResult Delete(int id)
		{
			Doctor doc = db.Doctors.Find(id);

			if (doc.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
			{
				db.Doctors.Remove(doc);
				db.SaveChanges();
				TempData["message"] = "Produsul a fost sters!";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["message"] = "Nu aveti dreptul sa stergeti un doctor";
				return RedirectToAction("Index");
			}
		}

		private void SetAccessRights()
		{
			ViewBag.esteAdmin = User.IsInRole("Admin");
			ViewBag.UtilizatorCurent = User.Identity.GetUserId();
		}

		[NonAction]
		public IEnumerable<SelectListItem> GetAllSpecializations()
		{
			var selectList = new List<SelectListItem>();

			var specs = from sp in db.Specializations
							 select sp;

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
			var reviews = db.Reviews.Where(a => a.DoctorId == doc.DoctorId);
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