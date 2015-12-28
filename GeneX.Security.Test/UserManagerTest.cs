using System;
using Xunit;
using Moq;
namespace GeneX.Security.Test
{
	[System.Runtime.InteropServices.Guid("D1786082-ABC7-4F77-A4F9-F5C3920240F0")]
	public class UserManagerTest : IClassFixture<ContextFixture>
	{
		ContextFixture fixture;

		public UserManagerTest(ContextFixture fixture)
		{
			this.fixture = fixture;
		}

		[Fact(DisplayName = "User Manager - Can Construct")]
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

		[Theory(DisplayName ="User Manager - Get Organizations"), ClassData(typeof(UserIdIndex))]
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