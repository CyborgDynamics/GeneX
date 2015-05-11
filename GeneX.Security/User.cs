using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace GeneX.Security
{
	public class User : IdentityUser<Guid, UserLogin, UserRole, UserClaim>, ISecurityEntity
	{
		public Guid? ActiveOrganizationId { get; set; }

		[ForeignKey("ActiveOrganizationId")]
		public virtual Organization ActiveOrganization { get; set; }

		[MaxLength(50)]
		public string FirstName { get; set; }

		[MaxLength(50)]
		public string LastName { get; set; }

		[NotMapped]
		public List<Organization> Organizations { get; set; }
		public bool IsDeleted { get; set; }

		public Guid? CreatedByUserId { get; set; }
		public User CreatedBy { get; set; }

		public Guid? UpdatedByUserId { get; set; }
		public User UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }
		public DateTime CreatedDate { get; set; }

		[NotMapped]
		public string FullName
		{
			get
			{
				if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
				{
					return UserName;
				}

				return string.Format("{0} {1}", FirstName, LastName).Trim();
			}
		}

		public string GetOrganizationConnectionString(UserManager manager)
		{
			return manager.GetOrganizationConnectionString(this.Id);
		}

		public User()
		{
			Organizations = new List<Organization>();

			CreatedDate = DateTime.UtcNow;
		}

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager manager, string authenticationType)
		{
			var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			return userIdentity;
		}

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager manager)
		{
			ClaimsIdentity identity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			return identity;
		}

		[NotMapped]
		public bool IsSuperAdmin
		{
			get
			{
				foreach (UserRole ur in this.Roles)
				{
					if (ur.RoleId == Constants.Roles.Ids.SuperAdministrator)
					{
						return true;
					}
				}
				return false;
			}
		}
	}    
}