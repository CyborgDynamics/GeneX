using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeneX.Model;
using GeneX.Web.Models;

namespace GeneX.Web.Controllers
{
	[Authorize]
	public class GenomeController : Controller
	{
		private string ServerMapPath
		{
			get
			{
				if (Server != null)
				{
					return Server.MapPath("~/App_Data/uploads");
				}
				else
				{
					return string.Empty;
				}
			}
		}

		[HttpGet]
		public ActionResult Index(Guid? id)
		{
			GenomeViewModel gvm = new GenomeViewModel();

			if (id != null)
			{
				using (GeneXContext context = new GeneXContext())
				{
					Genome g = context.Genome.Where(m => m.GenomeId == id.Value).FirstOrDefault();
					if (g != null)
					{
						gvm.Id = g.GenomeId;
						gvm.Name = g.Name;
					}
				}

				FileInfo fi = new FileInfo(Path.Combine(ServerMapPath, id.ToString() + ".txt"));
				if (fi.Exists)
				{
					gvm.DataFileExists = true;
					gvm.DateTime = fi.LastWriteTime;
				}
			}

			return View(gvm);
		}

		[HttpGet]
		//[Authorize(Roles = "ReadGenome")]
		public JsonResult PartialIndex(Guid? id, int page, int pageSize)
		{
			JsonResult jr = new JsonResult();
			pageSize = pageSize == 100 ? 101 : pageSize;
			using (GeneXContext context = new GeneXContext())
			{
				IQueryable<SNPDTO> query = context.SNP
						.Where(m => m.GenomeId == id.Value)
						.Join(context.SNPedia,
						c => c.ClusterId,
						d => d.ClusterId,
						(c, d) => new SNPDTO()
						{
							ClusterId = c.ClusterId,
							ChromosomeType = c.ChromosomeType,
							Chromosome = c.Chromosome,
							Allele1 = c.Allele1,
							Allele2 = c.Allele2,
							SNPId = c.SNPId,
							Position = c.Position,
							Notes = d.Notes
						}).Where(m => !(m.Notes == null || m.Notes.Trim() == string.Empty))
						.OrderBy(m => m.Chromosome)
						.ThenBy(m => m.Position)
									.Skip(page * pageSize)
									.Take(pageSize);


				jr.Data = query.ToList();
			}
			jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			return jr;
		}
		

		[HttpPost]
		// TODO: Fix the Calls to include Either FormValues Dictionary or DTO.
		public ActionResult Index(Guid? id, HttpPostedFileBase file)
		{
			if (file == null || file.ContentLength == 0 || file.ContentType == null || string.IsNullOrEmpty(file.FileName) || file.InputStream == null)
			{
				this.ModelState.AddModelError("PostedFile", "Must attach file.");
				return View();
			}

			if (file.ContentLength > 0)
			{
				string fileName = Path.GetFileName(file.FileName);
				string path = Path.Combine(ServerMapPath, id.ToString() + ".txt");
				file.SaveAs(path);

				List<SNP> snps = new List<SNP>();
				int i = 0;
				using (StreamReader sr = new StreamReader(path))
				{
					GeneXContext context = new GeneXContext();

					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();
						if (line.StartsWith("#", true, CultureInfo.InvariantCulture))
						{
							continue;
						}
						string[] parsed = line.Split("\t".ToCharArray());

						string tmp = parsed[0];
						SNP snp = context.SNP.Where(m => m.ClusterId == tmp && m.GenomeId == id.Value).FirstOrDefault();

						int chromosome = 0;
						string chromosomeType;
						if (parsed[1].ToUpper().Equals("X") || parsed[1].ToUpper().Equals("Y"))
						{
							chromosome = 23;
							chromosomeType = parsed[1].ToUpper();
						}
						else if (parsed[1].ToUpper().Equals("MT"))
						{
							chromosome = 0;
							chromosomeType = "MT";
						}
						else
						{
							chromosome = int.Parse(parsed[1]);
							chromosomeType = "A";
						}

						char? allele1 = null;
						char? allele2 = null;
						if (parsed[3].Length > 0)
						{
							allele1 = parsed[3][0];
						}
						if (parsed[3].Length > 1)
						{
							allele2 = parsed[3][0];
						}
						if (snp == null)
						{
							context.SNP.Add(new SNP { SNPId = Guid.NewGuid(), GenomeId = id.Value, ClusterId = parsed[0], Chromosome = chromosome, ChromosomeType = chromosomeType, Position = int.Parse(parsed[2]), Genotype = parsed[3], Allele1 = allele1.ToString(), Allele2 = allele2.ToString() });
						}
						else
						{
							snp.ChromosomeType = chromosomeType;
							snp.Genotype = parsed[3];
						}

						++i;
						if (i % 1000 == 0)
						{
							context.SaveChanges();
							context.Dispose();
							context = new GeneXContext();
						}
					}
					context.SaveChanges();
					context.Dispose();
				}

			}

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Add(string Name)
		{
			using (GeneXContext context = new GeneXContext())
			{
				Genome g = new Genome();
				g.GenomeId = Guid.NewGuid();
				g.Name = Name;
				context.Genome.Add(g);
				context.SaveChanges();

				GenomePermission gp = new GenomePermission();
				gp.GenomeId = g.GenomeId;
				gp.GenomePermissionId = Guid.NewGuid();
				//gp.OrganizationId = User.Identity.
			}
			return RedirectToAction("Index", "Home");
		}
	}
}