using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using FluentBootstrap;
using FluentBootstrap.Mvc;
using FluentBootstrap.Tables;
using MichaelsPlace;
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
            ajaxOptions = ajaxOptions ?? new AjaxOptions();
            var attributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            var link = helper.Link(linkText, href).AddCss("btn", "ladda-button", btnStyle).AddData("style", "zoom-out");

            foreach (var attribute in attributes)
            {
                link = link.AddAttribute(attribute.Key, attribute.Value);
            }

            return link;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Table> DataTable<TModel>(this HtmlHelper<TModel> helper, UrlHelper url)
        {
            var bootstrap = helper.Bootstrap();

            var table = bootstrap.Table().SetId("index-data-table")
                // ReSharper disable Mvc.ActionNotResolved
                                 .AddData("ajax-url", url.Action("JsonIndex"))
                                 .AddData("details-url", url.Action("Details"))
                                 .AddData("edit-url", url.Action("Edit"))
                                 .AddData("delete-url", url.Action("Delete"));

                // ReSharper restore Mvc.ActionNotResolved

            helper.AddScriptBundle("~/js/datatables");

            return table;
        }

    }
}