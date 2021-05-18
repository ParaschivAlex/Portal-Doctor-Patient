using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalDocPat.Models
{
	public class Doctor
	{
		[Key]
		public int DoctorId { get; set; }

		[Required]
		public string Name { get; set; }
		
		public bool IsAvailable { get; set; }

		public string Phone { get; set; }
		[Required]
		public float PriceRate { get; set; }
		public float Rating { get; set; }

		public int SpecializationId { get; set; }
		public virtual Specialization Specialization { get; set; }
		public IEnumerable<SelectListItem> Spec { get; set; }

		public virtual ICollection<Article> Articles { get; set; }

		public virtual ICollection<Review> Reviews { get; set; }

		public virtual ICollection<Consultation> Consultations { get; set; }

		public int UserId { get; set; }
		public virtual ApplicationUser User { get; set; }
	}
}