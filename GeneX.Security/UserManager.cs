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
	public class UserManager : UserManager<User, Guid>
	{
		public UserManager(UserStore store)
			: base(store)
		{
		}

		public override Task<IdentityResult> CreateAsync(User user, string password)
		{
			user.Id = Guid.NewGuid();
			return base.CreateAsync(user, password);
		}

		public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
		{
			UserManager manager = new UserManager(new UserStore(context.Get<Db>()));
			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<User, Guid>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = true,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = true,
			};

			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider = new DataProtectorTokenProvider<User, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}

		/// <summary>
		/// Sets the current active OrganizationId that the user is working on
		/// </summary>
		/// <param name="userId">The User Id</param>
		/// <param name="OrganizationId">The Organization Id</param>
		public async void SetActiveOrganizationAsync(Guid userId, Guid? OrganizationId)
		{
			User user = await Store.FindByIdAsync(userId);
			user.ActiveOrganizationId = OrganizationId;
			await Store.UpdateAsync(user);
		}

		/// <summary>
		/// Gets the current active OrganizationId that the user is working on
		/// </summary>
		/// <param name="userId">The User Id</param>
		/// <returns>OrganizationId</returns>
		public async Task<Guid?> GetActiveOrganizationAsync(Guid userId)
		{
			User user = await Store.FindByIdAsync(userId);
			return user.ActiveOrganizationId;
		}

		public string GetOrganizationConnectionString(Guid userId)
		{
			User user = this.FindById(userId);
			if ( user.ActiveOrganizationId.HasValue )
			{
			return ((UserStore)Store).GetOrganizationConnectionString(user.ActiveOrganizationId.Value);
			}
			return string.Empty;
		}
	}
}