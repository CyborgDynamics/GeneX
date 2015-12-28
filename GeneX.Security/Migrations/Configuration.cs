using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace GeneX.Security.Migrations
{
	public sealed class Configuration : DbMigrationsConfiguration<GeneX.Security.Db>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			ContextKey = "GeneX.Security.GeneXContext";
		}

		protected override void Seed(GeneX.Security.Db context)
		{
			SeedData(context);
		}

		public void SeedData(GeneX.Security.Db context)
		{
			SeedOrganizations(context);
			SeedRoles(context);

			UserStore userStore = new UserStore(context);
			UserManager userManager = new UserManager(userStore);

			// Only SuperAdmins have UserRoles
			User user1 = userManager.FindByEmail("trenton.adams@gmail.com");
			if (user1 == null)
			{
				user1 = new User() { Id = new Guid("1242EFCE-4F32-4D4D-A011-6BC5F9B46C00"), UserName = "trenton.adams@gmail.com", Email = "trenton.adams@gmail.com" };
				userManager.Create(user1, "CC2014!");
				user1.Roles.Add(new UserRole() { RoleId = Constants.Roles.Ids.SuperAdministrator, UserId = user1.Id });
				user1.Organizations.Add(new Organization { OrganizationId = new Guid("{D84B39C4-88EA-43C3-BB9E-09F1BB959453}") });
				user1.ActiveOrganizationId = new Guid("{D84B39C4-88EA-43C3-BB9E-09F1BB959453}");
				user1.Roles.Add(new UserRole() { RoleId = Constants.Roles.Ids.Administrator, UserId = user1.Id });
			}

			base.Seed(context);
		}

		public void SeedOrganizations(GeneX.Security.Db context)
		{
			context.Organization.AddOrUpdate(it => it.OrganizationId,
			   new Organization()
			   {
				   OrganizationId = new Guid("{D84B39C4-88EA-43C3-BB9E-09F1BB959453}"),
				   Description = "ArcherProd",
			   }
			);
			context.SaveChanges();
		}

		public void SeedRoles(GeneX.Security.Db context)
		{
			context.Roles.AddOrUpdate(it => it.Id,
				new Role()
				{
					Id = Constants.Roles.Ids.Administrator,
					Name = Constants.Roles.Names.Administrator,
					Description = Constants.Roles.Descriptions.Administrator
				},
				new Role()
				{
					Id = Constants.Roles.Ids.AssignUserOrganizationRole,
					Name = Constants.Roles.Names.AssignUserOrganizationRole,
					Description = Constants.Roles.Descriptions.AssignUserOrganizationRole,
				}, new Role()
				{
					Id = Constants.Roles.Ids.ReadOrganization,
					Name = Constants.Roles.Names.ReadOrganization,
					Description = Constants.Roles.Descriptions.ReadOrganization,
				},
				new Role()
				{
					Id = Constants.Roles.Ids.ReadOrganizationRole,
					Name = Constants.Roles.Names.ReadOrganizationRole,
					Description = Constants.Roles.Descriptions.ReadOrganizationRole,
				},
				new Role()
				{
					Id = Constants.Roles.Ids.ReadRole,
					Name = Constants.Roles.Names.ReadRole,
					Description = Constants.Roles.Descriptions.ReadRole,
				},
				new Role()
				{
					Id = Constants.Roles.Ids.ReadUser,
					Name = Constants.Roles.Names.ReadUser,
					Description = Constants.Roles.Descriptions.ReadUser,
				},
				new Role()
				{
					Id = Constants.Roles.Ids.SuperAdministrator,
					Name = Constants.Roles.Names.SuperAdministrator,
					Description = Constants.Roles.Descriptions.SuperAdministrator,
				},
				new Role()
				{
					Id = Constants.Roles.Ids.WriteOrganization,
					Name = Constants.Roles.Names.WriteOrganization,
					Description = Constants.Roles.Descriptions.WriteOrganization,
				},
				new Role()
				{
					Id = Constants.Roles.Ids.WriteOrganizationRole,
					Name = Constants.Roles.Names.WriteOrganizationRole,
					Description = Constants.Roles.Descriptions.WriteOrganizationRole,
				},
				new Role()
				{
					Id = Constants.Roles.Ids.WriteUser,
					Name = Constants.Roles.Names.WriteUser,
					Description = Constants.Roles.Descriptions.WriteUser,
				}
			);
			context.SaveChanges();
		}

		private void ExecuteSqlFiles(DbContext context, string folderName)
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			foreach (var file in Directory.GetFiles(Path.Combine(baseDirectory, "App_Data", "Sql", folderName), "*.sql"))
			{
				context.Database.ExecuteSqlCommand(File.ReadAllText(file), new object[0]);
			}
		}
	}
}