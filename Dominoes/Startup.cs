using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dominoes.Startup))]
namespace Dominoes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
