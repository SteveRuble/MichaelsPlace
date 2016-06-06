﻿using System.Web;
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
                "~/Scripts/bootstrap.js",
                "~/Scripts/sb-admin-2.js",
                "~/Scripts/jquery.validate.js",
                "~/scripts/jquery.validate.unobtrusive.js",
                "~/scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/modernizr").Include(
                "~/Scripts/modernizr-{version}.js"
                ));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/bootstrap-mvc-validation.css",
                "~/Content/font-awesome.css"
                ));
        }
    }
}