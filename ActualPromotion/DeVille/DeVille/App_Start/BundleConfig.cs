using System.Web.Optimization;

namespace DeVille
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/jquery-ui").Include(
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/common").Include(
                        "~/Scripts/common.js"));

            bundles.Add(new ScriptBundle("~/scripts/admin-common").Include(
                        "~/Scripts/layouts/admin/bootstrap.min.js",     /*Bootstrap Core JavaScript*/
                        "~/Scripts/layouts/admin/metisMenu.min.js",     /*Metis Menu Plugin JavaScript*/
                        "~/Scripts/layouts/admin/raphael-min.js.js",    /*Morris Charts JavaScript*/
                        "~/Scripts/layouts/admin/morris.min.js",        /*Morris Charts JavaScript*/
                        "~/Scripts/layouts/admin/sb-admin-2.js"));      /*Custom Theme JavaScript*/

            bundles.Add(new ScriptBundle("~/scripts/prettyPhoto").Include(
                        "~/Scripts/jquery.prettyPhoto.js"));

            bundles.Add(new StyleBundle("~/content/css").Include(
                      "~/content/reset.css",
                      "~/content/layouts/pure-min.css",
                      "~/content/styles.css"));

            bundles.Add(new StyleBundle("~/content/admin-layout-styles").Include(
                        "~/content/layouts/admin/metisMenu.min.css",        /*MetisMenu CSS*/
                        "~/content/layouts/admin/timeline.css",             /*Timeline CSS*/
                        "~/content/layouts/admin/sb-admin-2.css",           /*Custom CSS*/
                        "~/content/layouts/admin/morris.css").Include(            /*Morris Charts CSS*/
                        "~/content/layouts/admin/font-awesome.min.css", new CssRewriteUrlTransform()).
                        Include("~/content/layouts/admin/bootstrap.min.css", new CssRewriteUrlTransform()));   /*Bootstrap Core CSS*/

            bundles.Add(new StyleBundle("~/content/prettyPhoto").Include(
                      "~/content/prettyPhoto.css"));
        }
    }
}