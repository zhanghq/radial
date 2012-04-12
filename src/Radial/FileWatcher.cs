using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Radial
{
    /// <summary>
    /// Monitor file, and raise event while it changes. 
    /// </summary>
    public static class FileWatcher
    {
        /// <summary>
        /// Reload file delegate.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public delegate void ReloadHandler(string filePath);

        static Dictionary<string, ReloadHandler> S_File_Reload_Dict;

        static HashSet<string> S_Watched_Directory;

        static HashSet<string> S_ChangedFiles;

        static int S_TimeoutMillis = 2000;

        static object S_SyncRoot = new object();


        /// <summary>
        /// Initializes the <see cref="FileWatcher"/> class.
        /// </summary>
        static FileWatcher()
        {
            S_File_Reload_Dict = new Dictionary<string, ReloadHandler>();
            S_Watched_Directory = new HashSet<string>();
            S_ChangedFiles = new HashSet<string>();
        }


        /// <summary>
        /// Creates the monitor.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="reloadProcess">The reload process.</param>
        public static void CreateMonitor(string filePath, ReloadHandler reloadProcess)
        {
            Checker.Requires(!string.IsNullOrWhiteSpace(filePath), "file path can not be empty or null.");
            Checker.Requires(reloadProcess != null, "reload handler can not be null.");

            if (!File.Exists(filePath))
                return;

            lock (S_SyncRoot)
            {
                filePath = filePath.Trim().ToLower();
                string directory = Path.GetDirectoryName(filePath).ToLower();

                if (!S_Watched_Directory.Contains(directory))
                {
                    FileSystemWatcher watcher = new FileSystemWatcher(directory);
                    watcher.NotifyFilter = NotifyFilters.LastWrite;
                    watcher.Changed += new FileSystemEventHandler(watcher_Changed);

                    watcher.EnableRaisingEvents = true;

                    if (!S_File_Reload_Dict.ContainsKey(filePath))
                        S_File_Reload_Dict.Add(filePath, reloadProcess);
                }
            }
            
        }

        /// <summary>
        /// Manipulate file change event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.IO.FileSystemEventArgs"/> instance containing the event data.</param>
        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            lock (S_SyncRoot)
            {
                string filePath = e.FullPath.ToLower();

                if (!S_ChangedFiles.Contains(filePath))
                {
                    S_ChangedFiles.Add(filePath);

                    Timer timer=new Timer(TimerCallback,null,S_TimeoutMillis,Timeout.Infinite);
                }
            }
        }


        /// <summary>
        /// Timer callback.
        /// </summary>
        /// <param name="state">The state.</param>
        static void TimerCallback(object state)
        {
            string[] copyArray = null;

            lock (S_SyncRoot)
            {
                copyArray = new string[S_ChangedFiles.Count];
                S_ChangedFiles.CopyTo(copyArray);
                S_ChangedFiles.Clear();
            }

            foreach (string file in copyArray)
            {
                if (S_File_Reload_Dict.ContainsKey(file))
                {
                    S_File_Reload_Dict[file].BeginInvoke(file, null, null);
                    Logger.GetInstance("FileWatcher").Debug("file changed: {0}", file);
                }
            }
        }
    }
}
