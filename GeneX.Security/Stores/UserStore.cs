using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.SqlClient;

namespace GeneX.Security
{
	public class UserStore : UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>
	{
		private readonly string OrganizationRoleSQL = @"
			SELECT OrganizationRoleItemId 
			FROM Security.UserOrganizationRole   AS SUOR
			INNER JOIN  Security.OrganizationRole AS  SOR ON SUOR.OrganizationRoleId = SOR.OrganizationRoleId
			INNER JOIN  Security.OrganizationRoleItem AS  SORI ON SOR.OrganizationRoleId = SORI.OrganizationRoleId
			INNER JOIN  Security.Role AS R ON SORI.RoleId = R.Id
			WHERE SOR.OrganizationId = @OrgId
			AND R.RoleId = @RoleName
			AND SUOR.UserId = @UserId";

		private readonly string OrganizationListRolesSQL = @"
			SELECT R.Name
				FROM		Security.UserOrganizationRole	AS	SUOR 
				INNER JOIN	Security.OrganizationRole		AS	SOR		ON SUOR.OrganizationRoleId = SOR.OrganizationRoleId
				INNER JOIN	Security.OrganizationRoleItem	AS	SORI	ON SOR.OrganizationRoleId = SORI.OrganizationRoleId
				INNER JOIN Security.Role AS R ON R.Id = RoleId
				WHERE	SOR.OrganizationId = @OrgId 
				AND		SUOR.UserId = @UserId
			";

		public UserStore(Db context)
			: base(context)
		{
		}

		public async Task<IList<Organization>> GetOrganizationsAsync(User user)
		{
			List<Organization> toRet = new List<Organization>();
			if (user == null || user.Id == null || user.Id == Guid.Empty)
			{
				return null;
			}
			var compositeUserRoles = (this.Context as Db).UserOrganizationRole.Where(m => m.User.Id == user.Id)
				.Include(m => m.OrganizationRole.Organization).ToList();
			foreach (UserOrganizationRole role in compositeUserRoles)
			{
				toRet.Add(role.OrganizationRole.Organization);
			}
			return await Task.FromResult(toRet.Distinct().ToList());
		}

		public string GetOrganizationConnectionString(Guid OrganizationId)
		{
			return (this.Context as Db).Organization.Where(m => m.OrganizationId == OrganizationId).Select(m=>m.DatabaseName).FirstOrDefault();
		}

		public override Task UpdateAsync(User user)
		{
			return base.UpdateAsync(user);
		}

		public override async Task<bool> IsInRoleAsync(User user, string roleName)
		{
			bool baseRole = await base.IsInRoleAsync(user, roleName);
			bool orgRole = await IsInOrganizationRoleAsync(user, roleName);
			return (baseRole || orgRole);
		}

		private async Task<bool> IsInOrganizationRoleAsync(User user, string rolename)
		{
			SqlParameter OrgId = new SqlParameter("OrgId", user.ActiveOrganizationId);
			SqlParameter UserId = new SqlParameter("UserId", user.Id);
			SqlParameter RoleName = new SqlParameter("RoleName", rolename);
			List<Guid> result = await (this.Context as Db).Database.SqlQuery<Guid>(OrganizationRoleSQL, OrgId, UserId, RoleName).ToListAsync();
			bool exists = result.FirstOrDefault() == null ? false : true;
			return exists;
		}

		

		public async Task<IList<string>> GetOrganizationRolesAsync(User user)
		{
			SqlParameter OrgId = new SqlParameter("OrgId", user.ActiveOrganizationId);
			SqlParameter UserId = new SqlParameter("UserId", user.Id);
			return await (this.Context as Db).Database.SqlQuery<string>(OrganizationListRolesSQL, OrgId, UserId).ToListAsync();
		}
	}
}