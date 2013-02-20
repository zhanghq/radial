using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class ToolkitsTest
    {
        [Test]
        public void ExcelToDataSet()
        {
            Assert.DoesNotThrow(() => Toolkits.ExcelToDataSet("data.xlsx"));
        }

        [Test]
        public void ExcelToDataTableIndex()
        {
            Assert.DoesNotThrow(() =>
            {
                DataTable dt = Toolkits.ExcelToDataTable("data.xlsx", 0);
            });
        }

        [Test]
        public void ExcelToDataTableName()
        {
            Assert.DoesNotThrow(() =>
            {
                DataTable dt = Toolkits.ExcelToDataTable("data.xlsx", "Sheet1");
            });
        }
    }
}
