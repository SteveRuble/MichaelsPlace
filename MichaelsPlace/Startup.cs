using System.Data.Entity;
using MichaelsPlace.App_Start;
using MichaelsPlace.Models.Persistence;
using Microsoft.Owin;
using Ninject;
using Owin;

[assembly: OwinStartupAttribute(typeof(MichaelsPlace.Startup))]
namespace MichaelsPlace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureData(app);

            ConfigureAuth(app);

            ConfigureWebApi(app, NinjectWebCommon.Kernel);
        }


    }


}
