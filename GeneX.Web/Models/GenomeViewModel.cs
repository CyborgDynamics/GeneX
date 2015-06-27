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
			SNPs = new List<SNPDTO>();
		}
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime DateTime { get; set; }
		public bool DataFileExists { get; set; }
		public List<SNPDTO> SNPs { get; set; }
	}

	public class SNPDTO
	{
		public string ClusterId { get; set; }
		public string ChromosomeType { get; set; }
		public int Chromosome { get; set; }
		public string Allele1 { get; set; }
		public string Allele2 { get; set; }
		public Guid SNPId { get; set; }
		public int Position { get; set; }
		public string Notes { get; set; }
	}
}