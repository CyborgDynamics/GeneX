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
	public class GenomePermission
	{
		[Key]
		public Guid GenomePermissionId { get; set; }

		public Guid OrganizationId { get; set; }

		[ForeignKey("GenomeId")]
		public Genome Genome { get; set; }
		public Guid GenomeId { get; set; }
		
		public Guid PermissionId { get; set; }

		public Guid UserId { get; set; }
	}
}