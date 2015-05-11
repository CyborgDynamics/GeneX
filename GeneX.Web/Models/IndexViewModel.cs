using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace GeneX.Web.Models
{
	public class IndexViewModel
	{
		public IndexViewModel()
		{
			Genomes = new Dictionary<Guid, string>();
		}
		public Dictionary<Guid,string> Genomes { get; set; }
		public bool HasPassword { get; set; }
		public IList<UserLoginInfo> Logins { get; set; }
		public string PhoneNumber { get; set; }
		public bool TwoFactor { get; set; }
		public bool BrowserRemembered { get; set; }
	}
}