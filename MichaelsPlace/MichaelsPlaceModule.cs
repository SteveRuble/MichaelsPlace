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
