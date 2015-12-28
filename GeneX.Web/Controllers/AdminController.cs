using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GeneX.Model;
using System.Globalization;

namespace GeneX.Web.Controllers
{
	public class AdminController : Controller
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
		// GET: Admin
		[Authorize(Roles ="Any")]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
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

				List<SNPedia> snps = new List<SNPedia>();
				int i = 0;
				using (StreamReader sr = new StreamReader(path))
				{
					GeneXContext context = new GeneXContext();

					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();
						if (line.StartsWith("#", true, CultureInfo.InvariantCulture) || string.IsNullOrWhiteSpace(line))
						{
							continue;
						}
						string[] parsed = line.Split("\t".ToCharArray());

						string chrome = parsed[0];
						string reference = parsed[1];
						string chiptype = parsed[2];
						int pos = int.Parse(parsed[3]);
						string toParse = parsed[8];
						int chromosome = -1;
						string chromosomeType;


						string notes = toParse.Substring(toParse.IndexOf("Note=")+5).Replace("\"", string.Empty);
						int cidStart = toParse.IndexOf("ID=") + 6;
						int cidEnd = toParse.IndexOf(";",cidStart);
						string cid = toParse.Substring(cidStart, cidEnd - cidStart);
						
						if ( chrome.Contains("MT"))
						{
							chromosomeType = "MT";
							chromosome = 0;
						}
						else if ( chrome.Contains("X"))
						{
							chromosomeType = "X";
							chromosome = 23;
						}
						else if (chrome.Contains("Y"))
						{
							chromosomeType = "Y";
							chromosome = 23;
						}
						else
						{
							chromosomeType = "A";

							if (!int.TryParse(chrome.Substring(3), out chromosome))
							{
								// Not Human
								continue;
							}
						}

						SNPedia snpedia = context.SNPedia.Where(m => m.Position == pos && m.Chromosome == chromosome && m.ChromosomeType == chromosomeType).FirstOrDefault();
						if (snpedia == null)
						{
							context.SNPedia.Add(new SNPedia { SNPediaId = Guid.NewGuid(), ClusterId = cid, Chromosome = chromosome, ChromosomeType = chromosomeType, Position = pos, ChipType = chiptype, Source = reference, Notes = notes });
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
	}
}