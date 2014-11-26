using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Tools.Srvd
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Cmd.Initial(args);

                switch (Cmd.Context.Action)
                {
                    case CmdAction.Install: Install(); break;
                    case CmdAction.Start: Start(); break;
                    case CmdAction.Stop: Stop(); break;
                    case CmdAction.Restart: Restart(); break;
                    case CmdAction.Uninstall: Uninstall(); break;
                    case CmdAction.Daemon: Daemon(); break;
                    case CmdAction.State: State(); break;
                    case CmdAction.Help: Help(); break;
                    default: Console.WriteLine("unknown command, type \"-?\" for help"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void Install()
        {
            if (ServiceHelper.IsInstalled(Cmd.Context.ServiceName))
            {
                Console.WriteLine("service {0} already installed", Cmd.Context.ServiceName);
                return;
            }

            string exePath = string.Format("{0} -d --path={1}",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", null)),
                Convert.ToBase64String(Encoding.UTF8.GetBytes(Cmd.Context.ExePath)));

            if (!string.IsNullOrWhiteSpace(Cmd.Context.Args))
                exePath += string.Format(" --args={0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(Cmd.Context.Args)));

            ServiceHelper.Install(Cmd.Context.ServiceName, null,
                "The deamon of " + (string.IsNullOrWhiteSpace(Cmd.Context.ServiceName) ? Path.GetFileNameWithoutExtension(Cmd.Context.ExePath) : Cmd.Context.ServiceName),
                exePath, ServiceBootFlag.AutoStart);

            Console.WriteLine("service {0} install completed, waiting for start", Cmd.Context.ServiceName);
        }

        private static void Start()
        {
            if (!ServiceHelper.IsInstalled(Cmd.Context.ServiceName))
            {
                Console.WriteLine("service {0} not found", Cmd.Context.ServiceName);
                return;
            }

            ServiceHelper.Start(Cmd.Context.ServiceName);
            Console.WriteLine("service {0} started", Cmd.Context.ServiceName);
        }

        private static void Stop()
        {
            if (!ServiceHelper.IsInstalled(Cmd.Context.ServiceName))
            {
                Console.WriteLine("service {0} not found", Cmd.Context.ServiceName);
                return;
            }

            ServiceHelper.Stop(Cmd.Context.ServiceName);

            Console.WriteLine("service {0} stopped", Cmd.Context.ServiceName);
        }

        private static void Restart()
        {
            if (!ServiceHelper.IsInstalled(Cmd.Context.ServiceName))
            {
                Console.WriteLine("service {0} not found", Cmd.Context.ServiceName);
                return;
            }

            ServiceHelper.Stop(Cmd.Context.ServiceName);
            ServiceHelper.Start(Cmd.Context.ServiceName);

            Console.WriteLine("service {0} restart ok", Cmd.Context.ServiceName);
        }

        private static void Uninstall()
        {
            if (!ServiceHelper.IsInstalled(Cmd.Context.ServiceName))
            {
                Console.WriteLine("service {0} not found", Cmd.Context.ServiceName);
                return;
            }

            ServiceHelper.Uninstall(Cmd.Context.ServiceName);

            Console.WriteLine("service {0} uninstall completed", Cmd.Context.ServiceName);
        }

        private static void Daemon()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new MainService() 
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void State()
        {
            ServiceState status = ServiceHelper.GetStatus(Cmd.Context.ServiceName);

            Console.WriteLine("service {0} was {1}", Cmd.Context.ServiceName, status);
        }

        private static void Help()
        {
            Cmd.WriteHelp(Console.Out);
        }
    }
}
