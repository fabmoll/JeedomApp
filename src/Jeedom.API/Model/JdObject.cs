using Jeedom.API.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Jeedom.API.Model
{
    [DataContract]
    public class JdObject : INotifyPropertyChanged
    {
        #region Private Fields

        [DataMember(Name = "display")]
        private DisplayInfo _Display;

        [DataMember(Name = "eqLogics")]
        private ObservableCollectionEx<EqLogic> _EqLogics;

        [DataMember(Name = "father_id")]
        private string _FatherId;

        [DataMember(Name = "id")]
        private string _Id;

        [DataMember(Name = "image")]
        private string _Image;

        [DataMember(Name = "isVisible")]
        private string _IsVisible;

        [DataMember(Name = "name")]
        private string _Name;

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string Count
        {
            get
            {
                if (EqLogics != null)
                {
                    string s;
                    switch (EqLogics.Count)
                    {
                        case 0:
                            s = "aucun équipement";
                            break;

                        case 1:
                            s = "1 équipement";
                            break;

                        default:
                            s = EqLogics.Count + " équipements";
                            break;
                    }
                    return s;
                }
                else
                    return "aucun équipement";
            }
        }

        public DisplayInfo Display
        {
            get
            {
                return _Display;
            }

            set
            {
                _Display = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollectionEx<EqLogic> EqLogics
        {
            get
            {
                return _EqLogics;
            }

            set
            {
                _EqLogics = value;
                NotifyPropertyChanged();
            }
        }

        public string FatherId
        {
            get
            {
                return _FatherId;
            }

            set
            {
                _FatherId = value;
                NotifyPropertyChanged();
            }
        }

        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                NotifyPropertyChanged();
            }
        }

        public string Image
        {
            get
            {
                return _Image;
            }

            set
            {
                _Image = value;
                NotifyPropertyChanged();
            }
        }

        public string IsVisible
        {
            get
            {
                return _IsVisible;
            }

            set
            {
                _IsVisible = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Public Properties

        #region Private Methods

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            //System.Diagnostics.Debug.WriteLine("object update : " + Name + " " + propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Private Methods
    }
}