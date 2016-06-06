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
    /// <summary>
    /// Handles configuration of application.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures app.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            var resolutionRoot = NinjectWebCommon.ResolutionRoot;

            var log = resolutionRoot.Get<Serilog.ILogger>();

            log.Information("Application starting...");

            ConfigureData(app);

            ConfigureAuth(app, resolutionRoot);

            ConfigureWebApi(app, resolutionRoot);

            ConfigureIdentityManager(app);

            log.Information("Application started.");
        }


    }


}
