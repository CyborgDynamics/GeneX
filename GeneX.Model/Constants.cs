using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Model
{
	public static class Constants
	{
		public static class Permissions
		{
			public static class Ids
			{
				public static readonly Guid Owner = new Guid("{5891ABAC-BA7A-4B8F-A9B0-A95C4B0836E8}");
			}

			public static class Names
			{
				public const string Appender = ",";
				public const string Owner = "Owner";
			}

			public static class Descriptions
			{
				public const string Owner = "Owner";
			}
		}
	}
}
