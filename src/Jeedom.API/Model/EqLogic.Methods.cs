using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Jeedom.API.Model
{
    public partial class EqLogic
    {
        public ObservableCollection<Command> GetActionsCmds()
        {
            if (Cmds != null)
            {
                IEnumerable<Command> results = Cmds.Where(c => c.Type == "action");
                return new ObservableCollection<Command>(results);
            }
            else
                return new ObservableCollection<Command>();
        }

        public ObservableCollection<Command> GetInformationsCmds()
        {
            if (Cmds != null)
            {
                IEnumerable<Command> results = Cmds.Where(c => c.Type == "info");
                return new ObservableCollection<Command>(results);
            }
            else
                return new ObservableCollection<Command>();
        }

        public ObservableCollection<Command> GetVisibleCmds()
        {
            if (Cmds != null)
            {
                IEnumerable<Command> results = Cmds.Where(c => c.IsVisible == true && c.LogicalId != null);
                return new ObservableCollection<Command>(results);
            }
            else
                return new ObservableCollection<Command>();
        }

        private async Task ExecCommand(Command cmd)
        {
            if (cmd != null)
            {
                Updating = true;
                await RequestViewModel.Instance.ExecuteCommand(cmd);
                //await Task.Delay(TimeSpan.FromSeconds(3));
                await RequestViewModel.Instance.UpdateEqLogic(this);
                NotifyPropertyChanged("Cmds");
                Updating = false;
            }
        }
    }
}