using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class RoleStore : RoleStore<Role, Guid, UserRole>
	{
		public RoleStore(Db context)
			: base(context)
		{
		}

		public override Task UpdateAsync(Role role)
		{
			return base.UpdateAsync(role);
		}
	}
}
