using Jeedom.API.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeedom.API
{
	public class ConfigurationViewModel
	{
		public bool Populated = false;
		private const string settingConnexionAuto = "ConnexionAutoSetting";
		private const string settingFavoriteList = "FavoriteListSetting";
		private const string settingHost = "addressSetting";
		private const string settingHostExt = "addressExtSetting";
		private const string settingIdMobile = "IdMobileSetting";
		private const string settingIdPush = "IdPushSetting";
		private const string settingLogin = "LoginSetting";
		private const string settingPassword = "PasswordSetting";
		private const string settingTwoFactor = "TwoFactorSetting";
		private Address _address = new Address();
		private string _apikey;
		private bool? _connexionAuto;
		private bool isDemoEnabled = false;
		private List<string> _favoriteList;
		private bool _GeoFenceActivation;
		private string _GeoFenceActivationDistance;
		private bool _GeolocActivation;
		private string _GeolocObjectId;
		private string _idMobile;
		private string _idPush;
		private string _login;
		private bool _NotificationActivation;
		private string _NotificationObjectId;
		private string _password;
		private bool? _twoFactor;
		private string _twoFactorCode;
		private bool _useExtHost = false;

		public string ApiKey
		{
			set
			{
				if (value != null)
				{
					_apikey = value;
					TestPopulated();
				}
			}
			get
			{
				return _apikey;
			}
		}

		public bool? ConnexionAuto
		{
			get
			{
				return _connexionAuto;
			}

			set
			{
				_connexionAuto = value;
			}
		}

		public List<string> FavoriteList
		{
			get { return _favoriteList; }
			set
			{
				_favoriteList = value;
			}
		}

		public bool GeoFenceActivation
		{
			set
			{
				_GeoFenceActivation = value;
			}

			get
			{
				return _GeoFenceActivation;
			}
		}

		public string GeoFenceActivationDistance
		{
			set
			{
				_GeoFenceActivationDistance = value;
				
			}

			get
			{
				return _GeoFenceActivationDistance;
			}
		}

		public bool GeolocActivation
		{
			set
			{
				_GeolocActivation = value;
			}

			get
			{
				return _GeolocActivation;
			}
		}

		public string GeolocObjectId
		{
			set
			{
				_GeolocObjectId = value;
			}

			get
			{
				return _GeolocObjectId;
			}
		}

		public string Host
		{
			set
			{
				Address.Link = value;

				TestPopulated();
			}

			get
			{
				return Address.Link;
			}
		}

		public string IdMobile
		{
			set
			{
				if (value != null)
				{
					_idMobile = value;
				}
			}
			get
			{
				return _idMobile;
			}
		}

		public string IdPush
		{
			set
			{
				if (value != null)
				{
					_idPush = value;
				}
			}
			get
			{
				return _idPush;
			}
		}

		public bool IsDemoEnabled
		{
			get
			{
				return isDemoEnabled;
			}
		}

		public string Login
		{
			get
			{
				return _login;
			}

			set
			{
				_login = value;

			}
		}

		public bool NotificationActivation
		{
			set
			{
				_NotificationActivation = value;

			}

			get
			{
				return _NotificationActivation;
			}
		}

		public string NotificationObjectId
		{
			set
			{
				_NotificationObjectId = value;
			}

			get
			{
				return _NotificationObjectId;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}

			set
			{
				byte[] bytes = Encoding.GetEncoding(0).GetBytes(value);
				_password = Encoding.UTF8.GetString(bytes);
			}
		}

		public bool? TwoFactor
		{
			get
			{
				return _twoFactor;
			}

			set
			{
				_twoFactor = value;
			}
		}

		public string TwoFactorCode
		{
			get
			{
				return _twoFactorCode;
			}

			set
			{
				_twoFactorCode = value;
			}
		}

		/// <summary>
		/// URI d'accès au serveur JEEDOM
		/// </summary>
		public Uri Uri
		{
			get
			{
				return Address.Uri;
			}
		}

		public bool UseExtHost
		{
			get
			{
				return _useExtHost;
			}

			set
			{
				_useExtHost = value;
			}
		}

		internal Address Address
		{
			get
			{
				return _address;
			}
			set
			{
				_address = value;
			}
		}

		public ConfigurationViewModel()
		{
			//Address.Link = RoamingSettings.Values[settingHost] as string;
			//_login = RoamingSettings.Values[settingLogin] as string;
			//_password = RoamingSettings.Values[settingPassword] as string;

			//if (LocalSettings.Values[settingConnexionAuto] != null)
			//    _connexionAuto = Convert.ToBoolean(LocalSettings.Values[settingConnexionAuto]);
			//else
			//{
			//    ConnexionAuto = false;
			//}

			//Populated si API Key et host disponible
			//TestPopulated();         
		}

		public void Reset()
		{
			ApiKey = "";
			ConnexionAuto = true;
			GeoFenceActivation = false;
			GeoFenceActivationDistance = "";
			GeolocActivation = false;
			GeolocObjectId = "";
			Host = "";
			Login = "";
			NotificationActivation = false;
			NotificationObjectId = "";
			Password = "";
			TwoFactor = false;
			TwoFactorCode = "";
			FavoriteList = new List<string>();
		}

		/// <summary>
		/// Active le mode démo
		/// </summary>
		public void SetDemoMode()
		{
			isDemoEnabled = true;
			Reset();
		}

		/// <summary>
		/// Vérifie que la configuration est entièrement peuplée (api key et host disponible)
		/// </summary>
		private void TestPopulated()
		{
			if (String.IsNullOrWhiteSpace(Address.Link) || String.IsNullOrWhiteSpace(_apikey))
				Populated = false;
			else
				Populated = true;
		}
	}
}
