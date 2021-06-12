using Microsoft.AspNet.Identity;
using PortalDocPat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalDocPat.Controllers
{
    public class ReviewsController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Reviews
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult New(Review rev)
		{
			rev.Date = DateTime.Now;
			rev.UserId = User.Identity.GetUserId();
			try
			{

				if (ModelState.IsValid)
				{
					db.Reviews.Add(rev);
					db.SaveChanges();
					TempData["message"] = "Review adaugat cu succes";
					return Redirect("/Doctors/Show/" + rev.PatientId);
				}
				return Redirect("/Doctors/Show/" + rev.PatientId);
			}
			catch (Exception)
			{ return Redirect("/Doctors/Show/" + rev.PatientId); }
		}

		[HttpDelete]
		public ActionResult Delete(int id)
		{
			Review rev = db.Reviews.Find(id);
			if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
			{
				db.Reviews.Remove(rev);
				db.SaveChanges();
				return Redirect("/Doctors/Show/" + rev.DoctorId);
			}
			else
			{
				TempData["message"] = "Nu aveti dreptul sa stergeti un review care nu va apartine";
				return RedirectToAction("Index", "Home");
			}
		}


		public ActionResult Edit(int id)
		{	
			Review rev = db.Reviews.Find(id);
			if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
			{
				ViewBag.Review = rev;
				return View(rev);
			}
			else
			{
				TempData["message"] = "Nu aveti dreptul sa faceti modificari";
				return RedirectToAction("Index", "Home");
			}

		}

		[HttpPut]
		public ActionResult Edit(int id, Review requestReview)
		{
			try
			{
				Review rev = db.Reviews.Find(id);

				if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
				{
					if (TryUpdateModel(rev))
					{
						rev.Comment = requestReview.Comment;
						rev.Grade = requestReview.Grade;
						db.SaveChanges();
					}
					return Redirect("/Doctors/Show/" + rev.DoctorId);
				}
				else
				{
					TempData["message"] = "Nu aveti dreptul sa faceti modificari";
					return RedirectToAction("Index", "Home");
				}

			}
			catch (Exception e)
			{
				return View(requestReview);
			}

		}
		[Authorize(Roles = "Admin")]
		public ActionResult Show(int id)
		{
			Review review = db.Reviews.Find(id);
			ViewBag.Review = review;
			return View();
		}
	}
}