using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BullsAndCows.Web.Startup))]
namespace BullsAndCows.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
