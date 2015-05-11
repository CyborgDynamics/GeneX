using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Model
{
	[Table("SNP")]
	public class SNP
	{
		[Key]
		public Guid SNPId { get; set; }
		public Guid GenomeId { get; set; }
		[MaxLength(255), Required, Column(TypeName = "varchar")]
		public string ClusterId { get; set; } // RSID
		[MaxLength(8), Required, Column(TypeName = "varchar")]
		public string ChromosomeType { get; set; }
		public int Chromosome { get; set; }
		public int Position { get; set; }
		[MaxLength(4), Column(TypeName = "varchar")]
		public string Genotype { get; set; } //NVARCHAR 2
		[MaxLength(1), Column(TypeName = "varchar")]
		public string Allele1 { get; set; }
		[MaxLength(1), Column(TypeName = "varchar")]
		public string Allele2 { get; set; }
	}
}