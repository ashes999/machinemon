using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MachineMon.Startup))]
namespace MachineMon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
