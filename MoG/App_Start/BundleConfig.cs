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

            
            bundles.Add(new StyleBundle("~/Content/jQuery.FileUpload/jQueryFileupload").Include(
                          "~/Content/jQuery.FileUpload/jquery.fileupload.css",
                          "~/Content/jQuery.FileUpload/jquery.fileupload-ui.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jQuery.FileUpload/jQueryFileupload").Include(
  "~/Scripts/jQuery.Fileupload/jquery.ui.widget.js",
  "~/Scripts/jQuery.Fileupload/load-image.min.js",
  "~/Scripts/jQuery.Fileupload/canvas-to-blob.min.js",
  "~/Scripts/jQuery.Fileupload/jquery.iframe-transport.js",
  "~/Scripts/jQuery.Fileupload/jquery.fileupload.js",
  "~/Scripts/jQuery.Fileupload/jquery.fileupload-process.js",
  "~/Scripts/jQuery.Fileupload/jquery.fileupload-image.js",
  "~/Scripts/jQuery.Fileupload/jquery.fileupload-audio.js",
  "~/Scripts/jQuery.Fileupload/jquery.fileupload-video.js",
  "~/Scripts/jQuery.Fileupload/jquery.fileupload-validate.js",
  //"~/Scripts/jQuery.Fileupload/jquery.blueimp-gallery.min.js",
  "~/Scripts/jQuery.Fileupload/tmpl.min.js",
  "~/Scripts/jQuery.Fileupload/jquery.fileupload-ui.js"
            ));


            //SoundManager
            bundles.Add(new StyleBundle("~/Content/SndManager").Include(
                      "~/Content/SoundManager/css/demo.css",
                      "~/Content/SoundManager/css/page-player.css",
                      "~/Content/SoundManager/css/optional-annotations.css",
                        "~/Content/SoundManager/css/optional-themes.css"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/soundmanager").Include(
                        "~/Content/SoundManager/script/soundmanager2.js",
                          "~/Content/SoundManager/script/page-player.js",
                           "~/Content/SoundManager/script/optional-page-player-metadata.js"
                        ));


        }
    }
}
