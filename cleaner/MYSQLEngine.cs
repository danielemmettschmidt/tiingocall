using System;
using System.Data;
using System.Runtime.InteropServices;
using cleaner;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace cleaner_driver
{
    class MYSQLEngine
    {
        
        // STATIC BASIC FUNCTIONS

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
                                + ";port=3306;password="
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

        public static void WriteManifest(Parser parser, in EngineQuery eq)
        {

            // write old table to archive




            // drop old table

            eq.query = "DELETE FROM `stockplanner`.`manifest`;";

            Execute(eq);


            // write new table

            foreach (ManifestValue mv in parser.manifestvalues.values)
            {
                eq.query = "INSERT INTO `stockplanner`.`manifest` (`stock`, `target_percentage`, `write_date`) VALUES ('" +
                            mv.stock +
                            "'," +
                            mv.targetpercentage +
                            ",'" +
                            timenow() +
                            "');";

                Execute(eq);
            }


        }

        public static void WriteSource(Parser parser, in EngineQuery eq)
        {

            // write old table to archive




            // drop old table

            eq.query = "DELETE FROM `stockplanner`.`source`;";

            Execute(eq);


            // write new table

            foreach (CSVValue csvv in parser.readvalues.values)
            {
                eq.query = "INSERT INTO `stockplanner`.`source` (`stock`, `current_value`, `quantity`, `write_date`) VALUES ('" +
                            csvv.stock +
                            "'," +
                            csvv.current_value +
                            "," +
                            csvv.quantity +
                            ",'" +
                            timenow() +
                            "');";

                Execute(eq);
            }


        }

    }


}
