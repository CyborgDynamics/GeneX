using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;
using Moq;
using GeneX.Security.Test;
namespace GeneX.Security.UserManager.Test
{
	public class UserManagerTest : IClassFixture<ContextFixture>
	{
		ContextFixture fixture;

		public UserManagerTest(ContextFixture fixture)
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
			UserManager um = new UserManager(us);

			// assert
			Assert.NotNull(um);
		}

		[Theory, ClassData(typeof(UserIdIndex))]
		void GetOrganizationsAsyncDoesNotThrow(Guid id)
		{
			//// arrange
			//User u = new User { Id = id };
			//DbTest context = fixture.Context;

			//UserStore us = new UserStore(context);

			//// act
			//var ex = Record.ExceptionAsync(() => us.GetOrganizationsAsync(u));

			//// assert
			//Assert.Null(ex.Exception);
		}

		

	}
}