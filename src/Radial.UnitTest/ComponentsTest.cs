using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;

namespace Radial.UnitTest
{
    [TestFixture]
    public class ComponentsTest
    {
        [Test]
        public void CCTestDemo()
        {
            IDictionary<string,object> param=new Dictionary<string,object>();
            param.Add("name","hello");

            CCTestDemo o = Components.Resolve<CCTestDemo>(param);
            Console.WriteLine(o.Name);
        }
    }

    public class CCTestDemo
    {

        public CCTestDemo(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
