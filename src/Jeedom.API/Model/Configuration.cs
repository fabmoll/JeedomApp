using System;
using System.Collections.Generic;
using System.Text;

namespace Jeedom.API.Model
{
	public class Configuration
	{
		public int parentNumber { get; set; }
		public string tagColor { get; set; }
		public string tagTextColor { get; set; }
		public string desktopsummaryTextColor { get; set; }
		public string mobilesummaryTextColor { get; set; }
		public string hideOnDashboard { get; set; }
		public string summaryglobalsecurity { get; set; }
		public string summaryglobalmotion { get; set; }
		public string summaryglobaldoor { get; set; }
		public string summaryglobalwindows { get; set; }
		public string summaryglobalshutter { get; set; }
		public string summarygloballight { get; set; }
		public string summaryglobaloutlet { get; set; }
		public string summaryglobaltemperature { get; set; }
		public string summaryglobalhumidity { get; set; }
		public string summarygloballuminosity { get; set; }
		public string summaryglobalpower { get; set; }
		public string summaryhidedesktopsecurity { get; set; }
		public string summaryhidedesktopmotion { get; set; }
		public string summaryhidedesktopdoor { get; set; }
		public string summaryhidedesktopwindows { get; set; }
		public string summaryhidedesktopshutter { get; set; }
		public string summaryhidedesktoplight { get; set; }
		public string summaryhidedesktopoutlet { get; set; }
		public string summaryhidedesktoptemperature { get; set; }
		public string summaryhidedesktophumidity { get; set; }
		public string summaryhidedesktopluminosity { get; set; }
		public string summaryhidedesktoppower { get; set; }
		public string summaryhidemobilesecurity { get; set; }
		public string summaryhidemobilemotion { get; set; }
		public string summaryhidemobiledoor { get; set; }
		public string summaryhidemobilewindows { get; set; }
		public string summaryhidemobileshutter { get; set; }
		public string summaryhidemobilelight { get; set; }
		public string summaryhidemobileoutlet { get; set; }
		public string summaryhidemobiletemperature { get; set; }
		public string summaryhidemobilehumidity { get; set; }
		public string summaryhidemobileluminosity { get; set; }
		public string summaryhidemobilepower { get; set; }
		public Summary summary { get; set; }
		public string summaryglobalpresence { get; set; }
		public string summaryglobalcamera { get; set; }
		public string summaryhidedesktoppresence { get; set; }
		public string summaryhidedesktopcamera { get; set; }
		public string summaryhidemobilepresence { get; set; }
		public string summaryhidemobilecamera { get; set; }
	}
}
