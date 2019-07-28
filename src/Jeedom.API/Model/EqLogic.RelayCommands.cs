using Jeedom.API.Mvvm;
using System;
using System.Linq;

namespace Jeedom.API.Model
{
    public partial class EqLogic
    {
        /// <summary>
        /// Exécute une commande à partir de son "logicalId"
        /// </summary>
        public RelayCommand<object> ExecCommandByLogicalID
        {
            get
            {
                _ExecCommandByLogicalID = _ExecCommandByLogicalID ?? new RelayCommand<object>(async parameters =>
                {
                    var cmd = Cmds.Where(c => c.LogicalId.ToLower() == parameters.ToString().ToLower()).FirstOrDefault();
                    if (cmd != null)
                        await ExecCommand(cmd);
                });
                return _ExecCommandByLogicalID;
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "name"
        /// </summary>
        public RelayCommand<object> ExecCommandByName
        {
            get
            {
                _ExecCommandByName = _ExecCommandByName ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        var cmd = Cmds.Where(c => c.Name.ToLower() == parameters.ToString().ToLower()).FirstOrDefault();
                        if (cmd != null)
                            await ExecCommand(cmd);
                    }
                    catch (Exception) { }
                });
                return _ExecCommandByName;
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "generic_type"
        /// </summary>
        public RelayCommand<object> ExecCommandByType
        {
            get
            {
                _ExecCommandByType = _ExecCommandByType ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        var cmd = Cmds.Where(c => c.Display.generic_type == parameters.ToString()).FirstOrDefault();
                        if (cmd != null)
                            await ExecCommand(cmd);
                    }
                    catch (Exception) { }
                });
                return _ExecCommandByType;
            }
        }
    }
}