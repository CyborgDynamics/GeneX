using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class UserOrganizationRole
	{
		[Key]
		public Guid UserOrganizationRoleId { get; set; }

		[Index("IX_OrganizationRoleAndUser", IsUnique = true, Order = 1)]
		public Guid OrganizationRoleId { get; set; }
		[ForeignKey("OrganizationRoleId")]
		public virtual OrganizationRole OrganizationRole { get; set; }

		[Index("IX_OrganizationRoleAndUser", IsUnique = true, Order = 0)]
		public Guid UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }
	}
}
