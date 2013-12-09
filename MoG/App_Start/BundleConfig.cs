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
                      , "~/Scripts/TagInput/bootstrap-tagsinput.js"
                      , "~/Scripts/TagInput/typeahead.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"
                      , "~/content/modern-business.css"
                      , "~/Content/font-awesome/font-awesome.css"
                      , "~/Content/TagInput/bootstrap-tagsinput.css"
                       , "~/Content/TagInput/bootstrap-tagsinput-custom.css"));
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
    "~/Scripts/knockout-3.0.0.js"
    ));
#if DEBUG
            bundles.Add(new ScriptBundle("~/bundles/debug").Include(
    "~/Scripts/MOG/DEBUGUser.js"
    ));

#endif 
        }
    }
}
