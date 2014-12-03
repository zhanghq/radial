using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Radial.Tools.Hbm2Sql.TSqlFormatter.Interfaces
{
    interface IParseTree
    {
        XmlDocument ToXmlDoc();
    }
}
