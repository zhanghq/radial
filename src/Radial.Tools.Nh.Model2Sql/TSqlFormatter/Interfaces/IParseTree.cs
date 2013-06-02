using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Radial.Tools.Nh.Model2Sql.TSqlFormatter.Interfaces
{
    interface IParseTree
    {
        XmlDocument ToXmlDoc();
    }
}
