using System.Web;
using System.Web.Optimization;

namespace Jobbdagar
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

        }
    }
}