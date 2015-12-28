using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class Role : IdentityRole<Guid, UserRole>
	{
		public string Description { get; set; }
	}


}
