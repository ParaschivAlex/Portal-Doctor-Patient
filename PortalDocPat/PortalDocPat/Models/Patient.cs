using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace PortalDocPat.Models
{
	public class Patient
	{
		[Key]
		public int PatiendId { get; set; }

		[Required(ErrorMessage = "Numele este obligatoriu")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Sexul este obligatoriu")]
		public string Sex { get; set; }

		[Required(ErrorMessage = "Data nasterii este obligatorie")]
		public DateTime BirthDay { get; set; }

		public float Kg { get; set; }

		public string Allergies { get; set; }


		public int Age
		{
			get
			{
				var now = DateTime.Today;
				var age = now.Year - BirthDay.Year;
				if (BirthDay > now.AddYears(-age)) age--;
				return age;
			}

		}

		public virtual ICollection<Consultation> Consultations { get; set; }

		public int UserId { get; set; }
		public virtual ApplicationUser User { get; set; }

	}
}