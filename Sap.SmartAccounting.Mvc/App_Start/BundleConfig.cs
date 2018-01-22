using System.Web;
using System.Web.Optimization;

namespace Sap.SmartAccounting.Mvc
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                "~/Scripts/bootstrap-datepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/Scripts/select2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminlte").Include(
                "~/admin-lte/js/adminlte.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/bootstrap-theme.min.css",
                "~/Content/font-awesome.min.css",
                "~/admin-lte/css/AdminLTE.min.css",
                "~/admin-lte/css/skins/_all-skins.min.css",
                "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                "~/Content/bootstrap-datepicker3.min.css"));

            bundles.Add(new StyleBundle("~/Content/select2").Include(
                "~/Content/select2.min.css"));
        }
    }
}
