using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GeneX.Security
{
	public interface ISecurityEntity
	{
		bool IsDeleted { get; set; }

		Guid? CreatedByUserId { get; set; }

		[ForeignKey("Id")]
		User CreatedBy { get; set; }

		Guid? UpdatedByUserId { get; set; }

		[ForeignKey("Id")]
		User UpdatedBy { get; set; }

		DateTime? UpdatedDate { get; set; }
		DateTime CreatedDate { get; set; }
	}
}