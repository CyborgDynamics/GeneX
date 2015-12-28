using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class RoleManager : RoleManager<Role, Guid>
	{
		public RoleManager(IRoleStore<Role, Guid> store) : base(store) { }
	}
}
