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

        public void Do1()
        {
            Console.WriteLine("impl1: {0}", _a);
        }
    }

    interface Intface1{
        void Do1();
    }

    [RegisterInterface(typeof(Intface2))]
    class Impl2 : Intface2
    {
        int _a;
        public Impl2(int a)
        {
            _a = a;
        }

        public void Do2()
        {
            Console.WriteLine("impl2: {0}", _a);
        }
    }

    interface Intface2
    {
        void Do2();
    }


    [RegisterInterface(typeof(Intface1))]
    [RegisterInterface(typeof(Intface2))]
    class Impl3 : Intface1, Intface2
    {
        int _a;
        public Impl3(int a)
        {
            _a = a;
        }

        public void Do1()
        {
            Console.WriteLine("impl3_1: {0}", _a);
        }

        public void Do2()
        {
            Console.WriteLine("impl3_2: {0}", _a);
        }
    }

    [TestFixture]
    public class ComponentTest
    {
        [Test]
        public void RegisterInterfaces()
        {
            var p = new Microsoft.Practices.Unity.InjectionConstructor(123);

            Components.RegisterInterfaces(new Type[] { typeof(Impl2), typeof(Impl3) }, null,
                () => { return new ContainerControlledLifetimeManager(); }
                , p);

            var obj = Components.Container.Resolve<Intface2>();
            obj.Do2();
        }
    }
}
