using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Linkofy.Startup))]
namespace Linkofy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
