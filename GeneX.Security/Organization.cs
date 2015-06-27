using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneX.Security
{
	public class Organization : IdentityOrganization<Guid> { }

	public class IdentityOrganization<TKey> : ISecurityEntity, IOrganization<TKey>
	{
		[Key]
		public TKey OrganizationId { get; set; }

		[MaxLength(50)]
		[Column(TypeName = "VARCHAR")]
		public string Name { get; set; }

		[Column(TypeName = "VARCHAR")]
		[MaxLength(4000)]
		public string Description { get; set; }

		public bool IsDeleted { get; set; }

		public Guid? CreatedByUserId { get; set; }

		[ForeignKey("CreatedByUserId")]
		public User CreatedBy { get; set; }

		public Guid? UpdatedByUserId { get; set; }

		[ForeignKey("UpdatedByUserId")]
		public User UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }
		public DateTime CreatedDate { get; set; }

		[MaxLength(50)]
		[Column(TypeName = "VARCHAR")]
		public string DatabaseName { get; set; }
		
		[MaxLength(50)]
		[Column(TypeName = "VARCHAR")]
		public string Website { get; set; }

		public IdentityOrganization()
		{
			CreatedDate = DateTime.UtcNow;
		}
	}
}