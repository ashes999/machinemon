using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MachineMon.Web.Startup))]
namespace MachineMon.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
