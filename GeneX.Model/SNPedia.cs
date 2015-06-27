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
	[Table("SNPedia")]
	public class SNPedia
	{
		[Key]
		public Guid SNPediaId { get; set; }
		[MaxLength(255), Required, Column(TypeName = "varchar")]
		public string ClusterId { get; set; }
		[MaxLength(10), Required, Column(TypeName = "varchar")]
		public string Source { get; set; }
		[MaxLength(8), Required, Column(TypeName = "varchar")]
		public string ChromosomeType { get; set; }
		public int Chromosome { get; set; }
		[MaxLength(10), Required, Column(TypeName = "varchar")]
		public string ChipType { get; set; }
		public int Position { get; set; }
		[MaxLength(125), Column(TypeName = "varchar")]
		public string Notes { get; set; }
	}
}