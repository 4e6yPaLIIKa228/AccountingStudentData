using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingStudentData.Connection
{
    internal class DBConnection
    {
        public static string myConn = $@"Data Source = {Saver.NameDB};Version=3;";
    }
}
