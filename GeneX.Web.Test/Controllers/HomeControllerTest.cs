using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GeneX.Web;
using GeneX.Web.Controllers;
using Xunit;

namespace GeneX.Web.Test.Controllers
{
	public class HomeControllerTest
	{
		public HomeControllerTest()
		{
		}

		[Fact]
		void CanConstruct()
		{
			HomeController hc = new HomeController();

		}
	}
}
