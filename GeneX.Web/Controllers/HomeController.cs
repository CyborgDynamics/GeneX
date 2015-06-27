using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GeneX.Model;
using GeneX.Web.Models;

namespace GeneX.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			IndexViewModel ivm = new IndexViewModel();
			using (GeneXContext cs = new GeneXContext())
			{
				string id = ((System.Security.Claims.ClaimsPrincipal)this.User).Claims.Where(m => m.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
				List<Guid> genomeIds = (from gp in cs.GenomePermission where gp.UserId == new Guid(id) && gp.PermissionId == Model.Constants.Permissions.Ids.Owner select gp.GenomeId).ToList();
				List<Genome> results = (from g in cs.Genome where genomeIds.Contains(g.GenomeId) select g).ToList();
				foreach (Genome g in results )
				{
					ivm.Genomes.Add(g.GenomeId, g.Name);
				}
			}
			return View(ivm);
		}

		[AllowAnonymous, HttpGet]
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";
			return View();
		}

		[AllowAnonymous, HttpGet]
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";
			return View();
		}
	}
}