using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace FPJ.DAL
{
    public class Conn
    {
        public static IDbConnection GetConn(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
