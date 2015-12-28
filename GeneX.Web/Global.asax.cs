using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GeneX.Security;
using GeneX.Model;

namespace GeneX.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			//GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());

			AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<GeneXContext, GeneX.Model.Migrations.Configuration>());
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<Db, GeneX.Security.Migrations.Configuration>());

			GeneXContext context = new GeneXContext();
			context.Database.Initialize(false);
			context.Database.CreateIfNotExists();
			Db securityContext = new Db();
			securityContext.Database.Initialize(false);
			securityContext.Database.CreateIfNotExists();
		}
	}
}
