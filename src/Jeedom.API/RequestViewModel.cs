
using Jeedom.API.Api.Http;
using Jeedom.API.Api.Json;
using Jeedom.API.Api.Json.Event;
using Jeedom.API.Api.Json.Response;
using Jeedom.API.Model;
using Jeedom.API.Mvvm;
using Jeedom.API.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Jeedom.API
{
    // TODO: convertir le singleton en class threadsafe avec accès sur des objets statiques
    public sealed class RequestViewModel : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged;

		

		static public ConfigurationViewModel config = new ConfigurationViewModel();
        public string configByKey = "";
        public string InteractReply;
        public bool Populated = false;
        public CancellationTokenSource tokenSource;
        private static volatile RequestViewModel _instance;
        private static object threadLock = new object();
        private ObservableCollection<Model.Command> _commandList = new ObservableCollection<Model.Command>();
        private double _dateTime;
        private ObservableCollection<EqLogic> _eqLogicList = new ObservableCollection<EqLogic>();
        private ObservableCollection<JdItem> _favoriteList = new ObservableCollection<JdItem>();
        private List<string> _favoriteIdList = new List<string>();
        private ObservableCollection<Interact> _interactList = new ObservableCollection<Interact>();
        private string _loadingMessage;
        private ObservableCollection<Message> _messageList = new ObservableCollection<Message>();
        private ObservableCollection<JeedomObject> _objectList = new ObservableCollection<JeedomObject>();
        private int _progress = 0;
        private RelayCommand<object> _RefreshCommand;
        private ObservableCollection<Scene> _sceneList = new ObservableCollection<Scene>();
        private Boolean _updating;
        private string _version;
        private int pass = 0;

		public ObservableCollection<Model.Command> CommandList
		{
			get { return _commandList; }
			set
			{
				_commandList = value;
				NotifyPropertyChanged();
			}
		}

		public ObservableCollection<EqLogic> EqLogicList
		{
			get { return _eqLogicList; }
			set
			{
				_eqLogicList = value;
				NotifyPropertyChanged();
			}
		}

		public ObservableCollection<JdItem> FavoriteList
		{
			get { return _favoriteList; }
			set
			{
				_favoriteList = value;
				NotifyPropertyChanged();
			}
		}

		public ObservableCollection<Interact> InteractList
		{
			get { return _interactList; }
			set
			{
				_interactList = value;
				NotifyPropertyChanged();
			}
		}

		public string LoadingMessage
		{
			get
			{
				return _loadingMessage;
			}

			private set
			{
				_loadingMessage = value;
				NotifyPropertyChanged();
			}
		}

		public ObservableCollection<Message> MessageList
		{
			get { return _messageList; }
			set
			{
				_messageList = value;
				NotifyPropertyChanged();
			}
		}

		public ObservableCollection<JeedomObject> ObjectList
		{
			get { return _objectList; }
			set
			{
				_objectList = value;
				NotifyPropertyChanged();
			}
		}

		public int Progress
		{
			get { return _progress; }
			set
			{
				_progress = value;
				NotifyPropertyChanged();
			}
		}

		public RelayCommand<object> RefreshCommand
		{
			get
			{
				_RefreshCommand = _RefreshCommand ?? new RelayCommand<object>(async parameters =>
				{
					await UpdateTask();
				});
				return _RefreshCommand;
			}
		}

		public ObservableCollection<Scene> SceneList
		{
			get { return _sceneList; }
			set
			{
				_sceneList = value;
				NotifyPropertyChanged();
			}
		}

		public Boolean Updating
		{
			get
			{
				return _updating;
			}
			set
			{
				_updating = value;
				NotifyPropertyChanged();
			}
		}

		public string Version
		{
			get { return _version; }
			set
			{
				_version = value;
				NotifyPropertyChanged();
			}
		}

		public RequestViewModel()
        {
        }

        public static RequestViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(threadLock)
                    {
                        if (_instance == null)
                            _instance = new RequestViewModel();
                    }
                }

                return _instance;
            }
        }
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public void AddToFavorite(Model.EqLogic eq)
		{
			if (!FavoriteList.Contains(eq))
			{
				FavoriteList.Add(eq);
				_favoriteIdList.Add("eq_" + eq.Id);
				config.FavoriteList = _favoriteIdList;
			}
		}

		public void AddToFavorite(Scene sc)
		{
			if (!FavoriteList.Contains(sc))
			{
				FavoriteList.Add(sc);
				_favoriteIdList.Add("sc_" + sc.Id);
				config.FavoriteList = _favoriteIdList;
			}
		}

		public async Task<Error> CheckTwoFactorConnexion()
		{
			Parameters parameters = new Parameters();
			parameters.login = config.Login;
			var jsonrpc = new JsonRpcClient(parameters);

			if (await jsonrpc.SendRequest("user::useTwoFactorAuthentification"))
			{
				var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				if (reponse.result == "1")
					config.TwoFactor = true;
				else
					config.TwoFactor = false;
			}

			return jsonrpc.Error;
		}

		public async Task<Error> ConnectJeedomByLogin()
		{
			Parameters parameters = new Parameters();
			parameters.login = config.Login;
			parameters.password = config.Password;
			//if (config.TwoFactor == true)
			//    parameters.twoFactorCode = config.TwoFactorCode;
			var jsonrpc = new JsonRpcClient(parameters);

			//LoadingMessage = "Tentative de connexion en https";
			config.Address.ProtocolType = Address.Protocol.Https;
			if (await jsonrpc.SendRequest("user::getHash"))
			{
				var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				config.ApiKey = reponse.result;
				return jsonrpc.Error;
			}

			//LoadingMessage = "Tentative de connexion en https en ignorant les erreurs de certificat";
			config.Address.ProtocolType = Address.Protocol.SelfSigned;
			if (await jsonrpc.SendRequest("user::getHash"))
			{
				var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				config.ApiKey = reponse.result;
				return jsonrpc.Error;
			}

			//LoadingMessage = "Tentative de connexion en http";
			config.Address.ProtocolType = Address.Protocol.Http;
			if (await jsonrpc.SendRequest("user::getHash"))
			{
				var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				config.ApiKey = reponse.result;
				return jsonrpc.Error;
			}
			// Par sécurité on retourne sur https
			config.Address.ProtocolType = Address.Protocol.Https;
			return jsonrpc.Error;
		}

		public async Task<Error> CreateEqLogicMobile()
		{
			Parameters parameters = new Parameters();
			parameters.plugin = "mobile";
			parameters.platform = "windows";
			var jsonrpc = new JsonRpcClient(parameters);
			if (await jsonrpc.SendRequest("Iq"))
			{
				// Récupère la liste de tous les eqLogics
				var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				if (reponse != null)
				{
					config.IdMobile = reponse.result;
				}
			}

			return jsonrpc.Error;
		}

		public async Task<Error> DownloadInteraction()
		{
			var jsonrpc = new JsonRpcClient();
			//Ajouter le téléchargemnent et la mise a jours des interaction Jeedom
			try
			{
				//var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///JeedomAppVoiceCommandes.xml"));
				//await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);

				//VoiceCommandDefinition commandDefinitions;

				//string countryCode = CultureInfo.CurrentCulture.Name.ToLower();
				//if (countryCode.Length == 0)
				//{
				//	countryCode = "fr-fr";
				//}

				//if (VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue("JeedomAppCommandSet_" + countryCode, out commandDefinitions))
				//{
				//	List<string> InteractsList = new List<string>();
				//	if (await jsonrpc.SendRequest("interactQuery::all"))
				//	{
				//		InteractList.Clear();
				//		var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<Interact>>>();
				//		if (response != null)
				//			InteractList = response.result;
				//	}

				//	foreach (var Iteract in InteractList)
				//	{
				//		InteractsList.Add(Iteract.query);
				//	}
				//	await commandDefinitions.SetPhraseListAsync("InteractList", InteractsList);
				//}
			}
			catch (Exception ex)
			{
				// System.Diagnostics.Debug.WriteLine("Updating Phrase list for VCDs: " + ex.ToString());
			}

			return jsonrpc.Error;
		}

		public async Task<Error> DownloadMessages()
		{
			var jsonrpc = new JsonRpcClient();

			if (await jsonrpc.SendRequest("message::all"))
			{
				MessageList.Clear();
				var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<Message>>>();
				if (response != null)
					MessageList = response.result;
			}

			return jsonrpc.Error;
		}

		public async Task<Error> DownloadObjects()
		{
			var jsonrpc = new JsonRpcClient();

			if (await jsonrpc.SendRequest("object::full"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<RootJeedomObjects>();
				foreach (JeedomObject obj in response.result)
				{
					var lst = from o in ObjectList where o.id == obj.id select o;
					if (lst.Count() != 0)
					{
						var ob = lst.FirstOrDefault();
						ob = obj;
					}
					else
						ObjectList.Add(obj);
				}
			}
			return jsonrpc.Error;
		}

		public async Task<Error> DownloadScenes()
		{
			var jsonrpc = new JsonRpcClient();

			if (await jsonrpc.SendRequest("scenario::all"))
			{
				SceneList.Clear();
				var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<Scene>>>();
				if (response != null)
					SceneList = response.result;
			}

			return jsonrpc.Error;
		}

		public async Task<Error> DownloadVersion()
		{
			var jsonrpc = new JsonRpcClient();
			if (await jsonrpc.SendRequest("version"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				Version = response.result;
			}

			return jsonrpc.Error;
		}

		public async Task ExecuteCommand(Command cmd, Parameters parameters = null)
		{
			if (config.IsDemoEnabled)
				return;

			cmd.Updating = true;
			if (parameters == null)
			{
				parameters = new Parameters();
				parameters.id = cmd.id;
				parameters.name = cmd.name;
			}
			var jsonrpc = new JsonRpcClient(parameters);

			if (await jsonrpc.SendRequest("cmd::execCmd"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<CommandResult>>();
				cmd.value = response.result.value;
			}
			else
			{
				cmd.value = "N/A";
			}
			cmd.Updating = false;
		}

		public async Task FirstLaunch()
		{
			Updating = true;

			int pg = 100 / 8;

			LoadingMessage = "Inscription au notification";
			if (config.NotificationActivation == true)
				UpdateNotificationChannel();

			LoadingMessage = "Chargement de la Version";
			var error = await DownloadVersion();
			Progress += pg;

			LoadingMessage = "Chargement des Objets";
			error = await DownloadObjects();
			Progress += pg;

			LoadingMessage = "Chargement des Equipements";
			error = await SynchMobilePlugin();
			Progress += pg;

			LoadingMessage = "Chargement des Scénarios";
			error = await DownloadScenes();
			Progress += pg;

			LoadingMessage = "Chargement des Messages";
			error = await DownloadMessages();
			Progress += pg;

			//LoadingMessage = "Chargement des informations des Commandes";
			//error = await GetEventChanges();
			//Progress += pg;

			LoadingMessage = "Chargement des Interactions";
			await DownloadInteraction();
			Progress += pg;

			LoadingMessage = "Prêt";
			Updating = false;
		}

		public async Task<Error> GetEventChanges()
		{
			var parameters = new Parameters();
			parameters.datetime = _dateTime;
			var jsonrpc = new JsonRpcClient(parameters);

			if (await jsonrpc.SendRequest("event::changes"))
			{
				var response = jsonrpc.GetEvents();
				foreach (JdEvent e in response.Result)
				{
					switch (e.Name)
					{
						case "cmd::update":
							var ev = e as Event<EventOptionCmd>;
							var cmd = (from c in CommandList where c.id == ev.Option.CmdId select c).FirstOrDefault();
							if (cmd != null)
							{
								if (cmd.DateTime < ev.DateTime)
								{
									cmd.Value = ev.Option.Value;
									cmd.DateTime = ev.DateTime;
								}
							}
							break;

						case "eqLogic::update":
							var eveq = e as Event<EventOptionEqLogic>;
							var eq = (from c in EqLogicList where c.Id == eveq.Option.EqLogicId select c).FirstOrDefault();

							if (eq != null)
							{
								if (eq.DateTime < eveq.DateTime)
								{
									await UpdateEqLogic(eq);
									eq.DateTime = eveq.DateTime;
								}
							}
							break;

						default:
							break;
					}
				}
				_dateTime = response.DateTime;
			}

			return jsonrpc.Error;
		}

		public async Task<Error> interactTryToReply(string query)
		{
			InteractReply = "";
			var jsonrpc = new JsonRpcClient();
			Parameters parameters = new Parameters();
			parameters.query = query;
			jsonrpc.SetParameters(parameters);
			if (await jsonrpc.SendRequest("interact::tryToReply"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				if (response != null)
					InteractReply = response.result;
			}

			return jsonrpc.Error;
		}

		public void LaunchDemo()
		{
			Updating = true;
			// Active le mode démo
			config.SetDemoMode();

			// Charger les données json
			Version = "Démo";
			LoadingMessage = "Chargement des Objets";
			var assembly = GetType().GetTypeInfo().Assembly;
			var stream = assembly.GetManifestResourceStream("Jeedom.Demo.object.json");
			var sr = new StreamReader(stream);
			ObjectList = JsonConvert.DeserializeObject<ObservableCollection<JeedomObject>>(sr.ReadToEnd());

			// Populate EqlogicList and CmdList
			EqLogicList.Clear();
			CommandList.Clear();
			//foreach (var obj in ObjectList)
			//{
			//	foreach (var eq in obj.EqLogics)
			//	{
			//		EqLogicList.Add(eq);
			//		foreach (var cmd in eq.Cmds)
			//		{
			//			CommandList.Add(cmd);
			//		}
			//	}
			//}

			LoadingMessage = "Chargement des Scénarios";
			stream = assembly.GetManifestResourceStream("Jeedom.Demo.scenario.json");
			sr = new StreamReader(stream);
			SceneList = JsonConvert.DeserializeObject<ObservableCollection<Scene>>(sr.ReadToEnd());

			LoadingMessage = "Chargement des Messages";
			stream = assembly.GetManifestResourceStream("Jeedom.Demo.message.json");
			sr = new StreamReader(stream);
			MessageList = JsonConvert.DeserializeObject<ObservableCollection<Message>>(sr.ReadToEnd());

			// Mise à jour des favoris
			PopulateFavoriteList();

			LoadingMessage = "Prêt";
			Updating = false;
		}

		public async Task<Error> PingJeedom()
		{
			Updating = true;
			LoadingMessage = "Contacte Jeedom";
			var jsonrpc = new JsonRpcClient();
			//LoadingMessage = "Tentative de connexion en https";
			config.Address.ProtocolType = Address.Protocol.Https;
			if (await jsonrpc.SendRequest("ping"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				if (response.result == "pong")
				{
					Updating = false;
					return null;
				}
			}

			//LoadingMessage = "Tentative de connexion en https en ignorant les erreurs de certificat";
			config.Address.ProtocolType = Address.Protocol.SelfSigned;
			if (await jsonrpc.SendRequest("ping"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				if (response.result == "pong")
				{
					Updating = false;
					return null;
				}
			}

			//LoadingMessage = "Tentative de connexion en http";
			config.Address.ProtocolType = Address.Protocol.Http;
			if (await jsonrpc.SendRequest("ping"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				if (response.result == "pong")
				{
					Updating = false;
					return null;
				}
			}
			// Par sécurité on retourne sur https
			config.Address.ProtocolType = Address.Protocol.Https;
			Updating = false;
			return jsonrpc.Error;
		}

		public void PopulateFavoriteList()
		{
			FavoriteList.Clear();
			_favoriteIdList = config.FavoriteList;
			foreach (string id in _favoriteIdList)
			{
				var idx = id.Substring(3);
				if (id.StartsWith("eq_"))
				{
					var lst = from e in EqLogicList where e.Id == idx select e;
					if (lst.Count() != 0)
					{
						var eq = lst.First();
						FavoriteList.Add(eq);
					}
					else
						_favoriteIdList.Remove(id);
				}
				else if (id.StartsWith("sc_"))
				{
					var lst = from s in SceneList where s.Id == idx select s;
					if (lst.Count() != 0)
					{
						var sc = lst.First();
						FavoriteList.Add(sc);
					}
					else
						_favoriteIdList.Remove(id);
				}
				else
				{
					_favoriteIdList.Remove(id);
				}
			}
			config.FavoriteList = _favoriteIdList;
		}

		public async Task<bool> Reboot()
		{
			if (config.IsDemoEnabled)
				return true;

			var jsonrpc = new JsonRpcClient();

			await jsonrpc.SendRequest("jeeNetwork::reboot");

			if (jsonrpc.Error == null)
				return true;
			else
				return false;
		}

		public void RemoveFromFavorite(Model.EqLogic eq)
		{
			FavoriteList.Remove(eq);
			_favoriteIdList.Remove("eq_" + eq.Id);
			config.FavoriteList = _favoriteIdList;
		}

		public void RemoveFromFavorite(Scene sc)
		{
			FavoriteList.Remove(sc);
			_favoriteIdList.Remove("sc_" + sc.Id);
			config.FavoriteList = _favoriteIdList;
		}

		public async Task RunScene(Scene scene)
		{
			if (config.IsDemoEnabled)
				return;

			var parameters = new Parameters();
			parameters.id = scene.Id;
			parameters.state = "run";
			var jsonrpc = new JsonRpcClient(parameters);

			if (await jsonrpc.SendRequest("scenario::changeState"))
			{
				await UpdateScene(scene);
			}
		}

		public async Task<Error> SearchConfigByKey(string key, string plugin)
		{
			Parameters parameters = new Parameters();
			parameters.key = key;
			parameters.plugin = plugin;
			var jsonrpc = new JsonRpcClient(parameters);

			if (await jsonrpc.SendRequest("config::byKey"))
			{
				configByKey = jsonrpc.GetRequestResponseDeserialized<Response<string>>().result;
			}

			return jsonrpc.Error;
		}

		public async Task<Error> SendNotificationUri(string uri)
		{
			Parameters parameters = new Parameters();
			parameters.plugin = "pushNotification";
			parameters.platform = "windows";
			parameters.query = uri;
			if (config.IdPush != null)
				parameters.id = config.IdPush;
			var jsonrpc = new JsonRpcClient(parameters);
			if (await jsonrpc.SendRequest("Iq"))
			{
				// Récupère la liste de tous les eqLogics
				var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
				if (reponse != null)
				{
					config.IdPush = reponse.result;
				}
			}

			return jsonrpc.Error;
		}

		public async Task<bool> SendPosition(string position)
		{
			//var httpRpcClient = new HttpRpcClient("/plugins/geoloc/core/api/jeeGeoloc.php?apikey=" + config.ApiKey + "&id=" + config.GeolocObjectId + "&value=" + position);
			var httpRpcClient = new HttpRpcClient("core/api/jeeApi.php?api=" + config.ApiKey + "&type=geoloc&id=" + config.GeolocObjectId + "&value=" + position);
			return await httpRpcClient.SendRequest();
		}

		public async Task<bool> Shutdown()
		{
			if (config.IsDemoEnabled)
				return true;

			var jsonrpc = new JsonRpcClient();

			await jsonrpc.SendRequest("jeeNetwork::halt");

			if (jsonrpc.Error == null)
				return true;
			else
				return false;
		}

		public async Task<Error> SynchMobilePlugin()
		{
			var jsonrpc = new JsonRpcClient();
			Parameters parameters = new Parameters();
			parameters.plugin = "mobile";
			jsonrpc.SetParameters(parameters);
			EqLogicList.Clear();
			CommandList.Clear();

			if (await jsonrpc.SendRequest("sync"))
			{
				// Récupère la liste de tous les eqLogics
				var EqLogics = jsonrpc.GetRequestResponseDeserialized<Response<JeedomObject>>();
				if (EqLogics != null)
				{
					foreach (Model.EqLogic eq in EqLogics.result.eqLogics)
					{
						EqLogicList.Add(eq);
					}
				}

				// Récupère la liste de toutes les cmds
				var Cmds = jsonrpc.GetRequestResponseDeserialized<Response<Model.EqLogic>>();
				if (Cmds.result.Cmds != null)
				{
					foreach (Model.Command cmd in Cmds.result.Cmds)
					{
						// AJoute la cmd à son eqLogic
						var eq = (from e in EqLogicList where e.Id == cmd.EqLogic_id select e).FirstOrDefault();
						if (eq.Cmds == null)
							eq.Cmds = new ObservableCollectionEx<Model.Command>();
						eq.Cmds.Add(cmd);

						/*if (EqLogicList.Where(o => o.Id.Equals(cmd.EqLogic_id)).FirstOrDefault().Cmds == null)
                            EqLogicList.Where(o => o.Id.Equals(cmd.EqLogic_id)).FirstOrDefault().Cmds = new ObservableCollectionEx<Command>();
                        EqLogicList.Where(o => o.Id.Equals(cmd.EqLogic_id)).FirstOrDefault().Cmds.Add(cmd);*/

						// Ajoute la commande à la liste globale des cmds
						CommandList.Add(cmd);
					}
				}

				// Suppression des eqLogics sans cmds
				for (int i = EqLogicList.Count - 1; i >= 0; i--)
				{
					if (EqLogicList[i].Cmds == null)
						EqLogicList.RemoveAt(i);
				}

				// Affecte les eqLogics à leurs objects correspondants
				foreach (EqLogic eq in EqLogicList)
				{
					var ob = (from o in ObjectList where o.id == eq.object_id select o).FirstOrDefault();

					if (ob.eqLogics == null)
						ob.eqLogics = new List<EqLogic>();
					ob.eqLogics.Add(eq);

				}

				// Suppression des objects sans eqLogics
				for (int i = ObjectList.Count - 1; i >= 0; i--)
				{
					//todo review eqlogics
					//if (ObjectList[i].EqLogics == null)
					//	ObjectList.RemoveAt(i);
				}

				// Mise à jour des favoris
				PopulateFavoriteList();
			}

			return jsonrpc.Error;
		}

		public async Task UpdateEqLogic(EqLogic eq)
		{
			if (eq.Cmds == null)
				return;

			var infoCmds = (from cmd in eq.Cmds where cmd.type == "info" select cmd).DefaultIfEmpty();
			if (infoCmds.Count() > 0)
			{
				foreach (Command cmd in infoCmds)
				{
					if (cmd != null)
					{
						if (!cmd.Updating)
						{
							await ExecuteCommand(cmd);
							cmd.DateTime = _dateTime;
						}
					}
				}
			}

			eq.DateTime = _dateTime;
		}

		public async void UpdateNotificationChannel()
		{
			//var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
			//await SendNotificationUri(channel.Uri.ToString());
		}

		/*public async Task UpdateObject(JdObject obj)
        {
            var parameters = new Parameters();
            parameters.object_id = obj.Id;
            var jsonrpc = new JsonRpcClient(parameters);

            if (await jsonrpc.SendRequest("eqLogic::byObjectId"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<EqLogic>>>();
                if (response.result != null)
                {
                    foreach (EqLogic eqnew in response.result)
                    {
                        var lst = from e in EqLogicList where e.Id == eqnew.Id select e;
                        if (lst.Count() != 0)
                        {
                            var eqold = lst.FirstOrDefault();
                            eqnew.Cmds = eqold.Cmds;
                            eqold = eqnew;
                        }
                        else
                        {
                            EqLogicList.Add(eqnew);
                            obj.EqLogics.Add(eqnew);
                        }
                        await UpdateEqLogic(eqnew);
                    }
                }
            }
        }*/

		public async Task UpdateObjectList()
		{
			var jsonrpc = new JsonRpcClient();

			if (await jsonrpc.SendRequest("object::all"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<JeedomObject>>>();
				foreach (JeedomObject obj in response.result)
				{
					var lst = from o in ObjectList where o.id == obj.id select o;
					if (lst.Count() != 0)
					{
						var ob = lst.FirstOrDefault();
						ob = obj;
					}
					else
						ObjectList.Add(obj);
				}
			}
		}

		public async Task UpdateTask()
		{
			// Vérifie qu'on ne se trouve pas en mode demo
			if (config.IsDemoEnabled)
				return;

			//Vérifier que l'API Key est dispo avant de lancer l'update
			if (!config.Populated)
				return;

			// SI on est déjà en mise à jour on sort
			if (Updating)
				return;

			Updating = true;

			if (ObjectList.Count == 0)
			{
				LoadingMessage = "Contacte Jeedom";
				await DownloadDateTime();
				LoadingMessage = "Chargement des Objets";
				await DownloadObjects();
			}
			else
			{
				LoadingMessage = "Chargements des evènements";
				await GetEventChanges();
			}

			if (pass % 15 == 14)
			{
				LoadingMessage = "Chargement des Messages";
				await DownloadMessages();
			}

			// Met à jour les favoris
			PopulateFavoriteList();

			LoadingMessage = "Prêt";
			Updating = false;
		}

		public async Task<bool> Upgrade()
		{
			if (config.IsDemoEnabled)
				return true;

			var jsonrpc = new JsonRpcClient();

			await jsonrpc.SendRequest("update::update");

			if (jsonrpc.Error == null)
				return true;
			else
				return false;
		}

		private async Task<Error> DownloadDateTime()
		{
			var jsonrpc = new JsonRpcClient();

			if (await jsonrpc.SendRequest("datetime"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<double>>();
				_dateTime = response.result;
			}

			return jsonrpc.Error;
		}

		/*public async Task<bool> SendNotificationUri(string uri)
        {
            var httpRpcClient = new HttpRpcClient("/plugins/pushNotification/php/updatUri.php?api=" + config.ApiKey + "&id=" + config.NotificationObjectId + "&uri=" + uri);

            return await httpRpcClient.SendRequest();
        }*/

		private async Task UpdateScene(Scene scene)
		{
			var parameters = new Parameters();
			parameters.id = scene.Id;
			var jsonrpc = new JsonRpcClient(parameters);

			if (await jsonrpc.SendRequest("scenario::byId"))
			{
				var response = jsonrpc.GetRequestResponseDeserialized<Response<Scene>>();
				scene.lastLaunch = response.result.lastLaunch;
			}
		}
	}
}