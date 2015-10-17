using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


using GeneX.Security;
using GeneX.Security.TestExtensions;
using Moq;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Migrations;

namespace GeneX.Security.Test
{
	public class ContextFixture : IDisposable
	{
		public DbTest Context { get; set; }
		public ContextFixture()
		{
			Database.SetInitializer<DbTest>(new DropCreateDatabaseAlways<DbTest>());
			Context = new DbTest();
			Context.Database.Initialize(true);
			GeneX.Security.Test.Migrations.Configuration configuration = new GeneX.Security.Test.Migrations.Configuration();
			configuration.ContextType = typeof(DbTest);
			configuration.SeedData(Context);
			//
			//var migrator = new DbMigrator(configuration);
			//migrator.Update();
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}

	public class UserStoreTest : IClassFixture<ContextFixture>
	{
		ContextFixture fixture;

		public UserStoreTest(ContextFixture fixture)
		{
			this.fixture = fixture;
		}


		[Fact(DisplayName = "Can Construct")]
		void CanConstruct()
		{
			// arrange
			Mock<Db> mockContext = new Mock<Db>();

			// act
			UserStore us = new UserStore(mockContext.Object);

			// assert
			Assert.NotNull(us);
		}

		[Theory, ClassData(typeof(UserIdIndex))]
		void GetOrganizationsAsyncDoesNotThrow(Guid id)
		{
			// arrange
			User u = new User { Id = id };
			DbTest context = fixture.Context;

			UserStore us = new UserStore(context);

			// act
			var ex = Record.ExceptionAsync(() => us.GetOrganizationsAsync(u));

			// assert
			Assert.Null(ex.Exception);
		}

		[Fact]
		async void GetOrganizationsAsyncReturnsOrganizationsForProperUser()
		{
			UserStore us = new UserStore(fixture.Context);
			UserManager um = new UserManager(us);
			User u = await um.FindByEmailAsync("trenton.adams@gmail.com");

			// act
			var set = await us.GetOrganizationsAsync(u);

			// assert
			Assert.NotNull(set);
			Assert.Equal(set.Count(), 1);
		}
	}

	public class UserIdIndex : IEnumerable<object[]>
	{
		private readonly List<object[]> _data = new List<object[]>
		{
			new object[] { null },
			new object[] { Guid.Empty },
			new object[] { Guid.NewGuid() },
			new object[] { new Guid("43744CF2-B264-4F40-9B85-D59D4322B106") }
		};

		public IEnumerator<object[]> GetEnumerator()
		{ return _data.GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator()
		{ return GetEnumerator(); }
	}
}