using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Persist.Lite;

namespace PersistLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (
                DbSession session =
                    DbSession.NewSqlServerSession(ConfigurationManager.ConnectionStrings["default"].ConnectionString))
            {
                IList<RowDataCollection> rows = session.ExecuteRows("Select * From [HD_Exam_Question]");
                
            }
        }
    }
}
