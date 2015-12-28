using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Globalization;
using System.Data.Entity.Utilities;
using System.Security.Claims;

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
			return (await Store.FindByIdAsync(userId)).ActiveOrganizationId;
		}

		public string GetOrganizationConnectionString(Guid userId)
		{
			User user = this.FindById(userId);
			if (user.ActiveOrganizationId.HasValue)
			{
				return ((UserStore)Store).GetOrganizationConnectionString(user.ActiveOrganizationId.Value);
			}
			return string.Empty;
		}

		public override async Task<IdentityResult> AddToRoleAsync(Guid userId, string role)
		{
			IUserRoleStore<User, Guid> userRoleStore = GetUserRoleStore();
			User tUser = await FindByIdAsync(userId).WithCurrentCulture<User>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "User Id Not Found", new object[]
				{
			userId
				}));
			}
			IList<string> list = await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture<IList<string>>();
			IdentityResult result;
			if (list.Contains(role))
			{
				result = new IdentityResult(new string[]
				{
			"User Already in Role"
				});
			}
			else
			{
				await userRoleStore.AddToRoleAsync(tUser, role).WithCurrentCulture();
				result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
			}
			return result;
		}
		private IUserRoleStore<User, Guid> GetUserRoleStore()
		{
			//IUserRoleStore<User, Guid> userRoleStore = this.Store as IUserRoleStore<User, Guid>;
			return new UserRoleStore();
			//if (userRoleStore == null)
			//{
			//	throw new NotSupportedException("Store is not UserRoleStore<User,Guid>");
			//}
			//return userRoleStore;
		}

		public override Task<bool> IsInRoleAsync(Guid userId, string role)
		{
			return base.IsInRoleAsync(userId, role);
		}

		public override Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
		{
			return base.CreateIdentityAsync(user, authenticationType);
		}

		public async Task<IList<string>> GetOrganizationRolesAsync(User user)
		{
			UserStore store = this.Store as UserStore;
			return await store.GetOrganizationRolesAsync(user);
		}

	}
}