using System.Web.Optimization;

namespace WebSanaAssessment
{
   public class BundleConfig
   {
      // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
      public static void RegisterBundles(BundleCollection bundles)
      {
         #region Scripts
         bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

         bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                     "~/Scripts/jquery.validate*"));

         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
         bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                     "~/Scripts/modernizr-*"));

         bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   "~/Scripts/popper.min.js",
                   "~/Scripts/bootstrap/bootstrap.min.js"));

         bundles.Add(new ScriptBundle("~/bundles/site").Include(
                   "~/Scripts/functions.js"));

         //Additionals
         bundles.Add(new ScriptBundle("~/bundles/dataTable").Include(
                   "~/Scripts/moment.min.js",
                   "~/Scripts/dataTable/jquery.dataTables.min.js",
                   "~/Scripts/dataTable/dataTables.bootstrap4.min.js",
                   "~/Scripts/dataTable/dataTables.responsive.min.js",
                   "~/Scripts/dataTable/responsive.bootstrap4.min.js",
                   "~/Scripts/pnotify.custom.min.js",
                   "~/Scripts/chosen.jquery.min.js"));
         #endregion

         //Styles
         #region Styles
         bundles.Add(new StyleBundle("~/Content/styles").Include(
                      "~/Content/css/bootstrap/bootstrap.css",
                      "~/Content/css/all.css",
                      "~/Content/css/pnotify.custom.min.css",
                      "~/Content/css/dataTable/dataTables.bootstrap4.min.css",
                      "~/Content/css/dataTable/responsive.bootstrap4.min.css",
                      "~/Content/css/chosen.css",
                      "~/Content/css/site.css")); 
         #endregion
      }
   }
}