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
        /// The uninstall.
        /// </summary>
        Uninstall,
        /// <summary>
        /// The run.
        /// </summary>
        Run,
        /// <summary>
        /// The state.
        /// </summary>
        State
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

        public static void Initial(string[] args)
        {
            Cmd cmd = new Cmd();

            //srvd.exe -i --service=xxxx --path=xxx.exe --args=xxxxx
            //srvd.exe -i --path=xxx.exe --args=xxxx
            //srvd.exe -start --service=xxxx
            //srvd.exe -stop --service=xxxx
            //srvd.exe -u --service=xxxx
            //srvd.exe -r --path=xxx.exe --args=xxxxx
            //srvd.exe -state --service=xxxx

            if (args != null && args.Length > 0)
            {
                if (args[0] == "-i")
                    cmd.Action = CmdAction.Install;
                if (args[0] == "-start")
                    cmd.Action = CmdAction.Start;
                if (args[0] == "-stop")
                    cmd.Action = CmdAction.Stop;
                if (args[0] == "-u")
                    cmd.Action = CmdAction.Uninstall;
                if (args[0] == "-r")
                    cmd.Action = CmdAction.Run;
                if (args[0] == "-state")
                    cmd.Action = CmdAction.State;

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

                if (cmd.Action == CmdAction.Install || cmd.Action == CmdAction.Run)
                    Checker.Requires(!string.IsNullOrWhiteSpace(cmd.ExePath), "missing executable file path (--path)");

                if (cmd.Action == CmdAction.Start || cmd.Action == CmdAction.Stop ||
                    cmd.Action == CmdAction.Uninstall || cmd.Action == CmdAction.State)
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
