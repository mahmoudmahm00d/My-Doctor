using System.Web.Optimization;

namespace FinalProject
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/DataTables/jquery.dataTables.js",
                      "~/Scripts/DataTables/dataTables.bootstrap.js",
                      "~/Scripts/sweetalert2.min.js",
                      "~/Scripts/typeahead.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/loginStyle").Include("~/Content/login.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/management.css",
                      "~/Content/shared.css",
                      "~/Content/datatables/css/dataTables.bootstap.css",
                      "~/Content/sweetalert2.min.css",
                      "~/Content/typeahead.css"));
        }
    }
}
