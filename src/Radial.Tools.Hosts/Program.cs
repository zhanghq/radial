using Radial.Tools.Hosts.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Radial.Tools.Hosts
{
    static class Program
    {
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool createNew = false;
            Mutex m = new Mutex(true, Resources.MutexName, out createNew);

            if (createNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
                ResumeInstance();
        }

        private static void ResumeInstance()
        {
            Process cp = Process.GetCurrentProcess();
            List<Process> aps = new List<Process>();
            aps.AddRange(Process.GetProcessesByName(cp.ProcessName));
            aps.AddRange(Process.GetProcessesByName(cp.ProcessName + ".vshost"));

            foreach (Process p in aps)
            {
                var pfn = p.MainModule.FileName.Replace(".vshost", "");
                if (p.Id != cp.Id && pfn == cp.MainModule.FileName)
                {
                    ShowWindowAsync(p.MainWindowHandle, 1);//显示
                    SetForegroundWindow(p.MainWindowHandle);//当到最前端

                    break;
                }
            }
        }
    }
}
