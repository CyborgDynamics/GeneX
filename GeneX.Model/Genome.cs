using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Model
{
	public class Genome
	{
		[Key]
		public Guid GenomeId { get; set; }
		public string Name { get; set; }
	}
}