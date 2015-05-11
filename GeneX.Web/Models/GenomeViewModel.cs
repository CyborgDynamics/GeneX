using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using GeneX.Model;

namespace GeneX.Web.Models
{
	public class GenomeViewModel
	{
		public GenomeViewModel()
		{
			SNPs = new List<SNP>();
		}
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime DateTime { get; set; }
		public bool DataFileExists { get; set; }
		public List<SNP> SNPs { get; set; }
	}
}