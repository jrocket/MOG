using System.Web;
using System.Web.Optimization;

namespace MoG
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                      , "~/Scripts/modern-business.js"
                      , "~/Scripts/bootstrap-tagsinput.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"
                      , "~/content/modern-business.css"
                      , "~/Content/font-awesome/font-awesome.css"
                      , "~/Content/tags/bootstrap-tagsinput.css"));
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
    "~/Scripts/knockout-3.0.0.js"
    ));

        }
    }
}
