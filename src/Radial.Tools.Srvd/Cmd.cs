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
        /// <summary>
        /// The s_ context.
        /// </summary>
        private static Cmd S_Context;

        /// <summary>
        /// The service name suffix.
        /// </summary>
        const string ServiceNameSuffix = "Srvd";

        /// <summary>
        /// Prevents a default instance of the <see cref="Cmd"/> class from being created.
        /// </summary>
        private Cmd() { }

        /// <summary>
        /// Writes the help.
        /// </summary>
        /// <param name="tw">The text writer.</param>
        public static void WriteHelp(TextWriter tw)
        {
            tw.WriteLine("This program can be used to manipulate Windows services, and also can be used as a daemon");
            tw.WriteLine("");
            tw.WriteLine("======Command Action======");
            tw.WriteLine("-i  Install as a daemon");
            tw.WriteLine("-u  Uninstall service");
            tw.WriteLine("-start  Start service");
            tw.WriteLine("-stop  Stop service");
            tw.WriteLine("-restart  Stop service");
            tw.WriteLine("-state  Check service state");
            tw.WriteLine("-?        Show help");
            tw.WriteLine("");
            tw.WriteLine("======Command Args======");
            tw.WriteLine("--service  Service name");
            tw.WriteLine("--path      Executable file full path");
            tw.WriteLine("--args      Arguments of executable file");
            tw.WriteLine("");
            tw.WriteLine("======Usage ======");
            tw.WriteLine("#Install as a daemon using the specified service name");
            tw.WriteLine("srvd.exe -i --service=xxxx --path=xxx.exe --args=xxxxx");
            tw.WriteLine("#Install as a daemon using the auto service name");
            tw.WriteLine("srvd.exe -i --path=xxx.exe --args=xxxx");
            tw.WriteLine("#Uninstall  service of the specified name");
            tw.WriteLine("srvd.exe -u --service=xxxx");
            tw.WriteLine("#Start service of the specified name");
            tw.WriteLine("srvd.exe -start --service=xxxx");
            tw.WriteLine("#Stop service of the specified name");
            tw.WriteLine("srvd.exe -stop --service=xxxx");
            tw.WriteLine("#Restart service of the specified name");
            tw.WriteLine("srvd.exe -restart --service=xxxx");
            tw.WriteLine("#Check service state");
            tw.WriteLine("srvd.exe -state --service=xxxx");

        }

        public static void Initial(string[] args)
        {
            Cmd cmd = new Cmd();

            if (args != null && args.Length > 0)
            {
                if (args[0] == "-i")
                    cmd.Action = CmdAction.Install;
                if (args[0] == "-start")
                    cmd.Action = CmdAction.Start;
                if (args[0] == "-stop")
                    cmd.Action = CmdAction.Stop;
                if (args[0] == "-restart")
                    cmd.Action = CmdAction.Restart;
                if (args[0] == "-u")
                    cmd.Action = CmdAction.Uninstall;
                if (args[0] == "-d")
                    cmd.Action = CmdAction.Daemon;
                if (args[0] == "-state")
                    cmd.Action = CmdAction.State;
                if (args[0] == "-?")
                    cmd.Action = CmdAction.Help;

                string serviceArg = args.Where(o => o.StartsWith("--service=")).FirstOrDefault();
                string pathArg = args.Where(o => o.StartsWith("--path=")).FirstOrDefault();
                string argsArg = args.Where(o => o.StartsWith("--args=")).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(pathArg))
                    cmd.ExePath = pathArg.Trim().Remove(0, 7);

                if (string.IsNullOrWhiteSpace(serviceArg))
                {
                    if (cmd.Action == CmdAction.Install)
                        cmd.ServiceName = Path.GetFileNameWithoutExtension(cmd.ExePath) + ServiceNameSuffix;
                }
                else
                    cmd.ServiceName = serviceArg.Trim().Remove(0, 10);

                if (!string.IsNullOrWhiteSpace(argsArg))
                    cmd.Args = argsArg.Trim().Remove(0, 7);

                if (cmd.Action == CmdAction.Install || cmd.Action == CmdAction.Daemon)
                    Checker.Requires(!string.IsNullOrWhiteSpace(cmd.ExePath), "missing executable file path (--path)");

                if (cmd.Action == CmdAction.Start || cmd.Action == CmdAction.Stop || cmd.Action == CmdAction.Restart
                    || cmd.Action == CmdAction.Uninstall || cmd.Action == CmdAction.State)
                    Checker.Requires(!string.IsNullOrWhiteSpace(cmd.ServiceName), "missing service name (--service)");
            }

            S_Context = cmd;
        }
           
        /// <summary>
        /// Gets the cmd context.
        /// </summary>
        public static Cmd Context
        {
            get
            {
                return S_Context;
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
