using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PortalDoctorPatient.Startup))]
namespace PortalDoctorPatient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
