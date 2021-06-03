using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalDocPat.Models
{
	public class Specialization
	{
		[Key]
		public int SpecializationId { get; set; }

		[Required(ErrorMessage = "Numele specializarii este obligatoriu")]
		public string SpecializationName { get; set; }
		[Required(ErrorMessage = "Pretul mediu este obligatoriu!")]
		public float Price { get; set; }

		public virtual ICollection<Doctor> Doctors { get; set; }
	}
}