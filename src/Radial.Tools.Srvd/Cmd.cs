using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Tools.Srvd
{
    /// <summary>
    /// CmdAction.
    /// </summary>
    enum CmdAction
    {
        /// <summary>
        /// The unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// The install.
        /// </summary>
        Install,
        /// <summary>
        /// The start.
        /// </summary>
        Start,
        /// <summary>
        /// The stop.
        /// </summary>
        Stop,
        /// <summary>
        /// The restart.
        /// </summary>
        Restart,
        /// <summary>
        /// The uninstall.
        /// </summary>
        Uninstall,
        /// <summary>
        /// The daemon.
        /// </summary>
        Daemon,
        /// <summary>
        /// The state.
        /// </summary>
        State,
        /// <summary>
        /// The help.
        /// </summary>
        Help
    }



    /// <summary>
    /// Cmd.
    /// </summary>
    class Cmd
    {
        public Cmd(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                if (args[0] == "-i")
                    Action = CmdAction.Install;
                if (args[0] == "-start")
                    Action = CmdAction.Start;
                if (args[0] == "-stop")
                    Action = CmdAction.Stop;
                if (args[0] == "-restart")
                    Action = CmdAction.Restart;
                if (args[0] == "-u")
                    Action = CmdAction.Uninstall;
                if (args[0] == "-d")
                    Action = CmdAction.Daemon;
                if (args[0] == "-state")
                    Action = CmdAction.State;
                if (args[0] == "-?")
                    Action = CmdAction.Help;

                string serviceArg = args.Where(o => o.StartsWith("--service=")).FirstOrDefault();
                string pathArg = args.Where(o => o.StartsWith("--path=")).FirstOrDefault();
                string argsArg = args.Where(o => o.StartsWith("--args=")).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(pathArg))
                    ExePath = pathArg.Trim().Remove(0, 7);

                if (!string.IsNullOrWhiteSpace(serviceArg))
                    ServiceName = serviceArg.Trim().Remove(0, 10);

                if (!string.IsNullOrWhiteSpace(argsArg))
                    Args = argsArg.Trim().Remove(0, 7);

                if (string.IsNullOrWhiteSpace(ServiceName))
                    throw new ArgumentException("missing service name (--service)");

                if (Action == CmdAction.Install && string.IsNullOrWhiteSpace(ExePath))
                    throw new ArgumentException("missing executable file path (--path)");
            }
        }
          

        public CmdAction Action { get; private set; }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; private set; }

        /// <summary>
        /// Gets the executable path.
        /// </summary>
        public string ExePath { get; private set; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public string Args { get; private set; }
    }

}
