using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using GeneX.Security;
namespace GeneX.Security.Test
{
	public class DbTest : Db
	{
		public DbTest()
			: base("GeneXContext")
		{
		}

		public DbTest(string connectionString)
			: base(connectionString)
		{
		}

		public static new DbTest Create()
		{
			return Activator.CreateInstance<DbTest>();
		}
	}
}