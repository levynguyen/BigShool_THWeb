using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigShool_ThWeb.Startup))]
namespace BigShool_ThWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
