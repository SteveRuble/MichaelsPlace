using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MichaelsPlace.Startup))]
namespace MichaelsPlace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
