using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneX.Model
{
	public class GeneXContext : DbContext
	{
		public GeneXContext()
			: base("GeneXContext")
		{
		}

		public DbSet<Genome> Genome { get; set; }
		public DbSet<GenomePermission> GenomePermission { get; set; }
		public DbSet<Permission> Permission { get; set; }
		public DbSet<SNP> SNP { get; set; }
		
		

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<SNP>().ToTable("SNP", "Model");
			modelBuilder.Entity<Genome>().ToTable("Genome", "Model");
			modelBuilder.Entity<GenomePermission>().ToTable("GenomePermission", "Model");
			modelBuilder.Entity<Permission>().ToTable("Permission", "Model");

			//modelBuilder.Entity<GenomePermission>().HasRequired(m => m.UserId).WithMany().HasForeignKey(m => m.UserId);
		}
	}
}