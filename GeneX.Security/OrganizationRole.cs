using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class OrganizationRole : ISecurityEntity
	{
		[Key]
		public Guid OrganizationRoleId { get; set; }

		public Guid OrganizationId { get; set; }

		[ForeignKey("OrganizationId")]
		public Organization Organization { get; set; }

		public Guid RoleId { get; set; }

		[ForeignKey("RoleId")]
		public Role Role { get; set; }

		[MaxLength(100)]
		[Required]
		public string Name { get; set; }

		public bool IsDeleted { get; set; }

		public Guid? CreatedByUserId { get; set; }

		[ForeignKey("CreatedByUserId")]
		public User CreatedBy { get; set; }

		public Guid? UpdatedByUserId { get; set; }

		[ForeignKey("UpdatedByUserId")]
		public User UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }
		public DateTime CreatedDate { get; set; }

		public OrganizationRole()
		{
			CreatedDate = DateTime.UtcNow;
		}
	}
}
