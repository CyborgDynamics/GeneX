using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace GeneX.Web.Provider
{
	public static class Extensions
	{
		/// <summary>
		/// Extracts NameIdentifier from Claims Identity
		/// </summary>
		/// <param name="identity">IIdentity</param>
		/// <returns>Guid</returns>
		public static Guid GetUserGuid(this IIdentity identity)
		{
			string temp = string.Empty;
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			ClaimsIdentity identity1 = identity as ClaimsIdentity;
			if (identity1 != null)
			{
				temp = IdentityExtensions.FindFirstValue(identity1, ClaimTypes.NameIdentifier);
			}
			return new Guid(temp);
		}

		/// <summary>
		/// Returns Group SID (OrganizationID)
		/// </summary>
		/// <param name="identity">IIdentity</param>
		/// <returns>Group Sid</returns>
		public static Guid GetUserGroupId(this IIdentity identity)
		{
			string temp = string.Empty;
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			ClaimsIdentity identity1 = identity as ClaimsIdentity;
			if (identity1 != null)
			{
				temp = IdentityExtensions.FindFirstValue(identity1, ClaimTypes.GroupSid);
			}
			return new Guid(temp);
		}
	}
}