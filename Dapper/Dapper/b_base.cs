using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace Dapper
{
    public class b_base
    {
        public IDbConnection Connection = null;
        public b_base()
        {
            Connection = new SqlConnection(ConfigurationManager.AppSettings["dapper"]);       
        }
    }
}
