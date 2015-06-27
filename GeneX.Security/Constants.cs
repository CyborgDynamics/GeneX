using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public static class Constants
	{
		public static class Roles
		{
			public static class Ids
			{
				public static readonly Guid SuperAdministrator = new Guid("{726E9BEF-78D4-4DF6-82DF-D1EF9176ABA5}");
				public static readonly Guid Administrator = new Guid("{B8BFC16D-9F4B-403B-AE36-590E618EAB62}");

				public static readonly Guid ReadOrganization = new Guid("{E8428397-285D-4E25-8129-982C7978C975}");
				public static readonly Guid WriteOrganization = new Guid("{4C8B9DEA-255B-4DF3-A103-09C89C4C44A8}");

				public static readonly Guid ReadUser = new Guid("{F0B6DDFC-2C1D-47E5-A2CB-FCE6C7CBFE82}");
				public static readonly Guid WriteUser = new Guid("{C38DF54D-91C0-4B96-A03E-6A9CB99CAA52}");
				
				public static readonly Guid ReadRole = new Guid("{D813FCCC-82A2-4BB2-8B0A-FDF4ABFB32E5}");

				public static readonly Guid ReadOrganizationRole = new Guid("{9D9F62D0-6024-4DF0-97E8-1D5FBFECC41A}");
				public static readonly Guid WriteOrganizationRole = new Guid("{D55F7E37-DB03-41A6-95E3-B61C15DB2DE3}");

				public static readonly Guid AssignUserOrganizationRole = new Guid("{13D5AC58-9FFD-424F-8DCC-DE8ED3821A95}");

			}

			public static class Names
			{
				public const string Appender = ",";

				public const string SuperAdministrator = "SuperAdministrator";
				public const string Administrator = "Administrator";

				public const string ReadOrganization = "ReadOrganization";
				public const string WriteOrganization = "WriteOrganization";

				public const string ReadUser = "ReadUser";
				public const string WriteUser = "WriteUser";

				public const string ReadRole = "ReadRole";

				public const string ReadOrganizationRole = "ReadOrganizationRole";
				public const string WriteOrganizationRole = "WriteOrganizationRole";

				public const string AssignUserOrganizationRole = "AssignUserOrganizationRole";

			}

			public static class Descriptions
			{
				public const string SuperAdministrator = "Super Administrator";
				public const string Administrator = "Administrator";

				public const string ReadOrganization = "Read Organization";
				public const string WriteOrganization = "Write Organization";

				public const string ReadUser = "Read User";
				public const string WriteUser = "Write User";

				public const string ReadRole = "Read Role";

				public const string ReadOrganizationRole = "Read Organization Role";
				public const string WriteOrganizationRole = "Write Organization Role";

				public const string AssignUserOrganizationRole = "Assign User Organization Role";
			}
		}
	}
}
