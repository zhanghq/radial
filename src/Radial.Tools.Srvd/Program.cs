using System;
using System.IO;
using System.ServiceProcess;

namespace Radial.Tools.Srvd
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Cmd cmd = new Cmd(args);

                switch (cmd.Action)
                {
                    case CmdAction.Install: Install(cmd.ServiceName, cmd.ExePath, cmd.Args); break;
                    case CmdAction.Start: Start(cmd.ServiceName); break;
                    case CmdAction.Stop: Stop(cmd.ServiceName); break;
                    case CmdAction.Restart: Restart(cmd.ServiceName); break;
                    case CmdAction.Uninstall: Uninstall(cmd.ServiceName); break;
                    case CmdAction.Daemon: Daemon(cmd.ServiceName); break;
                    case CmdAction.State: State(cmd.ServiceName); break;
                    case CmdAction.Help: Help(); break;
                    default: Console.WriteLine("unknown command, type \"-?\" for help"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Install(string serviceName, string exePath, string args)
        {
            if (ServiceHelper.IsInstalled(serviceName))
            {
                Console.WriteLine("service {0} already installed", serviceName);
                return;
            }

            string daemonPath = string.Format("{0} -d --service={1}",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", null)),
                serviceName);

            ServiceHelper.Install(serviceName, serviceName, serviceName, daemonPath, ServiceBootFlag.AutoStart);

            if (exePath.StartsWith("%"))
                exePath = exePath.Replace("%", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));

            ServiceHelper.SetParameter(serviceName, "path", exePath);
            ServiceHelper.SetParameter(serviceName, "args", args);

            Console.WriteLine("service {0} install completed, waiting for start", serviceName);
        }

        private static void Start(string serviceName)
        {
            if (!ServiceHelper.IsInstalled(serviceName))
            {
                Console.WriteLine("service {0} not found", serviceName);
                return;
            }

            ServiceHelper.Start(serviceName);
            Console.WriteLine("service {0} started", serviceName);
        }

        private static void Stop(string serviceName)
        {
            if (!ServiceHelper.IsInstalled(serviceName))
            {
                Console.WriteLine("service {0} not found", serviceName);
                return;
            }

            ServiceHelper.Stop(serviceName);

            Console.WriteLine("service {0} stopped", serviceName);
        }

        private static void Restart(string serviceName)
        {
            if (!ServiceHelper.IsInstalled(serviceName))
            {
                Console.WriteLine("service {0} not found", serviceName);
                return;
            }

            ServiceHelper.Stop(serviceName);
            ServiceHelper.Start(serviceName);

            Console.WriteLine("service {0} restart ok", serviceName);
        }

        private static void Uninstall(string serviceName)
        {
            if (!ServiceHelper.IsInstalled(serviceName))
            {
                Console.WriteLine("service {0} not found", serviceName);
                return;
            }

            ServiceHelper.Uninstall(serviceName);

            Console.WriteLine("service {0} uninstall completed", serviceName);
        }

        private static void Daemon(string serviceName)
        {
            ServiceBase.Run(new ServiceBase[] { 
                new DaemonService(serviceName) 
            });
        }

        private static void State(string serviceName)
        {
            ServiceState status = ServiceHelper.GetStatus(serviceName);

            Console.WriteLine("service {0} was {1}", serviceName, status);
        }

        private static void Help()
        {
            Console.WriteLine("This program can be used to manipulate Windows services, and also can be used as a daemon");
            Console.WriteLine("");
            Console.WriteLine("======Command Action======");
            Console.WriteLine("-i  Install as a daemon");
            Console.WriteLine("-u  Uninstall service");
            Console.WriteLine("-start  Start service");
            Console.WriteLine("-stop  Stop service");
            Console.WriteLine("-restart  Stop service");
            Console.WriteLine("-state  Check service state");
            Console.WriteLine("-?        Show help");
            Console.WriteLine("");
            Console.WriteLine("======Command Args======");
            Console.WriteLine("--service  Service name");
            Console.WriteLine("--path      Executable file full path, or use % for current path");
            Console.WriteLine("--args      Arguments of executable file (optional)");
            Console.WriteLine("");
            Console.WriteLine("======Usage ======");
            Console.WriteLine("#Install as a daemon");
            Console.WriteLine("srvd.exe -i --service=xxxx --path=xxx --args=xxxx");
            Console.WriteLine(@"srvd.exe -i --service=xxxx --path=xxx --args=%\xxxx");
            Console.WriteLine("#Uninstall  service of the specified name");
            Console.WriteLine("srvd.exe -u --service=xxxx");
            Console.WriteLine("#Start service of the specified name");
            Console.WriteLine("srvd.exe -start --service=xxxx");
            Console.WriteLine("#Stop service of the specified name");
            Console.WriteLine("srvd.exe -stop --service=xxxx");
            Console.WriteLine("#Restart service of the specified name");
            Console.WriteLine("srvd.exe -restart --service=xxxx");
            Console.WriteLine("#Check service state");
            Console.WriteLine("srvd.exe -state --service=xxxx");
        }
    }
}
