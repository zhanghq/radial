using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class ExcelToolsTest
    {
        [Test]
        public void ImportToDataTable()
        {
            ExcelTools.ImportToDataTable(@"C:\Users\Rick\Desktop\Demo.xlsx",0, null,false);
        }
    }
}
