using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GildedRose
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Check DB - faster than migrations
            string sqlConnectionString = @"Server=(localdb)\mssqllocaldb;Database=master;Trusted_Connection=True;ConnectRetryCount=0;";

            var checkSql = "DECLARE @dbname nvarchar(128); SET @dbname = N'GildedRoseCRB';IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))  SELECT 1 AS InIt ELSE  SELECT 0 AS InIt";

            SqlConnection sqlConnection1 = new SqlConnection(sqlConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = checkSql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            string isItThere = "1";

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    isItThere = rdr["InIt"].ToString();
                }
            }

            sqlConnection1.Close();

            if (isItThere == "0")
            {
                SqlConnection conn = new SqlConnection(sqlConnectionString);
                string rooter = HttpContext.Current.Server.MapPath("~");
                string script = File.ReadAllText(rooter + "\\App_Data\\GildedRoseV2.sql");
                ExecuteBatchNonQuery(script, conn);
            }
        }
        private void ExecuteBatchNonQuery(string sql, SqlConnection conn)
        {
            string sqlBatch = string.Empty;
            SqlCommand cmd = new SqlCommand(string.Empty, conn);
            conn.Open();
            sql += "\nGO";   // make sure last batch is executed.
            try
            {
                foreach (string line in sql.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.ToUpperInvariant().Trim() == "GO")
                    {
                        if (sqlBatch != string.Empty)
                        {
                            cmd.CommandText = sqlBatch;
                            cmd.ExecuteNonQuery();
                        }
                        sqlBatch = string.Empty;
                    }
                    else
                    {
                        sqlBatch += line + "\n";
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
