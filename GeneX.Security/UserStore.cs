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
		public UserStore(Db context)
			: base(context)
		{
		}

		protected override async Task<User> GetUserAggregateAsync(System.Linq.Expressions.Expression<Func<User, bool>> filter)
		{
			User user = await Users.Include(u => u.Roles)
				.Include(u=>u.Logins)
				.Include(u=>u.Claims)
				.FirstOrDefaultAsync(filter);

			if (user == null)
			{
				return null;
			}

			IList<Organization> myOrganizations = await GetOrganizationsAsync(user) as IList<Organization>;
			user.Organizations.AddRange(myOrganizations);
			var compositeUserRoles = ((Db)this.Context).UserOrganizationRole.Include(m=>m.OrganizationRole).Where(m => m.User.Id == user.Id && m.OrganizationRole.OrganizationId == user.ActiveOrganizationId);
			foreach (UserOrganizationRole cru in compositeUserRoles)
			{
				UserRole iur = new UserRole();
				iur.UserId = user.Id;
				user.Roles.Add(iur);
			}

			return user;
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
	}
}