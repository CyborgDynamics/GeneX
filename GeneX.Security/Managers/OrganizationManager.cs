using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;

namespace GeneX.Security
{
	public class OrganizationManager<TOrganization, TKey> : IDisposable
		where TOrganization : IdentityOrganization<TKey>
		where TKey : System.IEquatable<TKey>
	{
		public OrganizationManager()
		{
		}

		public OrganizationManager(IOrganizationStore<TOrganization, TKey> store)
		{
			this.Store = store;
		}

		protected internal IOrganizationStore<TOrganization, TKey> Store { get; set; }

		public static OrganizationManager<TOrganization, TKey> Create(TOrganization Organization, IOwinContext context)
		{
			var db = context.Get<Db>();
			OrganizationStore<TOrganization, TKey> store = new OrganizationStore<TOrganization, TKey>(db);
			OrganizationManager<TOrganization, TKey> manager = new OrganizationManager<TOrganization, TKey>(store);
			return manager;
		}

		public IQueryable<IOrganization<TKey>> Organizations
		{
			get
			{
				IQueryableOrganizationStore<TOrganization, TKey> queryableOrganizationStore = this.Store as IQueryableOrganizationStore<TOrganization, TKey>;
				if (queryableOrganizationStore == null)
				{
					throw new NotSupportedException();
				}
				return queryableOrganizationStore.Organizations;
			}
		}

		void IDisposable.Dispose()
		{

		}
	}
}