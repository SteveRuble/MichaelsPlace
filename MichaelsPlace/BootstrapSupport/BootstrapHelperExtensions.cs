using System;
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
            ajaxOptions = ajaxOptions ?? new AjaxOptions();
            var attributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            var link = helper.Link(linkText, href).AddCss("btn", "ladda-button", btnStyle).AddData("style", "zoom-out");

            foreach (var attribute in attributes)
            {
                link = link.AddAttribute(attribute.Key, attribute.Value);
            }

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

        public static IDisposable BeginModal<TModel>(this HtmlHelper<TModel> @this, string title)
        {
            @this.ViewContext.Writer.Write(
                $@"<div class='modal fade' tabindex='-1' role='dialog'>
    <div class='modal-dialog'>
        <div class='modal-content'>
            <div class='modal-header'>
                <button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
                <h4 class='modal-title'>{title}</h4>
            </div>
            <div class='modal-body' id='edit-modal-container'>");



            return Disposable.Create(() =>
            {
                @this.ViewContext.Writer.Write(@"
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div><!-- /.modal -->");
            });
        }



    }
}