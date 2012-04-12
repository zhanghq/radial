using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration.Install;
using System.Diagnostics;
using Radial.Lock.WinService.Properties;

namespace Radial.Lock.WinService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string [] args)
        {
            Logger.Start();

            //only one argument
            string arg = args == null || args.Length == 0 ? string.Empty : args[0];

            if (string.IsNullOrWhiteSpace(arg) || string.Compare(arg, "-help", true) == 0)
            {
                Console.WriteLine("available arguments:");
                Console.WriteLine(" -i: install windows service");
                Console.WriteLine(" -u: unstall windows service");
                Console.WriteLine(" -help: arguments help");
                return;
            }

            try
            {

                if (string.Compare(arg, "-r", true) == 0)
                {
                    //run

                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[] 
			        { 
				        new WinHostService() 
			        };

                    ServiceBase.Run(ServicesToRun);
                }



                if (string.Compare(arg, "-i", true) == 0)
                {
                    //install
                    string serviceLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceLocation, new string[] { });
                    assemblyInstaller.Install(null);
                    assemblyInstaller.Commit(null);

                    Console.WriteLine(RunCmd(string.Format("sc config {0} binpath= \"{1}\"", Resources.ServiceName, serviceLocation + " -r")));
                }

                if (string.Compare(arg, "-u", true) == 0)
                {
                    //uninstall
                    Console.WriteLine(RunCmd(string.Format("sc delete {0}", Resources.ServiceName)));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static string RunCmd(string cmd)
        {
            Process prc = new Process();
            prc.StartInfo.FileName = "cmd.exe";
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.RedirectStandardInput = true;
            prc.StartInfo.RedirectStandardOutput = true;
            prc.StartInfo.RedirectStandardError = true;
            prc.StartInfo.CreateNoWindow = true;
            prc.Start();
            prc.StandardInput.WriteLine(cmd);
            prc.StandardInput.Close();

            string txt = prc.StandardOutput.ReadToEnd().Trim();
            return txt.Substring(txt.IndexOf("[SC]"), txt.LastIndexOf('\n') - txt.IndexOf("[SC]")).Trim();
        }
    }
}
