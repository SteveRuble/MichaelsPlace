using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MichaelsPlace.Models.Api;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Serilog;

namespace MichaelsPlace
{
    public class MichaelsPlaceModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(c => c.FromThisAssembly().SelectAllClasses().BindDefaultInterface());

            Kernel.Bind(c => c.FromThisAssembly()
                              .SelectAllClasses()
                              .InheritedFrom<Profile>()
                              .BindToSelf());

            Kernel.Bind<IMapper>().ToMethod(CreateMapper).InSingletonScope();

            ConfigureLogging();
        }

        private void ConfigureLogging()
        {
            var log = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.RollingFile("log-{Date}.txt")
                .WriteTo.Glimpse()

                .CreateLogger();

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
