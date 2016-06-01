using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Api;
using Microsoft.Owin;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Serilog;

namespace MichaelsPlace
{
    public class MichaelsPlaceModule : NinjectModule
    {
        private readonly bool _logToConsole;

        public MichaelsPlaceModule() : this(false) { }

        /// <summary>
        /// Extra constructor to configure logging for the test environment.
        /// </summary>
        /// <param name="logToConsole"></param>
        public MichaelsPlaceModule(bool logToConsole)
        {
            _logToConsole = logToConsole;
        }

        public override void Load()
        {
            ConfigureLogging();               
            
            Kernel.Bind(c => c.FromThisAssembly().SelectAllClasses().BindDefaultInterface());

            Kernel.Bind(c => c.FromThisAssembly()
                              .SelectAllClasses()
                              .InheritedFrom<Profile>()
                              .BindToSelf());

            Kernel.Bind<IMapper>().ToMethod(CreateMapper).InSingletonScope();
            
            Kernel.Rebind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

            ConfigureHttp();

            ConfigureAuthentication();

        }

        private void ConfigureAuthentication()
        {
        }

        public void ConfigureHttp()
        {
            Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current));
            Bind<IOwinContext>().ToMethod(ctx => HttpContext.Current.GetOwinContext());
        }

        public void ConfigureLogging()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log-{Date}.log");

            var config = new LoggerConfiguration()
                .ReadFrom.AppSettings();
            if (_logToConsole)
            {
                config = config.WriteTo.ColoredConsole();
            }
            else
            {
                config = config.WriteTo.RollingFile(path, fileSizeLimitBytes: 1000000, retainedFileCountLimit: 10)
                               .WriteTo.Glimpse();
            }

            var log = config.CreateLogger();

            Bind<ILogger>().ToConstant(log);
        }

        private static IMapper CreateMapper(IContext ctx)
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<ApiModelMappingProfile>();
                c.AddProfile<AdminModelMappingProfile>();
            });
            return configuration.CreateMapper();
        }
    }
}
