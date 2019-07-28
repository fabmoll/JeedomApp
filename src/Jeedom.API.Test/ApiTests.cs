using Jeedom.API.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jeedom.API.Test
{
	[TestClass]
	public class ApiTests
	{
		private RequestViewModel _requestionViewModel;

		[TestInitialize]
		public void Initialize()
		{
			_requestionViewModel = new RequestViewModel();
			RequestViewModel.config.Login = "admin";
			RequestViewModel.config.Password = "";
			RequestViewModel.config.Host = "";
			RequestViewModel.config.ApiKey = "";
		}


		[TestMethod]
		public void TestMethod1()
		{
			Error error = null;
			error = _requestionViewModel.DownloadVersion().Result;

			error = _requestionViewModel.DownloadObjects().Result;

			error = _requestionViewModel.SynchMobilePlugin().Result;

			error = _requestionViewModel.DownloadScenes().Result;
			error = _requestionViewModel.DownloadMessages().Result;



		}
	}
}
