using System.Data.Entity;
using IdentityManager;
using IdentityManager.Configuration;
using MichaelsPlace.App_Start;
using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity;
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
            var log = NinjectWebCommon.Kernel.Get<Serilog.ILogger>();

            log.Information("Application starting...");

            ConfigureData(app);

            ConfigureAuth(app);

            ConfigureWebApi(app, NinjectWebCommon.Kernel);

            ConfigureIdentityManager(app);

            log.Information("Application started.");
        }


    }


}
