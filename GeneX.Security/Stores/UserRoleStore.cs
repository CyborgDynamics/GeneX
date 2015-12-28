using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class UserRoleStore : IUserRoleStore<User, Guid>
	{
		public Task AddToRoleAsync(User user, string roleName)
		{
			throw new NotImplementedException();
		}

		public Task CreateAsync(User user)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(User user)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public Task<User> FindByIdAsync(Guid userId)
		{
			throw new NotImplementedException();
		}

		public Task<User> FindByNameAsync(string userName)
		{
			throw new NotImplementedException();
		}

		public Task<IList<string>> GetRolesAsync(User user)
		{
			throw new NotImplementedException();
		}

		public Task<bool> IsInRoleAsync(User user, string roleName)
		{
			throw new NotImplementedException();
		}

		public Task RemoveFromRoleAsync(User user, string roleName)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(User user)
		{
			throw new NotImplementedException();
		}
	}
}
