using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class OrganizationRoleItem : ISecurityEntity
	{
		[Key]
		public Guid OrganizationRoleItemId { get; set; }

		public Guid OrganizationRoleId { get; set; }

		[ForeignKey("OrganizationRoleId")]
		public OrganizationRole OrganizationRole { get; set; }

		public Guid RoleId { get; set; }

		[ForeignKey("RoleId")]
		public Role Role { get; set; }
		public OrganizationRoleItem()
		{
		}

		public bool IsDeleted
		{
			get;
			set; 
		}

		public Guid? CreatedByUserId
		{
			get;
			set;
		}

		public User CreatedBy
		{
			get;
			set;
		}

		public Guid? UpdatedByUserId
		{
			get;
			set;
		}

		public User UpdatedBy
		{
			get;
			set;
		}

		public DateTime? UpdatedDate
		{
			get;
			set;
		}

		public DateTime CreatedDate
		{
			get;
			set;
		}
	}
}