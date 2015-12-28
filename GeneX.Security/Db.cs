using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GeneX.Security
{
	

	public class Db : IdentityDbContext<User, Role, Guid, UserLogin, UserRole, UserClaim>
	{
		public virtual DbSet<Organization> Organization { get; set; }
		public virtual DbSet<OrganizationRole> OrganizationRole { get; set; }
		public virtual DbSet<OrganizationRoleItem> OrganizationRoleItem { get; set; }
		public virtual DbSet<UserOrganizationRole> UserOrganizationRole { get; set; }
		public virtual DbSet<UserRole> UserRole { get; set; }

		public Db()
			: base("GeneXContext")
		{
		}

		public Db(string connectionString)
			: base(connectionString)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
			
			modelBuilder.Entity<User>().ToTable("User", "Security").Property(p => p.Id).HasColumnName("UserId");
			modelBuilder.Entity<UserRole>().ToTable("UserRole", "Security");
			modelBuilder.Entity<UserLogin>().ToTable("UserLogin", "Security");
			modelBuilder.Entity<UserClaim>().ToTable("UserClaim", "Security");
			modelBuilder.Entity<Role>().ToTable("Role", "Security");
			modelBuilder.Entity<Organization>().ToTable("Organization", "Security");
			modelBuilder.Entity<OrganizationRole>().ToTable("OrganizationRole", "Security");
			modelBuilder.Entity<OrganizationRoleItem>().ToTable("OrganizationRoleItem", "Security");
			modelBuilder.Entity<UserOrganizationRole>().ToTable("UserOrganizationRole", "Security");

			modelBuilder.Entity<OrganizationRole>().HasMany<OrganizationRoleItem>(u => u.OrganizationRoleItems).WithRequired(m => m.OrganizationRole);

			modelBuilder.Entity<User>().HasOptional(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedByUserId).WillCascadeOnDelete(false);
			modelBuilder.Entity<User>().HasOptional(u => u.UpdatedBy).WithMany().HasForeignKey(u => u.UpdatedByUserId).WillCascadeOnDelete(false);
			modelBuilder.Entity<User>().HasOptional(u => u.ActiveOrganization).WithMany().HasForeignKey(u => u.ActiveOrganizationId).WillCascadeOnDelete(false);

			modelBuilder.Entity<Organization>().HasOptional(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedByUserId).WillCascadeOnDelete(false);
			modelBuilder.Entity<OrganizationRole>().HasOptional(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedByUserId).WillCascadeOnDelete(false);

			//modelBuilder.Entity<UserOrganizationRole>()
			//	.HasRequired(a => a.OrganizationRole)
			//	.WithMany(a => a.)
			//	.WillCascadeOnDelete(true);

			//modelBuilder.Entity<UserSiteRole>()
			//	.HasRequired(a => a.User)
			//	.WithMany(a => a.SiteRoleUsers)
			//	.WillCascadeOnDelete(true);

		}

		public static Db Create()
		{
			return Activator.CreateInstance<Db>();
		}
	}
}