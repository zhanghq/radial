using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;

namespace Radial
{
    /// <summary>
    /// Computer processor information.
    /// </summary>
    public class ProcessorInfo
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        public string Manufacturer { get; internal set; }

        /// <summary>
        /// Gets the architecture.
        /// </summary>
        public string Architecture { get; internal set; }

        /// <summary>
        /// Gets the processor quantity in computer.
        /// </summary>
        public uint Quantity { get; internal set; }

        /// <summary>
        /// Gets the physical cores in each processor.
        /// </summary>
        public uint PhysicalCores { get; internal set; }

        /// <summary>
        /// Gets the logical cores in each processor.
        /// </summary>
        public uint LogicalCores { get; internal set; }

        /// <summary>
        /// Gets the max clock speed in MHz.
        /// </summary>
        public uint MaxClockSpeed { get; internal set; }

        /// <summary>
        /// Gets the current clock speed in MegaHertz.
        /// </summary>
        public uint CurrentClockSpeed { get; internal set; }

        /// <summary>
        /// Gets the size of the L2 cache in kilobytes.
        /// </summary>
        public uint L2CacheSize { get; internal set; }
    }

    /// <summary>
    /// Computer memory statistics.
    /// </summary>
    public class MemoryStatistics
    {
        /// <summary>
        /// Gets the total physical memory in megabytes.
        /// </summary>
        public float TotalPhysical { get; internal set; }

        /// <summary>
        /// Gets the available physical memory in megabytes.
        /// </summary>
        public float AvailablePhysical { get; internal set; }

        /// <summary>
        /// Gets the total virtual memory.
        /// </summary>
        public float TotalVirtual { get; internal set; }

        /// <summary>
        /// Gets the available virtual memory.
        /// </summary>
        public float AvailableVirtual { get; internal set; }


        /// <summary>
        /// Gets the amount of physical memory mapped to the process context in megabytes.
        /// </summary>
        public float WorkingSet { get; internal set; }
    }

    /// <summary>
    /// Windows system information.
    /// </summary>
    public class WindowsOS
    {
        /// <summary>
        /// Gets the os name.
        /// </summary>
        public string OSName { get; internal set; }

        /// <summary>
        /// Gets the os version.
        /// </summary>
        public string OSVersion{get;internal set;}

        /// <summary>
        /// Gets the OS architecture.
        /// </summary>
        public string OSArchitecture { get; internal set; }

        /// <summary>
        /// Gets the name of the machine.
        /// </summary>
        public string MachineName { get; internal set; }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        public string Domain { get; internal set; }

        /// <summary>
        /// Gets the language.
        /// </summary>
        public string Language { get; internal set; }

        /// <summary>
        /// Gets the system directory.
        /// </summary>
        public string SystemDirectory { get; internal set; }

        /// <summary>
        /// Gets the windows directory.
        /// </summary>
        public string WindowsDirectory { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the operating system is a 64-bit operating system.
        /// </summary>
        public bool Is64Bit { get; internal set; }

        /// <summary>
        /// Gets the time zome.
        /// </summary>
        public TimeZone TimeZome { get; internal set; }

    }

    /// <summary>
    /// Static class used to obtain information of the local computer.
    /// </summary>
    public static class LocalComputer
    {

        #region Methods

        /// <summary>
        /// Gets the ManagementObject value.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="propertyName">the property name.</param>
        /// <returns>
        /// ManagementObject value.
        /// </returns>
        public static T GetObjectValue<T>(string path, string propertyName)
        {
            return (T)GetObjectValue(path, propertyName);
        }

        /// <summary>
        /// Gets the ManagementObject value.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="propertyName">the property name.</param>
        /// <returns>ManagementObject value.</returns>
        public static object GetObjectValue(string path, string propertyName)
        {
            object o = null;
            ManagementClass mc = new ManagementClass(path);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                o = mo[propertyName];
            }

            return o;
        }

        #endregion

        /// <summary>
        /// Gets the processor information.
        /// </summary>
        public static ProcessorInfo Processor
        {
            get
            {
                ProcessorInfo processor = new ProcessorInfo();

                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    processor.Id = mo["ProcessorId"] == null ? string.Empty : mo["ProcessorId"].ToString();
                    processor.Name = mo["Name"] == null ? string.Empty : mo["Name"].ToString();
                    processor.Manufacturer = mo["Manufacturer"] == null ? string.Empty : mo["Manufacturer"].ToString();

                    if (mo["Architecture"] != null)
                    {
                        ushort architectureValue = (ushort)mo["Architecture"];

                        switch (architectureValue)
                        {
                            case 0: processor.Architecture = "x86"; break;
                            case 1: processor.Architecture = "MIPS"; break;
                            case 2: processor.Architecture = "Alpha"; break;
                            case 3: processor.Architecture = "PowerPC"; break;
                            case 6: processor.Architecture = "Itanium-based systems"; break;
                            case 9: processor.Architecture = "x64"; break;
                            default: processor.Architecture = "Unknown"; break;
                        }
                    }
                    else
                        processor.Architecture = string.Empty;

                    processor.PhysicalCores = mo["NumberOfCores"] == null ? 0 : (uint)mo["NumberOfCores"];
                    processor.LogicalCores = mo["NumberOfLogicalProcessors"] == null ? 0 : (uint)mo["NumberOfLogicalProcessors"];

                    processor.MaxClockSpeed = mo["MaxClockSpeed"] == null ? 0 : (uint)mo["MaxClockSpeed"];

                    processor.L2CacheSize = mo["L2CacheSize"] == null ? 0 : (uint)mo["L2CacheSize"];

                    processor.CurrentClockSpeed = mo["CurrentClockSpeed"] == null ? 0 : (uint)mo["CurrentClockSpeed"];
                }

                object numberOfProcessors = GetObjectValue("Win32_ComputerSystem", "NumberOfProcessors");
                processor.Quantity = numberOfProcessors == null ? 0 : (uint)numberOfProcessors;

                return processor;
            }
        }


        /// <summary>
        /// Gets the memory statistics.
        /// </summary>
        public static MemoryStatistics Memory
        {
            get
            {
                MemoryStatistics memory = new MemoryStatistics();
                object totalPhysicalMemory = GetObjectValue("Win32_ComputerSystem", "TotalPhysicalMemory");
                memory.TotalPhysical = totalPhysicalMemory == null ? 0 : (ulong)totalPhysicalMemory / 1024f / 1024f;

                ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    memory.AvailablePhysical = mo["FreePhysicalMemory"] == null ? 0 : (ulong)mo["FreePhysicalMemory"] / 1024f;
                    memory.TotalVirtual = mo["TotalVirtualMemorySize"] == null ? 0 : (ulong)mo["TotalVirtualMemorySize"] / 1024f;
                    memory.AvailableVirtual = mo["FreeVirtualMemory"] == null ? 0 : (ulong)mo["FreeVirtualMemory"] / 1024f;
                }
                memory.WorkingSet = (ulong)System.Environment.WorkingSet / 1024f / 1024f;
                return memory;
            }
        }



        /// <summary>
        /// Gets the windows information.
        /// </summary>
        public static WindowsOS Windows
        {
            get
            {

                WindowsOS os = new WindowsOS();

                os.Language = System.Globalization.CultureInfo.InstalledUICulture.Name;

                os.Is64Bit = Environment.Is64BitOperatingSystem;

                ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    os.OSName = mo["Caption"] == null ? string.Empty : mo["Caption"].ToString();

                    os.OSVersion = Environment.OSVersion.Version.ToString(3);

                    if (mo["CSDVersion"] != null)
                        os.OSVersion += string.Format(" {0}", mo["CSDVersion"].ToString());
                    if(mo["BuildNumber"]!=null)
                        os.OSVersion += string.Format(" Build {0}", mo["BuildNumber"].ToString());

                    os.MachineName = Environment.MachineName;
                    os.SystemDirectory = mo["SystemDirectory"] == null ? string.Empty : mo["SystemDirectory"].ToString();
                    os.WindowsDirectory = mo["WindowsDirectory"] == null ? string.Empty : mo["WindowsDirectory"].ToString();
                    os.OSArchitecture = mo["OSArchitecture"] == null ? string.Empty : mo["OSArchitecture"].ToString();

                    os.TimeZome = TimeZone.CurrentTimeZone;
                }

                object domainObj = GetObjectValue("Win32_ComputerSystem", "Domain");
                os.Domain = domainObj == null ? string.Empty : domainObj.ToString();

                return os;
            }
        }

    }
}
