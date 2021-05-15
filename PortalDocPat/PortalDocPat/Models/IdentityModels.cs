using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PortalDocPat.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext()
		: base("DefaultConnection", throwIfV1Schema: false)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, PortalDocPat.Migrations.Configuration>("DefaultConnection"));
		}
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Specialization> Specialziations { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Consultation> Consultations { get; set; }
		public DbSet<Article> Articles { get; set; }


		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}