using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using FluentBootstrap;
using FluentBootstrap.Dropdowns;
using FluentBootstrap.Html;
using FluentBootstrap.Internals;
using FluentBootstrap.Mvc;
using FluentBootstrap.Mvc.Internals;
using FluentBootstrap.Tables;
using MichaelsPlace;
using MichaelsPlace.Extensions;
using MichaelsPlace.Utilities;

namespace BootstrapSupport
{
    public static class BootstrapHelperExtensions
    {
        public static ComponentBuilder<TConfig, TTag> AddData<TConfig, TTag>(this ComponentBuilder<TConfig, TTag> builder, string name, string data)
            where TConfig : BootstrapConfig where TTag : Tag => builder.AddAttribute("data-" + name, data);

        public static ComponentBuilder<TConfig, TTag> AddFeedback<TConfig, TTag>(this ComponentBuilder<TConfig, TTag> builder, string style = "zoom-out")
            where TConfig : BootstrapConfig where TTag : Tag
        {
            return builder.AddCss("ladda-button").AddData("style", style);
        }

        public static ComponentBuilder<TConfig, FluentBootstrap.Forms.Form> AjaxForm<TConfig, TComponent>(
            this BootstrapHelper<TConfig, TComponent> helper,
            string formAction,
            System.Web.Mvc.Ajax.AjaxOptions ajaxOptions = null
        ) where TConfig : BootstrapConfig where TComponent : Component, ICanCreate<FluentBootstrap.Forms.Form>
        {
            var id = SomeRandom.Id();

            ajaxOptions = ajaxOptions ?? new AjaxOptions()
                                         {
                                             OnSuccess = "indexViewModel.modalLoaded",
                                             OnComplete = "Ladda.bind('.ladda-button')",
                                         };

            var form = helper.Form()
                             .AddAttribute("action", formAction)
                             .AddAttribute("method", "post")
                             .AddAttributes(ajaxOptions.ToUnobtrusiveHtmlAttributes())
                             .SetId(id);

            return form;
        }

        public static ComponentBuilder<TConfig, FluentBootstrap.Links.Link> AjaxLinkButton<TConfig, TComponent>(
            this BootstrapHelper<TConfig, TComponent> helper,
            string linkText,
            string href,
            string btnStyle = "btn-primary",
            System.Web.Mvc.Ajax.AjaxOptions ajaxOptions = null
            ) where TConfig : BootstrapConfig where TComponent : Component, ICanCreate<FluentBootstrap.Links.Link>
        {
            ajaxOptions = ajaxOptions ?? new AjaxOptions()
                                         {
                                             OnSuccess = "indexViewModel.modalLoaded"
                                         };

            var link = helper.Link(linkText, href).AddCss("btn", btnStyle)
                .AddAttributes(ajaxOptions.ToUnobtrusiveHtmlAttributes());

            return link;
        }

        public static ComponentBuilder<TConfig, FluentBootstrap.Dropdowns.DropdownLink> AjaxDropdownLinkButton<TConfig, TComponent>(
            this BootstrapHelper<TConfig, TComponent> helper,
            string linkText,
            string href,
            TextState state = TextState.Primary
            ) where TConfig : BootstrapConfig where TComponent : Component, ICanCreate<DropdownLink>
        {
            var ajaxOptions = new AjaxOptions()
                              {
                                  OnSuccess = "indexViewModel.modalLoaded"
                              };

            var attributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            var link = helper.DropdownLink(linkText, href).AddAttributes(attributes);
            
            return link;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FluentBootstrap.Tables.Table> AjaxDataTable<TModel>(
            this MvcBootstrapHelper<TModel> 
            helper, UrlHelper url)

        {
            //helper.RenderPartial("Templates/_IndexEditModal");
            //helper.RenderPartial("Templates/_IndexItemButtons");

            helper.GetConfig().GetHtmlHelper().ViewContext.Writer.Write("<div id=\"ajax-modal\"></div>");

            var table = helper.Table().SetId("index-data-table")
                // ReSharper disable Mvc.ActionNotResolved
                                 .AddData("ajax-url", url.Action("JsonIndex"))
                                 .AddData("details-url", url.Action("Details"))
                                 .AddData("edit-url", url.Action("Edit"))
                                 .AddData("delete-url", url.Action("Delete"));

                // ReSharper restore Mvc.ActionNotResolved

            helper.GetConfig().GetHtmlHelper().AddScriptBundle("~/js/datatables");
            
            return table;
        }

        public static string JsonNameFor<TModel, TValue>(this HtmlHelper<TModel> @this, Expression<Func<TModel, TValue>> expression) => @this.IdFor(expression).ToString().Split('_').Last().ToCamelCase();
        public static string JsonNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> @this, Expression<Func<TModel, TValue>> expression) => ExpressionHelper.GetExpressionText(expression).Split('_').Last().ToCamelCase();
    }
}
