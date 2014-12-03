using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Radial.Windows
{
    /// <summary>
    /// ServiceInstaller.
    /// </summary>
    public static class ServiceInstaller
    {
        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        private const int SERVICE_CONFIG_DESCRIPTION = 0x01;

        /// <summary>
        /// SERVICE_STATUS.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class SERVICE_STATUS
        {
            /// <summary>
            /// The dw service type.
            /// </summary>
            public int dwServiceType = 0;
            /// <summary>
            /// The dw current state.
            /// </summary>
            public ServiceState dwCurrentState = 0;
            /// <summary>
            /// The dw controls accepted.
            /// </summary>
            public int dwControlsAccepted = 0;
            /// <summary>
            /// The dw win32 exit code.
            /// </summary>
            public int dwWin32ExitCode = 0;
            /// <summary>
            /// The dw service specific exit code.
            /// </summary>
            public int dwServiceSpecificExitCode = 0;
            /// <summary>
            /// The dw check point.
            /// </summary>
            public int dwCheckPoint = 0;
            /// <summary>
            /// The dw wait hint.
            /// </summary>
            public int dwWaitHint = 0;
        }

        #region OpenSCManager
        /// <summary>
        /// Opens the sc manager.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        static extern IntPtr OpenSCManager(string machineName, string databaseName, ScmAccessRights dwDesiredAccess);
        #endregion

        #region OpenService
        /// <summary>
        /// Opens the service.
        /// </summary>
        /// <param name="hSCManager">The h sc manager.</param>
        /// <param name="lpServiceName">Name of the lp service.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, ServiceAccessRights dwDesiredAccess);
        #endregion

        #region CreateService
        /// <summary>
        /// Creates the service.
        /// </summary>
        /// <param name="hSCManager">The h sc manager.</param>
        /// <param name="lpServiceName">Name of the lp service.</param>
        /// <param name="lpDisplayName">Display name of the lp.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <param name="dwServiceType">Type of the dw service.</param>
        /// <param name="dwStartType">Start type of the dw.</param>
        /// <param name="dwErrorControl">The dw error control.</param>
        /// <param name="lpBinaryPathName">Name of the lp binary path.</param>
        /// <param name="lpLoadOrderGroup">The lp load order group.</param>
        /// <param name="lpdwTagId">The LPDW tag identifier.</param>
        /// <param name="lpDependencies">The lp dependencies.</param>
        /// <param name="lp">The lp.</param>
        /// <param name="lpPassword">The lp password.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName, ServiceAccessRights dwDesiredAccess, int dwServiceType, ServiceBootFlag dwStartType, ServiceError dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lp, string lpPassword);


        /// <summary>
        /// Changes the service config2.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <param name="dwInfoLevel">The dw information level.</param>
        /// <param name="lpInfo">The lp information.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ChangeServiceConfig2(
            IntPtr hService,
            int dwInfoLevel,
            ref string lpInfo);
        
        #endregion

        #region CloseServiceHandle
        /// <summary>
        /// Closes the service handle.
        /// </summary>
        /// <param name="hSCObject">The h sc object.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseServiceHandle(IntPtr hSCObject);
        #endregion

        #region QueryServiceStatus
        /// <summary>
        /// Queries the service status.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <param name="lpServiceStatus">The lp service status.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll")]
        private static extern int QueryServiceStatus(IntPtr hService, SERVICE_STATUS lpServiceStatus);
        #endregion

        #region DeleteService
        /// <summary>
        /// Deletes the service.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteService(IntPtr hService);
        #endregion

        #region ControlService
        /// <summary>
        /// Controls the service.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <param name="dwControl">The dw control.</param>
        /// <param name="lpServiceStatus">The lp service status.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll")]
        private static extern int ControlService(IntPtr hService, ServiceControl dwControl, SERVICE_STATUS lpServiceStatus);
        #endregion

        #region StartService
        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <param name="dwNumServiceArgs">The dw number service arguments.</param>
        /// <param name="lpServiceArgVectors">The lp service argument vectors.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);
        #endregion

        /// <summary>
        /// Uninstalls the specified service name.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <exception cref="System.ApplicationException">
        /// Service not installed.
        /// or
        /// Could not delete service  + Marshal.GetLastWin32Error()
        /// </exception>
        public static void Uninstall(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Service not installed.");

                try
                {
                    Stop(service);
                    if (!DeleteService(service))
                        throw new ApplicationException("Could not delete service " + Marshal.GetLastWin32Error());
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }


        /// <summary>
        /// Determines whether the specified service name is installed.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public static bool IsInstalled(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);

                if (service == IntPtr.Zero)
                    return false;

                CloseServiceHandle(service);
                return true;
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Installs services.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="description">The description.</param>
        /// <param name="exeFilePath">The executable file path.</param>
        /// <param name="bootFlag">The boot flag.</param>
        /// <exception cref="System.ApplicationException">Failed to install service.</exception>
        public static void Install(string serviceName, string displayName, string description, string exeFilePath, ServiceBootFlag bootFlag = ServiceBootFlag.AutoStart)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);

                if (service == IntPtr.Zero)
                    service = CreateService(scm, serviceName, displayName, ServiceAccessRights.AllAccess, SERVICE_WIN32_OWN_PROCESS, bootFlag, ServiceError.Normal, exeFilePath, null, IntPtr.Zero, null, null, null);

                if (service == IntPtr.Zero)
                    throw new ApplicationException("Failed to install service.");

                ChangeServiceConfig2(service, SERVICE_CONFIG_DESCRIPTION, ref description);
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <exception cref="System.ApplicationException">Could not open service.</exception>
        public static void Start(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");

                try
                {
                    Start(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <exception cref="System.ApplicationException">Could not open service.</exception>
        public static void Stop(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Stop);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");

                try
                {
                    Stop(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Restarts the specified service name.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public static void Restart(string serviceName)
        {
            Stop(serviceName);
            Start(serviceName);
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <exception cref="System.ApplicationException">Unable to start service</exception>
        private static void Start(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            StartService(service, 0, 0);
            var changedStatus = WaitForStatus(service, ServiceState.StartPending, ServiceState.Running);
            if (!changedStatus)
                throw new ApplicationException("Unable to start service");
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <exception cref="System.ApplicationException">Unable to stop service</exception>
        private static void Stop(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            ControlService(service, ServiceControl.Stop, status);
            var changedStatus = WaitForStatus(service, ServiceState.StopPending, ServiceState.Stopped);
            if (!changedStatus)
                throw new ApplicationException("Unable to stop service");
        }

        /// <summary>
        /// Gets the service status.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public static ServiceState GetStatus(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    return ServiceState.NotFound;

                try
                {
                    return GetStatus(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Gets the service status.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Failed to query service status.</exception>
        private static ServiceState GetStatus(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();

            if (QueryServiceStatus(service, status) == 0)
                throw new ApplicationException("Failed to query service status.");

            return status.dwCurrentState;
        }

        /// <summary>
        /// Waits for service status.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="waitStatus">The wait status.</param>
        /// <param name="desiredStatus">The desired status.</param>
        /// <returns></returns>
        private static bool WaitForStatus(IntPtr service, ServiceState waitStatus, ServiceState desiredStatus)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();

            QueryServiceStatus(service, status);
            if (status.dwCurrentState == desiredStatus) return true;

            int dwStartTickCount = Environment.TickCount;
            int dwOldCheckPoint = status.dwCheckPoint;

            while (status.dwCurrentState == waitStatus)
            {
                // Do not wait longer than the wait hint. A good interval is
                // one tenth the wait hint, but no less than 1 second and no
                // more than 10 seconds.

                int dwWaitTime = status.dwWaitHint / 10;

                if (dwWaitTime < 1000) dwWaitTime = 1000;
                else if (dwWaitTime > 10000) dwWaitTime = 10000;

                Thread.Sleep(dwWaitTime);

                // Check the status again.

                if (QueryServiceStatus(service, status) == 0) break;

                if (status.dwCheckPoint > dwOldCheckPoint)
                {
                    // The service is making progress.
                    dwStartTickCount = Environment.TickCount;
                    dwOldCheckPoint = status.dwCheckPoint;
                }
                else
                {
                    if (Environment.TickCount - dwStartTickCount > status.dwWaitHint)
                    {
                        // No progress made within the wait hint
                        break;
                    }
                }
            }
            return (status.dwCurrentState == desiredStatus);
        }

        /// <summary>
        /// Opens the sc manager.
        /// </summary>
        /// <param name="rights">The rights.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Could not connect to service control manager.</exception>
        private static IntPtr OpenSCManager(ScmAccessRights rights)
        {
            IntPtr scm = OpenSCManager(null, null, rights);
            if (scm == IntPtr.Zero)
                throw new ApplicationException("Could not connect to service control manager.");

            return scm;
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">The parameter value.</param>
        /// <param name="valueKind">Kind of the value.</param>
        public static void SetParameter(string serviceName, string paramName, object paramValue
            , RegistryValueKind valueKind = RegistryValueKind.ExpandString)
        {
            if (string.IsNullOrWhiteSpace(paramName))
                return;

            if (paramValue == null)
            {
                RemoveParameter(serviceName, paramName);
                return;
            }

            if (IsInstalled(serviceName))
            {
                var reg = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + serviceName, true);

                var preg = reg.OpenSubKey("Parameters", true);

                if (preg == null)
                    preg = reg.CreateSubKey("Parameters");

                preg.SetValue(paramName, paramValue, valueKind);

            }
        }

        /// <summary>
        /// Removes the parameter.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static void RemoveParameter(string serviceName, string paramName)
        {
            if (string.IsNullOrWhiteSpace(paramName))
                return;

            if (IsInstalled(serviceName))
            {
                var reg = Registry.LocalMachine.OpenSubKey(string.Format(@"SYSTEM\CurrentControlSet\Services\{0}\Parameters", serviceName), true);

                if (reg != null)
                    reg.DeleteValue(paramName);
            }
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns></returns>
        public static object GetParameter(string serviceName, string paramName)
        {
            if (!string.IsNullOrWhiteSpace(paramName)
                && IsInstalled(serviceName))
            {
                var reg = Registry.LocalMachine.OpenSubKey(string.Format(@"SYSTEM\CurrentControlSet\Services\{0}\Parameters", serviceName));

                if (reg != null)
                    return reg.GetValue(paramName);
            }

            return null;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns></returns>
        public static T GetParameter<T>(string serviceName, string paramName)
        {
            object val = GetParameter(serviceName, paramName);

            if (val == null)
                return default(T);

            return (T)val;
        }

    }


    /// <summary>
    /// ServiceState.
    /// </summary>
    public enum ServiceState
    {
        /// <summary>
        /// The unknown.
        /// </summary>
        Unknown = -1, // The state cannot be (has not been) retrieved.
        /// <summary>
        /// The not found.
        /// </summary>
        NotFound = 0, // The service is not known on the host server.
        /// <summary>
        /// The stopped.
        /// </summary>
        Stopped = 1,
        /// <summary>
        /// The start pending.
        /// </summary>
        StartPending = 2,
        /// <summary>
        /// The stop pending.
        /// </summary>
        StopPending = 3,
        /// <summary>
        /// The running.
        /// </summary>
        Running = 4,
        /// <summary>
        /// The continue pending.
        /// </summary>
        ContinuePending = 5,
        /// <summary>
        /// The pause pending.
        /// </summary>
        PausePending = 6,
        /// <summary>
        /// The paused.
        /// </summary>
        Paused = 7
    }

    /// <summary>
    /// ScmAccessRights.
    /// </summary>
    [Flags]
    public enum ScmAccessRights
    {
        /// <summary>
        /// The connect.
        /// </summary>
        Connect = 0x0001,
        /// <summary>
        /// The create service.
        /// </summary>
        CreateService = 0x0002,
        /// <summary>
        /// The enumerate service.
        /// </summary>
        EnumerateService = 0x0004,
        /// <summary>
        /// The lock.
        /// </summary>
        Lock = 0x0008,
        /// <summary>
        /// The query lock status.
        /// </summary>
        QueryLockStatus = 0x0010,
        /// <summary>
        /// The modify boot configuration.
        /// </summary>
        ModifyBootConfig = 0x0020,
        /// <summary>
        /// The standard rights required.
        /// </summary>
        StandardRightsRequired = 0xF0000,
        /// <summary>
        /// All access.
        /// </summary>
        AllAccess = (StandardRightsRequired | Connect | CreateService |
                     EnumerateService | Lock | QueryLockStatus | ModifyBootConfig)
    }

    /// <summary>
    /// ServiceAccessRights.
    /// </summary>
    [Flags]
    public enum ServiceAccessRights
    {
        /// <summary>
        /// The query configuration.
        /// </summary>
        QueryConfig = 0x1,
        /// <summary>
        /// The change configuration.
        /// </summary>
        ChangeConfig = 0x2,
        /// <summary>
        /// The query status.
        /// </summary>
        QueryStatus = 0x4,
        /// <summary>
        /// The enumerate dependants.
        /// </summary>
        EnumerateDependants = 0x8,
        /// <summary>
        /// The start.
        /// </summary>
        Start = 0x10,
        /// <summary>
        /// The stop.
        /// </summary>
        Stop = 0x20,
        /// <summary>
        /// The pause continue.
        /// </summary>
        PauseContinue = 0x40,
        /// <summary>
        /// The interrogate.
        /// </summary>
        Interrogate = 0x80,
        /// <summary>
        /// The user defined control.
        /// </summary>
        UserDefinedControl = 0x100,
        /// <summary>
        /// The delete.
        /// </summary>
        Delete = 0x00010000,
        /// <summary>
        /// The standard rights required.
        /// </summary>
        StandardRightsRequired = 0xF0000,
        /// <summary>
        /// All access.
        /// </summary>
        AllAccess = (StandardRightsRequired | QueryConfig | ChangeConfig |
                     QueryStatus | EnumerateDependants | Start | Stop | PauseContinue |
                     Interrogate | UserDefinedControl)
    }

    /// <summary>
    /// ServiceBootFlag.
    /// </summary>
    public enum ServiceBootFlag
    {
        /// <summary>
        /// The start.
        /// </summary>
        Start = 0x00000000,
        /// <summary>
        /// The system start.
        /// </summary>
        SystemStart = 0x00000001,
        /// <summary>
        /// The automatic start.
        /// </summary>
        AutoStart = 0x00000002,
        /// <summary>
        /// The demand start.
        /// </summary>
        DemandStart = 0x00000003,
        /// <summary>
        /// The disabled.
        /// </summary>
        Disabled = 0x00000004
    }

    /// <summary>
    /// ServiceControl.
    /// </summary>
    public enum ServiceControl
    {
        /// <summary>
        /// The stop.
        /// </summary>
        Stop = 0x00000001,
        /// <summary>
        /// The pause.
        /// </summary>
        Pause = 0x00000002,
        /// <summary>
        /// The continue.
        /// </summary>
        Continue = 0x00000003,
        /// <summary>
        /// The interrogate.
        /// </summary>
        Interrogate = 0x00000004,
        /// <summary>
        /// The shutdown.
        /// </summary>
        Shutdown = 0x00000005,
        /// <summary>
        /// The parameter change.
        /// </summary>
        ParamChange = 0x00000006,
        /// <summary>
        /// The net bind add.
        /// </summary>
        NetBindAdd = 0x00000007,
        /// <summary>
        /// The net bind remove.
        /// </summary>
        NetBindRemove = 0x00000008,
        /// <summary>
        /// The net bind enable.
        /// </summary>
        NetBindEnable = 0x00000009,
        /// <summary>
        /// The net bind disable.
        /// </summary>
        NetBindDisable = 0x0000000A
    }

    /// <summary>
    /// ServiceError.
    /// </summary>
    public enum ServiceError
    {
        /// <summary>
        /// The ignore.
        /// </summary>
        Ignore = 0x00000000,
        /// <summary>
        /// The normal.
        /// </summary>
        Normal = 0x00000001,
        /// <summary>
        /// The severe.
        /// </summary>
        Severe = 0x00000002,
        /// <summary>
        /// The critical.
        /// </summary>
        Critical = 0x00000003
    }
}
