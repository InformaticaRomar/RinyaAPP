using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(rinya_app.Startup))]
namespace rinya_app
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
