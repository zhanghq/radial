using QuickStart.Application;
using QuickStart.Startup;
using Radial.Boot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStart.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //add boot task
            Bootstrapper.RegisterTask(new SqlClientBootTask());
            //initialize
            Bootstrapper.Initialize();
            //start
            Bootstrapper.Start();

            //working
            DoWork();

            //stop
            Bootstrapper.Stop();
        }

        private static void DoWork()
        {
            var u = ServiceHub.User.Create("abc", "abc@sina.com");
            Console.WriteLine("User Id: {0}", u.Id);
        }
    }
}
