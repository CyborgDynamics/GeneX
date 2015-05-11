namespace GeneX.Model.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<GeneX.Model.GeneXContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			ContextKey = "GeneX.Model.GeneXContext";
		}

		protected override void Seed(GeneX.Model.GeneXContext context)
		{
			SeedPermissions(context);
			SeedGenomePermissions(context);
		}

		private void SeedGenomePermissions(GeneX.Model.GeneXContext context)
		{
			context.GenomePermission.AddOrUpdate(it => it.GenomePermissionId,
				new GenomePermission()
				{
					GenomePermissionId = new Guid("{86C96618-B9AE-4F1B-BF94-218141D936E6}"),
					OrganizationId = new Guid("{D84B39C4-88EA-43C3-BB9E-09F1BB959453}"),
					GenomeId = new Guid("A16171CE-063A-4344-9184-1A85445A7C3F"),
					PermissionId = Constants.Permissions.Ids.Owner,
					UserId = new Guid("43744CF2-B264-4F40-9B85-D59D4322B106")
				},
				new GenomePermission()
				{
					GenomePermissionId = new Guid("{88EF851C-E756-489D-81D5-F423A12A53B5}"),
					OrganizationId = new Guid("{D84B39C4-88EA-43C3-BB9E-09F1BB959453}"),
					GenomeId = new Guid("B1DA53EB-63CD-4AAF-8829-26B3ACECA0F3"),
					PermissionId = Constants.Permissions.Ids.Owner,
					UserId = new Guid("43744CF2-B264-4F40-9B85-D59D4322B106")
				}
			);
			context.SaveChanges();
		}

		private void SeedPermissions(GeneX.Model.GeneXContext context)
		{
			context.Permission.AddOrUpdate(it => it.PermissionId,
				new Permission()
				{
					PermissionId = Constants.Permissions.Ids.Owner,
					Name = Constants.Permissions.Names.Owner,
					Description = Constants.Permissions.Descriptions.Owner
				}
			);
			context.SaveChanges();
		}
	}
}
