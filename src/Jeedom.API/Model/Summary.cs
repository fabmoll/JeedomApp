using System;
using System.Collections.Generic;
using System.Text;

namespace Jeedom.API.Model
{
	public class Summary
	{
		public object[] security { get; set; }
		public object[] motion { get; set; }
		public object[] door { get; set; }
		public object[] windows { get; set; }
		public object[] shutter { get; set; }
		public object[] light { get; set; }
		public object[] outlet { get; set; }
		public object[] temperature { get; set; }
		public object[] humidity { get; set; }
		public object[] luminosity { get; set; }
		public object[] power { get; set; }
		public Presence[] presence { get; set; }
		public Camera[] camera { get; set; }
	}

}
