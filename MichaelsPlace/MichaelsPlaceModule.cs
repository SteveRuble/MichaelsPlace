using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web;
using AutoMapper;
using MediatR;
using MichaelsPlace.Handlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Services;
using MichaelsPlace.Services.Messaging;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Ninject;
using Ninject.Activation;
using Ninject.Components;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
using Ninject.Infrastructure;
using Ninject.Modules;
using Ninject.Planning.Bindings;
using Ninject.Planning.Bindings.Resolvers;
using Ninject.Web.Common;
using Serilog;
using Serilog.Filters;

namespace MichaelsPlace
{
    /// <summary>
    /// Container class for <see cref="NinjectModule"/> implementations,
    /// so that they can be concise and re-usable without being scattered all over the application.
    /// </summary>
    public class Modules
    {
        public class EntityFramework : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind<ApplicationDbContext>().ToSelf().WhenInjectedInto<SingleEntityService>();
                Kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();

                Kernel.Bind<IApplicationDbContextFactory>().ToFactory();
            }
        }

        public class Http : NinjectModule
        {
            public override void Load()
            {
                Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current));
                Bind<IOwinContext>().ToMethod(ctx => HttpContext.Current.GetOwinContext());

                Bind<IAuthenticationManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Authentication);
                Bind<ApplicationUserManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>());

                Bind<ApplicationRoleStore>().ToSelf().InRequestScope();
                Bind<ApplicationRoleManager>().ToSelf().InRequestScope();

            }
        }

        public class Logging : NinjectModule
        {
            public override void Load()
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log-{Date}.log");

                var config = new LoggerConfiguration()
                    .ReadFrom.AppSettings()
                    .Filter.ByExcluding(Matching.FromSource(typeof(ApplicationDbContext).FullName))
                    .WriteTo.RollingFile(path, fileSizeLimitBytes: 1000000, retainedFileCountLimit: 10)
                    .WriteTo.Glimpse();


                var log = config.CreateLogger();

                Bind<ILogger>().ToConstant(log);

            }
        }

        public class Mapping : NinjectModule
        {
            public override void Load()
            {
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

        public class Services : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind<IEmailService>().To<DevelopmentEmailService>().InSingletonScope();
                Kernel.Bind<ISmsService>().To<DevelopmentSmsService>().InSingletonScope();
                Kernel.Bind<ISingleEntityService>().To<SingleEntityService>().InSingletonScope();
            }
        }

        public class Mediatr : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>().SelectAllClasses().BindDefaultInterface());
                Kernel.Bind(scan => scan.FromAssemblyContaining<MichaelsPlace.Modules>()
                                        .SelectAllClasses()
                                        .InheritedFrom(typeof(IAsyncNotificationHandler<>))
                                        .BindAllInterfaces());

                Kernel.Bind(scan => scan.FromAssemblyContaining<MichaelsPlace.Modules>()
                                        .SelectAllClasses()
                                        .InheritedFrom(typeof(IAsyncRequestHandler<,>))
                                        .BindAllInterfaces());
                
                Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
                Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

                Rebind<IMediator>().To<Mediator>().InSingletonScope();
            }
        }
    }


    public class ContravariantBindingResolver : NinjectComponent, IBindingResolver
    {
        /// <summary>
        /// Returns any bindings from the specified collection that match the specified service.
        /// </summary>
        public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
        {
            if (service.IsGenericType)
            {
                var genericType = service.GetGenericTypeDefinition();
                var genericArguments = genericType.GetGenericArguments();
                if (genericArguments.Any(ga => ga.GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant)))
                {
                    var requestedGenericArguments = service.GetGenericArguments();
                    var candidateBindings = bindings.Where(binding => binding.Key.IsGenericType
                                                                      && binding.Key.GetGenericTypeDefinition() == genericType);


                    var matches = new List<IBinding>();

                    foreach (var candidateBinding in candidateBindings)
                    {
                        var candidateGenericArguments = candidateBinding.Key.GetGenericArguments();
                        var matched = true;
                        var contra = false;
                        for (int i = 0; i < candidateGenericArguments.Length; i++)
                        {
                            contra |= candidateGenericArguments[i] != requestedGenericArguments[i];
                            matched &= genericArguments[i].GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant)
                                       && candidateGenericArguments[i].IsAssignableFrom(requestedGenericArguments[i]);
                        }
                        if (contra && matched)
                        {
                            matches.AddRange(candidateBinding.Value);
                        }
                    }

                    return matches;
                }
            }

            return Enumerable.Empty<IBinding>();
        }
    }

}
