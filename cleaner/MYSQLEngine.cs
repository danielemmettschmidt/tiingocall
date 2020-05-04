using System;
using System.Data;
using System.Runtime.InteropServices;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace cleaner_driver
{
    class MYSQLEngine
    {

        public static string timenow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }

        public static void Execute(EngineQuery EQ)
        {            
            string connStr =      "server="
                                + EQ.server 
                                + ";user="
                                + EQ.user 
                                + ";database="
                                + EQ.database
                                + ";port=3306;password"
                                + EQ.password;

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                
                MySqlCommand cmd = new MySqlCommand(EQ.query, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("Done.");
        }

    }


}
