using Microsoft.Practices.Unity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest
{

    [RegisterInterface(typeof(Intface1))]
    class Impl1 : Intface1
    {
        int _a;
        public Impl1(int a)
        {
            _a = a;
        }

        public void Do()
        {
            Console.WriteLine("impl1: {0}", _a);
        }
    }

    interface Intface1{
        void Do();
    }

    [RegisterInterface(typeof(Intface2))]
    class Impl2 : Intface2
    {
        int _a;
        public Impl2(int a)
        {
            _a = a;
        }

        public void Do()
        {
            Console.WriteLine("impl2: {0}", _a);
        }
    }

    interface Intface2
    {
        void Do();
    }

    [TestFixture]
    public class ComponentTest
    {
        [Test]
        public void RegisterInterfaces()
        {
            var p = new Microsoft.Practices.Unity.InjectionConstructor(123);

            Components.RegisterInterfaces(new Type[] { typeof(Impl1), typeof(Impl2) }, null,
                () => { return new ContainerControlledLifetimeManager(); }
                , p);

            var obj= Components.Container.Resolve<Intface2>();
            obj.Do();
        }
    }
}
