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
	public class GenomeController : Controller
	{
		[HttpGet, Authorize]
		public ActionResult Index(Guid? id)
		{
			GenomeViewModel gvm = new GenomeViewModel();

			if (id != null)
			{
				using (GeneXContext context = new GeneXContext())
				{
					Genome g = context.Genome.Where(m => m.GenomeId == id.Value).FirstOrDefault();
					gvm.Id = g.GenomeId;
					gvm.Name = g.Name;
					gvm.SNPs = context.SNP.Where(m => m.GenomeId == id.Value).Take(100).ToList();
				}
				FileInfo fi = new FileInfo(Path.Combine(Server.MapPath("~/App_Data/uploads"), id.ToString() + ".txt"));
				if (fi.Exists)
				{
					gvm.DataFileExists = true;
					gvm.DateTime = fi.LastWriteTime;
				}
			}

			return View(gvm);
		}

		[HttpPost]
		public ActionResult Index(Guid? id, HttpPostedFileBase file)
		{
			if (file.ContentLength > 0)
			{
				string fileName = Path.GetFileName(file.FileName);
				string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), id.ToString() + ".txt");
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
			}
			return RedirectToAction("Index", "Home");
		}
	}
}