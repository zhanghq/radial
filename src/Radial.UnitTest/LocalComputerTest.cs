using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class LocalComputerTest
    {

        [Test]
        public void Memory()
        {
            MemoryStatistics m = LocalComputer.Memory;
            Console.WriteLine("Memory:");
            Console.WriteLine("TotalPhysical:{0}", m.TotalPhysical);
            Console.WriteLine("AvailablePhysical:{0}", m.AvailablePhysical);
            Console.WriteLine("TotalVirtual:{0}", m.TotalVirtual);
            Console.WriteLine("AvailableVirtual:{0}", m.AvailableVirtual);
            Console.WriteLine("WorkingSet:{0}", m.WorkingSet);
        }

        [Test]
        public void Processor()
        {
            ProcessorInfo p = LocalComputer.Processor;
            Console.WriteLine("Processor:");
            Console.WriteLine("Id:{0}", p.Id);
            Console.WriteLine("Name:{0}", p.Name);
            Console.WriteLine("Manufacturer:{0}", p.Manufacturer);
            Console.WriteLine("Architecture:{0}", p.Architecture);
            Console.WriteLine("Quantity:{0}", p.Quantity);
            Console.WriteLine("PhysicalCores:{0}", p.PhysicalCores);
            Console.WriteLine("LogicalCores:{0}", p.LogicalCores);
            Console.WriteLine("MaxClockSpeed:{0}", p.MaxClockSpeed);
            Console.WriteLine("CurrentClockSpeed:{0}", p.CurrentClockSpeed);
            Console.WriteLine("L2CacheSize:{0}", p.L2CacheSize);
        }

        [Test]
        public void Windows()
        {
            WindowsOS os = LocalComputer.Windows;
            Console.WriteLine("Windows:");
            Console.WriteLine("OSName:{0}", os.OSName);
            Console.WriteLine("OSVersion:{0}", os.OSVersion);
            Console.WriteLine("OSArchitecture:{0}", os.OSArchitecture);
            Console.WriteLine("MachineName:{0}", os.MachineName);
            Console.WriteLine("Language:{0}", os.Language);
            Console.WriteLine("Is64Bit:{0}", os.Is64Bit);
            Console.WriteLine("SystemDirectory:{0}", os.SystemDirectory);
            Console.WriteLine("WindowsDirectory:{0}", os.WindowsDirectory);
            Console.WriteLine("Domain:{0}", os.Domain);
            
        }
    }
}
