using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;

using Microsoft.AspNet.Identity;

using GeneX.Security;

namespace GeneX.Security.Test.Migrations
{

    internal class Configuration : DbMigrationsConfiguration<DbTest>
    {
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
			ContextKey = "GeneX.Security.GeneXContext";
		}

		protected override void Seed(DbTest context)
		{
			SeedData(context);
		}

		public void SeedData(DbTest context)
		{
			SeedOrganizations(context);
			SeedRoles(context);

			UserStore userStore = new UserStore(context);
			UserManager userManager = new UserManager(userStore);

			// Only SuperAdmins have UserRoles
			User user1 = userManager.FindByEmail("trenton.adams@gmail.com");
			if (user1 == null)
			{
				user1 = new User() {UserName = "trenton.adams@gmail.com", Email = "trenton.adams@gmail.com" };
				userManager.Create(user1, "CC2014!");
				user1.Roles.Add(new UserRole() { RoleId = Constants.Roles.Ids.SuperAdministrator, UserId = user1.Id });
			}
			context.SaveChanges();

			user1 = userManager.FindByEmail("trenton.adams@gmail.com");

			OrganizationRole or = new OrganizationRole() { 
				OrganizationRoleId = new Guid("{A1909F34-5C69-4C72-B676-59C2ACB079CA}"), 
				CreatedByUserId = user1.Id, 
				IsDeleted = false, 
				Name = "Temp",
				OrganizationId = new Guid("{D84B39C4-88EA-43C3-BB9E-09F1BB959453}"), 
				CreatedDate = DateTime.Now };
			
			OrganizationRoleItem ori = new OrganizationRoleItem() { 
				OrganizationRoleItemId = new Guid("{A1909F34-5C69-4C72-B676-59C2ACB079CB}"), 
				OrganizationRoleId = new Guid("{A1909F34-5C69-4C72-B676-59C2ACB079CA}"), 
				RoleId = Constants.Roles.Ids.ReadOrganization,
				CreatedDate = DateTime.Now,
				CreatedByUserId = user1.Id
			};
			OrganizationRoleItem ori2 = new OrganizationRoleItem() { 
				OrganizationRoleItemId = new Guid("{A1909F34-5C69-4C72-B676-59C2ACB079CC}"), 
				OrganizationRoleId = new Guid("{A1909F34-5C69-4C72-B676-59C2ACB079CA}"),
				CreatedByUserId = user1.Id, 
				CreatedDate = DateTime.Now,
				RoleId = Constants.Roles.Ids.WriteOrganization };
			
			UserOrganizationRole uor = new UserOrganizationRole() { 
				UserOrganizationRoleId = new Guid("{3179DBA0-CF8F-404A-A727-8F8032021A41}"),
				UserId = user1.Id, 
				OrganizationRoleId = new Guid("{A1909F34-5C69-4C72-B676-59C2ACB079CA}") };

			context.Users.AddOrUpdate(user1);
			context.OrganizationRole.AddOrUpdate(or);
			context.UserOrganizationRole.AddOrUpdate(uor);
			context.OrganizationRoleItem.AddOrUpdate(ori);
			context.OrganizationRoleItem.AddOrUpdate(ori2);
			int a = context.SaveChanges();
			//base.Seed(context);
		}

		public void SeedOrganizations(DbTest context)
		{
			context.Organization.AddOrUpdate(new Organization()
			   {
				   OrganizationId = new Guid("{D84B39C4-88EA-43C3-BB9E-09F1BB959453}"),
				   Description = "ArcherProd",
			   }
			);
			context.SaveChanges();
		}

		public void SeedRoles(DbTest context)
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
