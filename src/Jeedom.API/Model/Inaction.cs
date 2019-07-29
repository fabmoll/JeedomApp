using System;
using System.Collections.Generic;
using System.Text;

namespace Jeedom.API.Model
{
	public class Inaction
	{
		public Options options { get; set; }
		public string onlyIfMode { get; set; }
		public string cmd { get; set; }
	}

}
