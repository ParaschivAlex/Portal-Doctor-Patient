
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalDocPat.Models
{
	public class Review
	{
		[Key]
		public int ReviewId { get; set; }

		[Required]
		[Range(1, 10, ErrorMessage = "Review-ul trebuie sa aiba o nota")]
		public int Grade { get; set; } //de la 1 la 10

		[MaxLength(150)]
		public string Comment { get; set; }

		public DateTime Date { get; set; }

		public int DoctorId { get; set; }
		public virtual Doctor Doctor { get; set; }

		public int PatientId { get; set; }
		public virtual Patient Patient { get; set; }

		public string UserId { get; internal set; }
	}
}