using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneX.Security
{
	public class OrganizationStore<TOrganization, TKey> : IOrganizationStore<TOrganization, TKey>, IQueryableOrganizationStore<TOrganization, TKey>
		where TOrganization : IdentityOrganization<TKey>
		where TKey : System.IEquatable<TKey>
	{
		// TODO: Refactor this to a more "gracious" context
		private Db db;

		public OrganizationStore(Db context)
		{
			db = context;
		}

		public IQueryable<TOrganization> Organizations
		{
			get
			{
				return (IQueryable<TOrganization>)db.Organization;
			}
		}

		void IDisposable.Dispose()
		{
		}
	}
}
