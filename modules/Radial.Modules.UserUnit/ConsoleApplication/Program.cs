using Radial;
using Radial.Modules.UserUnit;
using Radial.Persist;
using Radial.Persist.Nhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Components.Container.RegisterType<IFactoryPoolInitializer, FactoryPoolInitializer>(new ContainerControlledLifetimeManager());
            Components.Container.RegisterType<IUnitOfWork, ContextualUnitOfWork>();

            HibernateEngine.OpenAndBindSession();

            UserService us = new UserService();
            var result = us.Register(RandomCode.Create(8) + "@abc.com", null, null, "123456", "127.0.0.1");
            Console.WriteLine(result.Code);
            if (result.Code == RegisterResultCode.OK)
            {
                Console.WriteLine(result.User.RegisterTime);
                us.Delete(result.User.Id);
            }

            HibernateEngine.CloseAndUnbindSession();
        }
    }
}
