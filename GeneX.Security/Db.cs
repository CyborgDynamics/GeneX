using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
namespace GeneX.Security
{
	public class UserLogin : IdentityUserLogin<Guid> { }
	public class UserRole : IdentityUserRole<Guid> { }
	public class Role : IdentityRole<Guid, UserRole>
	{
		public string Description { get; set; }
	}

	public class RoleStore : RoleStore<Role, Guid, UserRole> 
	{
		public RoleStore(Db context)
			: base(context)
		{
		}
	}

	public class RoleManager : RoleManager<Role, Guid> 
	{
		public RoleManager(IRoleStore<Role,Guid> store) : base(store) { }
	}

	public class UserClaim : IdentityUserClaim<Guid> { }

	
	public class Db : IdentityDbContext<User, Role, Guid, UserLogin, UserRole, UserClaim>
	{
		public DbSet<Organization> Organization { get; set; }
		public DbSet<OrganizationRole> OrganizationRole { get; set; }
		public DbSet<UserOrganizationRole> UserOrganizationRole { get; set; }
		public DbSet<UserRole> UserRole { get; set; }

		public Db()
			: base("GeneXContext")
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
			modelBuilder.Entity<UserOrganizationRole>().ToTable("UserOrganizationRole", "Security");

			//modelBuilder.Entity<Organization>().HasMany(u => u.).WithRequired(w => w.Organization);
			//modelBuilder.Entity<OrganizationRole>().HasMany(u => u.Organization).WithRequired(w => w.OrganizationRole);

			modelBuilder.Entity<User>().HasOptional(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedByUserId).WillCascadeOnDelete(false);
			modelBuilder.Entity<User>().HasOptional(u => u.UpdatedBy).WithMany().HasForeignKey(u => u.UpdatedByUserId).WillCascadeOnDelete(false);
			modelBuilder.Entity<User>().HasOptional(u => u.ActiveOrganization).WithMany().HasForeignKey(u => u.ActiveOrganizationId).WillCascadeOnDelete(false);

			modelBuilder.Entity<Organization>().HasOptional(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedByUserId).WillCascadeOnDelete(false);
			modelBuilder.Entity<OrganizationRole>().HasOptional(u => u.CreatedBy).WithMany().HasForeignKey(u => u.CreatedByUserId).WillCascadeOnDelete(false);

			//modelBuilder.Entity<UserSiteRole>()
			//	.HasRequired(a => a.SiteRole)
			//	.WithMany(a => a.UserSiteRoles)
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