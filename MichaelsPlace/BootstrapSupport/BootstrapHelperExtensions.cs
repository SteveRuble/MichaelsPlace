using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using System.Web.WebPages;
using FluentBootstrap;

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
    }
}