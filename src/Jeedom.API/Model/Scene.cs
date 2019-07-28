namespace Jeedom.API.Model
{
    public class Scene : JdItem
    {
        public string description;
        public string isActive;
        public string isVisible;
        public string lastLaunch;
        public string name;
        public string object_id;
        public string state;
        public string timeout;
        private bool _updating = false;

        public string LastLaunch
        {
            get
            {
                return lastLaunch;
            }

            set
            {
                lastLaunch = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        public bool Updating
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
    }
}