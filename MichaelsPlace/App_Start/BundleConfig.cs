using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace MichaelsPlace
{
    internal static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-migrate-{version}.js",
                "~/Scripts/jquery-sortable.js",
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/underscore.js",
                "~/Scripts/spin.js",
                "~/Scripts/ladda.js",
                "~/Scripts/moment.js",
                "~/Scripts/jquery.validate.js",
                "~/scripts/jquery.validate.unobtrusive.js",
                "~/scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.validate.unobtrusive.bootstrap.js",
                "~/Scripts/toastr.js",
                "~/Scripts/admin.js"
                ));

            bundles.Add(new ScriptBundle("~/js/datatables")
                            .Include("~/Scripts/DataTables/datatables.js",
                                     "~/Scripts/Shared/_index-layout.js"
                            ));

            bundles.Add(new ScriptBundle("~/js/person-reference-model")
                            .Include("~/Content/Selectize/js/standalone/selectize.js",
                                     "~/Scripts/Shared/EditorTemplates/person-reference-model.js"
                            ));

            bundles.Add(new ScriptBundle("~/modernizr").Include(
                "~/Scripts/modernizr-{version}.js"
                ));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/ladda-themeless.css",
                "~/Content/site.css",
                "~/Content/datatables.css",
                "~/Content/bootstrap-mvc-validation.css",
                "~/Content/font-awesome.css",
//                "~/Content/Selectize/css/selectize.css",
                "~/Content/Selectize/css/selectize.bootstrap3.css",
                "~/Content/toastr.css"
                ));
        }
    }
}