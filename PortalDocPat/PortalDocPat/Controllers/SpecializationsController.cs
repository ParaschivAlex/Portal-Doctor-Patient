using PortalDocPat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalDocPat.Controllers
{
	[Authorize(Roles = "Admin")]
	public class SpecializationsController : Controller
	{
			private ApplicationDbContext db = new ApplicationDbContext();

			public ActionResult Index()
			{
				var specializations = db.Specializations.Include("Doctors").OrderBy(spec => spec.SpecializationName);

				if (TempData.ContainsKey("message"))
				{
					ViewBag.message = TempData["message"].ToString();
				}
				ViewBag.Specializations = specializations;
				return View();
			}
			public ActionResult Show(int id)
			{
				try
				{
					Specialization specialization = db.Specializations.Find(id);
					ViewBag.Specialization = specialization;
					ViewBag.Doctors = specialization.Doctors;

					return View(specialization);
				}

				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return RedirectToAction("Index");
				}
			}
			public ActionResult New()
			{
				return View();
			}

			[HttpPost]
			public ActionResult New(Specialization specialization)
			{
				try
				{
					if (ModelState.IsValid)
					{
						db.Specializations.Add(specialization);
						db.SaveChanges();
						TempData["message"] = "Specializarea a fost adaugata!";
						return RedirectToAction("Index");
					}
					else
					{
						return View(specialization);
					}
				}

				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return RedirectToAction("Index");
				}
			}

			public ActionResult Edit(int id)
			{
				Specialization specialization = db.Specializations.Find(id);
				return View(specialization);
			}


			[HttpPut]
			public ActionResult Edit(int id, Specialization requestSpecialization)
			{
				try
				{
					if (ModelState.IsValid)
					{
						Specialization specialization = db.Specializations.Find(id);
						if (TryUpdateModel(specialization))
						{
							specialization.SpecializationName = requestSpecialization.SpecializationName;
							db.SaveChanges();
							TempData["message"] = "Specializarea a fost modificata!";
							return RedirectToAction("Index");
						}
						return RedirectToAction("Index");
					}
					return RedirectToAction("Index");
				}

				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return RedirectToAction("Index");
				}
			}

			[HttpDelete]
			public ActionResult Delete(int id)
			{
				try
				{
					Specialization specialization = db.Specializations.Find(id);
					db.Specializations.Remove(specialization);
					TempData["message"] = "Specializarea a fost stearsa!";
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return RedirectToAction("Index");
				}
			}

	}
}