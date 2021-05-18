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

		[Required]
		public string Title { get; set; }
		[Required]
		public string Content { get; set; }
		[Required]
		public DateTime Date { get; set; }

		public int DoctorId { get; set; }
		public virtual Doctor Doctor { get; set; }
	}
}