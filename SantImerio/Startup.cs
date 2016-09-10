using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SantImerio.Startup))]
namespace SantImerio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
