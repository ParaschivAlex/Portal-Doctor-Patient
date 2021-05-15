using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalDocPat.Models
{
	public class Consultation
	{
		[Key]
		public int ConsultationId { get; set; }

		public int DoctorId { get; set; }
		public virtual Doctor Doctor { get; set; }

		public int PatientId { get; set; }
		public virtual Patient Patient { get; set; }

		[Required(ErrorMessage = "Descrierea este obligatorie")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Data este obligatorie")]
		public DateTime StartDate { get; set; }

		/*
		 * [Required(ErrorMessage = "Data este obligatorie")]
		public DateTime EndDate { get; set; }
		*/

		[Required(ErrorMessage = "Titlul consultatiei este obligatoriu")]
		public DateTime ConsultationName { get; set; }

		public bool CancelStatus { get; set; }

	}
}