using System.Web;
using System.Web.Optimization;

namespace CommunityHealth
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/myScripts").Include(
                        "~/Scripts/CustomJS.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.11.4.min.js",
                       "~/Scripts/DatePickerReady.js",
                       "~/Scripts/CustomJS.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                    "~/Scripts/jquery.unobtrusive*"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                       "~/Scripts/CustomJS.js"));
            //custom bundle for the reports page
            bundles.Add(new ScriptBundle("~/bundles/reports").Include(
                "~/Scripts/Reports.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/base/datepicker.css",
                      "~/Content/themes/base/core.css",
                      "~/Content/themes/base/theme.css",
                      "~/Content/Site.css"));
        }
    }
}
