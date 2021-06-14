using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalDocPat.Models
{
	public class Article
	{
		[Key]
		public int ArticleId { get; set; }

		[Required(ErrorMessage = "Titlul este obligatoriu")]
		public string Title { get; set; }
		[Required(ErrorMessage = "Continutul articolului este obligatoriu")]
		public string Content { get; set; }
		[Required(ErrorMessage = "Data este obligatorie")]
		public DateTime Date { get; set; }

		public int DoctorId { get; set; }
		public virtual Doctor Doctor { get; set; }
	}
}