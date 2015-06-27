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
		//
		// Summary:
		//     Return the user id as a Guid
		//
		// Parameters:
		//   identity:
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
				temp = IdentityExtensions.FindFirstValue(identity1, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
			}
			return new Guid(temp);
		}
	}
}