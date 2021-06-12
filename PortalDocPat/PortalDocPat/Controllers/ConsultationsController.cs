using Microsoft.AspNet.Identity;
using PortalDocPat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace PortalDocPat.Controllers
{
    public class ConsultationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Consultations

        public ActionResult New(int id)
        {
            
            ViewBag.DoctorId = id;
            var userCurent = User.Identity.GetUserId();
            Patient pat = db.Patients.Where(i => i.UserId == userCurent).First();
            ViewBag.PatientId = pat.PatiendId;

            var consultatii = db.Consultations.Where(i => i.DoctorId == id);
            List<DateTime> lista_consultatii = new List<DateTime>();
            foreach(Consultation c in consultatii)
            {
                lista_consultatii.Add(c.StartDate);
            }
            ViewBag.ListaConsultatii = lista_consultatii;

            Debug.WriteLine(id);
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

		private void SendEmailNotification(string toEmail, string subject, string content)
		{
			const string senderEmail = "mdstestportal@gmail.com";
			const string senderPassword = "!1AdminAdmin";
			const string smtpServer = "smtp.gmail.com";
			const int smtpPort = 587;

			SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.EnableSsl = true;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

			MailMessage email = new MailMessage(senderEmail, toEmail, subject, content);
			email.IsBodyHtml = true;
			email.BodyEncoding = UTF8Encoding.UTF8;

			try
			{
				System.Diagnostics.Debug.WriteLine("Emailul se trimite...");
				smtpClient.Send(email);
				System.Diagnostics.Debug.WriteLine("Emailul a fost trimis cu succes!");
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.WriteLine("A aparut o eroare la trimiterea emailului.");
				System.Diagnostics.Debug.WriteLine(e.Message.ToString());
			}
		}
    }
}