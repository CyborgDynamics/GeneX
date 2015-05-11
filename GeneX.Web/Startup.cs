using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GeneX.Web.Startup))]
namespace GeneX.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
