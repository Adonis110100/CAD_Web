using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CADWeb.SQL
{
    public class SQLConnect
    {
        private static SqlConnection conn = null;
        private static object Singleton_Lock = new object();

        public static SqlConnection GetConnection()
        {
            if (conn == null)
            {
                lock (Singleton_Lock)
                {
                    if (conn == null)
                    {
                        conn = new SqlConnection(@"server=ADONIS\SQLEXPRESS;database=CAD;uid=sa;pwd=123456");
                    }
                }
            }
            return conn;
        }
    }
}