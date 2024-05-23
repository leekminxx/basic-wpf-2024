using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openApi_min.Helpers
{
    internal class Common
    {
        public static readonly string CONNSTRING = "Data Source=127.0.0.1;" +
                                                    "Initial Catalog=DagueFood;" +
                                                    "Persist Security Info=True;" +
                                                    "User ID=sa;" +
                                                    "Encrypt=False;" +
                                                    "Password=mssql_p@ss";
        public static string Index;
    }
}

