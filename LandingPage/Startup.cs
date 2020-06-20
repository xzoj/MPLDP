using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LandingPage.Startup))]
namespace LandingPage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
