using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class UserStore : UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>
	{
		private readonly string OrganizationRoleSQL = @"
			SELECT OrganizationRoleItemId 
			FROM Security.UserOrganizationRole   AS SUOR
			INNER JOIN  Security.OrganizationRole AS  SOR ON SUOR.OrganizationRoleId = SOR.OrganizationRoleId
			INNER JOIN  Security.OrganizationRoleItem AS  SORI ON SOR.OrganizationRoleId = SORI.OrganizationRoleId
			WHERE SOR.OrganizationId = @OrgId
			AND SORI.RoleId = @RoleId
			AND SUOR.UserId = @UserId";

		public UserStore(Db context)
			: base(context)
		{
		}

		//protected override async Task<User> GetUserAggregateAsync(System.Linq.Expressions.Expression<Func<User, bool>> filter)
		//{
		//	User user = await Users.Include(u => u.Roles)
		//		.Include(u=>u.Logins)
		//		.Include(u=>u.Claims)
		//		.FirstOrDefaultAsync(filter);

		//	if (user == null)
		//	{
		//		return null;
		//	}

		//	IList<Organization> myOrganizations = await GetOrganizationsAsync(user) as IList<Organization>;
		//	user.Organizations.AddRange(myOrganizations);
		//	var compositeUserRoles = ((Db)this.Context).UserOrganizationRole.Include(m=>m.OrganizationRole).Where(m => m.User.Id == user.Id && m.OrganizationRole.OrganizationId == user.ActiveOrganizationId);
		//	foreach (UserOrganizationRole cru in compositeUserRoles)
		//	{
		//		UserRole iur = new UserRole();
		//		iur.RoleId = cru.OrganizationRoleId;
		//		iur.UserId = user.Id;
		//		if (!user.Roles.Contains(iur))
		//		{
		//			user.Roles.Add(iur);
		//		}
		//	}

		//	return user;
		//}

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
			List<Guid> result = await (this.Context as Db).Database.SqlQuery<Guid>(OrganizationRoleSQL, user.Id, user.ActiveOrganizationId, rolename).ToListAsync();
			bool exists = result.FirstOrDefault() == null ? false : true;
			return exists;
		}

		private readonly string OrganizationListRolesSQL = @"
			SELECT R.Name
				FROM		Security.UserOrganizationRole	AS	SUOR 
				INNER JOIN	Security.OrganizationRole		AS	SOR		ON SUOR.OrganizationRoleId = SOR.OrganizationRoleId
				INNER JOIN	Security.OrganizationRoleItem	AS	SORI	ON SOR.OrganizationRoleId = SORI.OrganizationRoleId
				INNER JOIN Security.Role AS R ON R.Id = RoleId
				WHERE	SOR.OrganizationId = @OrgId 
				AND		SUOR.UserId = @UserId
			";

		public async Task<IList<string>> GetOrganizationRolesAsync(User user)
		{
			return await (this.Context as Db).Database.SqlQuery<string>(OrganizationListRolesSQL, user.Id, user.ActiveOrganizationId).ToListAsync();
		}
	}
}