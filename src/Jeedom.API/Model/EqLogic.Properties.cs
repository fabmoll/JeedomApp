using Jeedom.API.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jeedom.API.Model
{
    public partial class EqLogic
    {
        /// <summary>
        /// Liste des commandes de l'équipement
        /// </summary>
        public ObservableCollectionEx<Command> Cmds
        {
            get
            {
                return _Cmds;
            }

            set
            {
                _Cmds = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("VisibleCmds");
            }
        }

        public int ColSpan
        {
            get
            {
                var _visibleCmds = GetVisibleCmds().Count();
                if (_visibleCmds > 3)
                    return 6;
                else
                    return _visibleCmds * 2;
            }
        }

        /// <summary>
        /// Dernière modification de l'équipement
        /// </summary>
        public double DateTime
        {
            get
            {
                return _DateTime;
            }
            set
            {
                _DateTime = value;
                NotifyPropertyChanged();
            }
        }

        public EqLogicDisplay Display
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

        public String EqTypeName
        {
            get
            {
                return _EqTypeName;
            }
            set
            {
                _EqTypeName = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsEnable
        {
            get
            {
                return _IsEnable;
            }

            set
            {
                _IsEnable = Convert.ToBoolean(value);
            }
        }

        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }

            set
            {
                _IsVisible = Convert.ToBoolean(value);
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

        public string ObjectId
        {
            get
            {
                return _ObjectId;
            }
            set
            {
                _ObjectId = value;
                NotifyPropertyChanged();
            }
        }

        public int RowSpan
        {
            get
            {
                var _visibleCmds = GetVisibleCmds().Count();
                if (_visibleCmds > 3)
                    return (int)Math.Ceiling((double)_visibleCmds / 3) * 2;
                else
                    return 2;
            }
        }

        public bool Updating
        {
            get
            {
                return _Updating;
            }
            set
            {
                _Updating = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Command> VisibleCmds
        {
            get
            {
                return GetVisibleCmds();
            }
        }
    }
}