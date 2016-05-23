using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Extensions.ChildKernel;
using Ninject.Syntax;
using Ninject.Web.WebApi;
using Ninject.Web.WebApi.Filter;
using Owin;
using Swashbuckle.Application;

namespace MichaelsPlace
{
    public partial class Startup
    {
        private void ConfigureWebApi(IAppBuilder app, IResolutionRoot resolutionRoot)
        {
            var config = new HttpConfiguration();

            var kernel = new ChildKernel(resolutionRoot);

            kernel.Rebind<HttpConfiguration>().ToConstant(config);
            kernel.Bind<DefaultModelValidatorProviders>().ToConstant(new DefaultModelValidatorProviders(config.Services.GetServices(typeof(ModelValidatorProvider)).Cast<ModelValidatorProvider>()));
            kernel.Bind<DefaultFilterProviders>().ToConstant(new DefaultFilterProviders(config.Services.GetServices(typeof(IFilterProvider)).Cast<IFilterProvider>()));

            var dependencyResolver = new NinjectDependencyResolver(kernel);

            config.DependencyResolver = dependencyResolver;

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.EnableCors(new EnableCorsAttribute("http://localhost:9000", "*", "*"));

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("Default", "{controller}/{id}");

            config.EnableSwagger(c =>
            {
                c.RootUrl(h => string.Format("{0}://{1}:{2}/api", h.RequestUri.Scheme, h.RequestUri.Host,  h.RequestUri.Port));
                c.SingleApiVersion("v1", "Michael's Place SPA API.");
            })
                  .EnableSwaggerUi(c => c.EnableDiscoveryUrlSelector() );

            app.Map("/api", webApi =>
            {
                webApi.UseWebApi(config);
            });
        }

    }
}
