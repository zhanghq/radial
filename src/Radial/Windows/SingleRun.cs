using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Radial.Windows
{
    /// <summary>
    /// Tools to run app in singleton model.
    /// </summary>
    public static class SingleRun
    {

        static string Vshost = ".vshost";


        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Look app run in singleton model.
        /// </summary>
        /// <returns>If no other instance is running return true, otherwise return false.</returns>
        public static bool Lock()
        {
            return Lock(null);
        }

        /// <summary>
        /// Look app run in singleton model.
        /// </summary>
        /// <param name="mutexName">The mutex name.</param>
        /// <returns>If no other instance is running return true, otherwise return false.</returns>
        public static bool Lock(string mutexName)
        {
            if (string.IsNullOrWhiteSpace(mutexName))
                mutexName = Radial.Security.CryptoProvider.SHA1Encrypt(Process.GetCurrentProcess().MainModule.FileName.Replace(Vshost, null));

            bool createNew = false;

            Mutex m = new Mutex(true, mutexName, out createNew);

            return createNew;
        }

        /// <summary>
        /// Unlock app.
        /// </summary>
        public static void Unlock()
        {
            Unlock(null);
        }

        /// <summary>
        /// Unlock app.
        /// </summary>
        /// <param name="mutexName">The mutex name</param>
        public static void Unlock(string mutexName)
        {
            if (string.IsNullOrWhiteSpace(mutexName))
                mutexName = Radial.Security.CryptoProvider.SHA1Encrypt(Process.GetCurrentProcess().MainModule.FileName.Replace(Vshost, null));

            Mutex result;

            if (Mutex.TryOpenExisting(mutexName, out result))
            {
                result.ReleaseMutex();
            }
        }

        /// <summary>
        /// Resume running instance and set it to foreground.
        /// </summary>
        public static void Resume()
        {
            Process cp = Process.GetCurrentProcess();
            List<Process> aps = new List<Process>();
            aps.AddRange(Process.GetProcessesByName(cp.ProcessName));
            aps.AddRange(Process.GetProcessesByName(cp.ProcessName + Vshost));

            foreach (Process p in aps)
            {
                var pfn = p.MainModule.FileName.Replace(Vshost, null);
                if (p.Id != cp.Id && pfn == cp.MainModule.FileName)
                {
                    ShowWindowAsync(p.MainWindowHandle, 1);
                    SetForegroundWindow(p.MainWindowHandle);
                    break;
                }
            }
        }
    }
}
