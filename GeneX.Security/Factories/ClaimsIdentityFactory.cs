using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Utilities;
using GeneX.Security;

namespace GeneX.Security.Factories
{
	public class ClaimsIdentityFactory : ClaimsIdentityFactory<User,Guid>
	{
		public override async Task<ClaimsIdentity> CreateAsync(UserManager<User, Guid> manager, User user, string authenticationType)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			var claims = new List<Claim>()
			{ 
				// http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
				new Claim(ClaimTypes.Name, user.Email), 
				// http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				// http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid
				new Claim(ClaimTypes.GroupSid, user.ActiveOrganizationId.ToString())
			};

			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie); 
			if (manager.SupportsUserSecurityStamp)
			{
				claimsIdentity.AddClaim(new Claim(this.SecurityStampClaimType, await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture<string>()));
			}
			if (manager.SupportsUserRole)
			{
				IList<string> roleList = await (manager as UserManager).GetRolesAsync(user.Id).WithCurrentCulture<IList<string>>();
				foreach (string current in roleList)
				{
					claimsIdentity.AddClaim(new Claim(this.RoleClaimType, current, "http://www.w3.org/2001/XMLSchema#string"));
				}

				IList<string> orgRoleList = await (manager as UserManager).GetOrganizationRolesAsync(user).WithCurrentCulture<IList<string>>();
				foreach (string current in orgRoleList)
				{
					claimsIdentity.AddClaim(new Claim(this.RoleClaimType, current, "http://www.w3.org/2001/XMLSchema#string"));
				}
			}
			if (manager.SupportsUserClaim)
			{
				claimsIdentity.AddClaims(await manager.GetClaimsAsync(user.Id).WithCurrentCulture<IList<Claim>>());
			}
			return claimsIdentity;
		}
	}
}
