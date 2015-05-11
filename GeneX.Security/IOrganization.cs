using System;
namespace GeneX.Security
{
	public interface IOrganization<out TKey>
	{
		TKey OrganizationId { get; }
		string DatabaseName { get; set; }
		string Description { get; set; }
		string Name { get; set; }
		string Website { get; set; }
	}
}
