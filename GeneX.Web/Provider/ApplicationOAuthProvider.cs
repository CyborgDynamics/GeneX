using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using GeneX.Web.Models;
using GeneX.Security;

namespace GeneX.Web.Provider
{
	/// <summary>
	/// TODO
	/// </summary>
	public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
	{
		private readonly string _publicClientId;

		/// <summary>
		/// TODO
		/// </summary>
		public ApplicationOAuthProvider(string publicClientId)
		{
			if (publicClientId == null)
			{
				throw new ArgumentNullException("publicClientId");
			}

			_publicClientId = publicClientId;
		}

		/// <summary>
		/// TODO
		/// </summary>
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			GeneX.Security.UserManager userManager = context.OwinContext.GetUserManager<UserManager>();

			User user = await userManager.FindAsync(context.UserName, context.Password);

			if (user == null)
			{
				context.SetError("invalid_grant", "The user name or password is incorrect.");
				return;
			}

			ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
			   OAuthDefaults.AuthenticationType);
			ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
				CookieAuthenticationDefaults.AuthenticationType);

			AuthenticationProperties properties = CreateProperties(user.UserName);
			AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
			context.Validated(ticket);
			context.Request.Context.Authentication.SignIn(cookiesIdentity);
		}

		/// <summary>
		/// TODO
		/// </summary>
		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}

			return Task.FromResult<object>(null);
		}

		/// <summary>
		/// TODO
		/// </summary>
		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			// Resource owner password credentials does not provide a client ID.
			if (context.ClientId == null)
			{
				context.Validated();
			}

			return Task.FromResult<object>(null);
		}

		/// <summary>
		/// TODO
		/// </summary>
		public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
		{
			if (context.ClientId == _publicClientId)
			{
				Uri expectedRootUri = new Uri(context.Request.Uri, "/");

				if (expectedRootUri.AbsoluteUri == context.RedirectUri)
				{
					context.Validated();
				}
			}

			return Task.FromResult<object>(null);
		}

		/// <summary>
		/// TODO
		/// </summary>
		public static AuthenticationProperties CreateProperties(string userName)
		{
			IDictionary<string, string> data = new Dictionary<string, string>
			{
				{ "userName", userName }
			};
			return new AuthenticationProperties(data);
		}
	}
}