using System.Web;
using System.Web.Optimization;

namespace GeneX.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
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
					  "~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/bundles/morris").Include(
					  "~/Scripts/morris/morris.min.js",
					  "~/Scripts/morris/morris-data.js",
					  "~/Scripts/morris/raphael.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/flot").Include(
					  "~/Scripts/flot/excanvas.min.js",
					  "~/Scripts/flot/jquery.*",
					  "~/Scripts/flot/flot-*"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			bundles.Add(new StyleBundle("~/Content/morris_css").Include(
					  "~/Content/Plugins/morris.css"));

			bundles.Add(new StyleBundle("~/Content/sb_admin").Include(
					  "~/Content/sb-admin.css"));

			bundles.Add(new StyleBundle("~/Content/font_awesome").Include(
					  "~/Content/Font-awesome/css/font-awesome.min.css"));
		}
	}
}
