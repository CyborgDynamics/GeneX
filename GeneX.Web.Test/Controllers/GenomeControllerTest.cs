using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

using GeneX.Web.Controllers;

using Moq;
using Xunit;
using Xunit.Extensions;

namespace GeneX.Web.Test.Controllers
{
	public class GenomeControllerTest
	{
		public GenomeControllerTest()
		{
		}

		[Fact]
		void CanConstruct()
		{
			GenomeController gc = new GenomeController();
			Assert.NotNull(gc);
		}

		[Theory]
		[ClassData(typeof(IndexOfGuids))]
		void Index_Get_ReturnsView(Guid? value)
		{
			GenomeController gc = new GenomeController();

			ActionResult v = gc.Index(value);
			Assert.NotNull(v);
			Assert.IsType<ViewResult>(v);
		}

		[Theory]
		[ClassData(typeof(IndexOfGuidFilesInvalid))]
		void Index_Post_InvalidDataReturnsError(Guid? value, HttpPostedFileBase file)
		{
			GenomeController gc = new GenomeController();
			ActionResult r = gc.Index(value, file);
			Assert.NotNull(r);
			Assert.IsType<ViewResult>(r);
		}

		[Theory]
		[ClassData(typeof(IndexOfGuidFilesValid))]
		void Index_Post_ValidDataRedirectsHome(Guid? value, HttpPostedFileBase file)
		{
			GenomeController gc = new GenomeController();
			
			ActionResult r = gc.Index(value, file);
			Assert.NotNull(r);
			Assert.IsType<RedirectToRouteResult>(r);
		}

		[Fact]
		void Add_Get_ReturnsView()
		{
			GenomeController gc = new GenomeController();
			ActionResult v = gc.Add();
			Assert.NotNull(v);
			Assert.IsType<ViewResult>(v);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("asdf1234")]
		[InlineData("\\<>d!@#$%$%^&*()")]
		void Add_Post_RedirectsHome(string value)
		{
			GenomeController gc = new GenomeController();
			ActionResult r = gc.Add(value);
			Assert.NotNull(r);
			Assert.IsType<RedirectToRouteResult>(r);
		}

		class IndexOfGuids : IEnumerable<object[]>
		{
			private readonly List<object[]> _data = new List<object[]>
    {
		new object[] { null },
        new object[] { Guid.NewGuid() },
        new object[] { Guid.Empty }
    };

			public IEnumerator<object[]> GetEnumerator()
			{ return _data.GetEnumerator(); }

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		class IndexOfGuidFilesInvalid : IEnumerable<object[]>
		{
			private readonly List<object[]> _data = new List<object[]>
    {
		new object[] { null, null },
        new object[] { Guid.NewGuid(), null },
        new object[] { Guid.Empty, null },
		//new object[] { null, new HttpPostedFileWrapper() },
        //new object[] { Guid.Empty, new HttpPostedFileWrapper() },

    };

			public IEnumerator<object[]> GetEnumerator()
			{ return _data.GetEnumerator(); }

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		class IndexOfGuidFilesValid : IEnumerable<object[]>
		{
			private readonly List<object[]> _data = new List<object[]>
    {
		//new object[] { null, null },
        //new object[] { Guid.NewGuid(), null },
        //new object[] { Guid.Empty, null },
		///new object[] { null, new GeneXHttpPostedFile() },
        new object[] { Guid.NewGuid(), null}//new HttpPostedFile()}
        //new object[] { Guid.Empty, new GeneXHttpPostedFile() },

    };

			public IEnumerator<object[]> GetEnumerator()
			{ return _data.GetEnumerator(); }

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}