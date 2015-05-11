using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneX.Security
{
	public interface IOrganizationStore<TOrganization, in TKey> : IDisposable
		where TOrganization : class, IOrganization<TKey>
	{
	}

	public interface IQueryableOrganizationStore<TOrganization, in TKey> : IOrganizationStore<TOrganization, TKey>, IDisposable where TOrganization : class, IOrganization<TKey>
	{
		IQueryable<TOrganization> Organizations { get; }
	}
}