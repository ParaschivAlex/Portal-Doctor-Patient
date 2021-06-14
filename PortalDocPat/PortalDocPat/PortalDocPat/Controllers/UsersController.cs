using System;
using Microsoft.AspNet.Identity.EntityFramework;
using PortalDocPat.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PortalDocPat.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Users
		public ActionResult Index()
		{
			var users = from user in db.Users
						orderby user.UserName
						select user;
			ViewBag.UsersList = users;
			return View();
		}

		[HttpDelete]
		public ActionResult Delete(string id)
		{
			ApplicationDbContext context = new ApplicationDbContext();

			var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
			var user = UserManager.Users.FirstOrDefault(u => u.Id == id.ToString());
			try
			{
				var patient = db.Patients.Where(u => u.UserId == id).First();
				var cons = db.Consultations.Where(c => c.PatientId == patient.PatiendId);
				if (cons != null)
				{
					foreach (var con in cons)
					{
						db.Consultations.Remove(con);
					}
				}

				// Commit pe articles
				db.SaveChanges();
				UserManager.Delete(user);
				return RedirectToAction("Index");
			}
			catch(Exception e)
			{
				UserManager.Delete(user);
				return RedirectToAction("Index");
			}
			
		}
	}
}