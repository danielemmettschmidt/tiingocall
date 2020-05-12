using System;
using System.Collections.Generic;
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

        public static List<List<string>> Execute(EngineQuery EQ)
        {
            string connStr = "server="
                                + EQ.server
                                + ";user="
                                + EQ.user
                                + ";database="
                                + EQ.database
                                + ";port=3306;password="
                                + EQ.password;

            MySqlConnection conn = new MySqlConnection(connStr);

            List<List<string>> ret = new List<List<string>>();

            try
            {
                conn.Open();


                MySqlCommand cmd = new MySqlCommand(EQ.query, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();



                if (EQ.query.ToLower().Contains("select"))
                {
                    while (rdr.Read())
                    {
                        List<string> tackon = new List<string>();

                        short ii = 0;

                        while (ii < rdr.FieldCount)
                        {
                            tackon.Add(rdr[ii].ToString());
                            ii++;
                        }

                        ret.Add(tackon);
                    }
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return ret;
        }

        public static void WriteManifest(Parser parser)
        {

            // write old table to archive




            // drop old table

            parser.eq.query = "DELETE FROM `stockplanner`.`manifest`;";

            Execute(parser.eq);

            // write new table

            foreach (ManifestValue mv in parser.manifestvalues.values)
            {
                parser.eq.query = "INSERT INTO `stockplanner`.`manifest` (`stock`, `target_percentage`, `write_date`) VALUES ('" +
                            mv.stock +
                            "'," +
                            mv.targetpercentage +
                            ",'" +
                            timenow() +
                            "');";

                Execute(parser.eq);
            }

            parser.eq.query = "";
        }

        public static void WriteYCA(EngineQuery eq, string amnt)
        {
            eq.query = "INSERT INTO `stockplanner`.`yearly_contribution` (`write_date`, `yearly_contribution_amount`) VALUES(" +
                        "'" + timenow() + "'," +
                        amnt +
                        ");"
                        ;

            Execute(eq);

            eq.query = "";

        }

        public static void WriteSource(Parser parser)
        {

            // write old table to archive

            ArchiveSource(parser.readvalues, parser.eq);

            // drop old table

            parser.eq.query = "DELETE FROM `stockplanner`.`source`;";

            Execute(parser.eq);


            // write new table

            foreach (CSVValue csvv in parser.readvalues.values)
            {
                parser.eq.query = "INSERT INTO `stockplanner`.`source` (`stock`, `current_value`, `quantity`, `write_date`) VALUES ('" +
                            csvv.stock +
                            "'," +
                            csvv.current_value +
                            "," +
                            csvv.quantity +
                            ",'" +
                            csvv.write_date +
                            "');";

                Execute(parser.eq);
            }

            parser.eq.query = "";
        }

        public static CSVValues ReadSource(in EngineQuery eq)
        {
            eq.query = "SELECT * FROM stockplanner.source;";

            CSVValues ret = new CSVValues(Execute(eq));

            eq.query = "";

            return ret;
        }

        public static void ArchiveSource(in EngineQuery eq)
        {
            ArchiveSource(new CSVValues(), eq);
        }

        public static void ArchiveSource(CSVValues csvvs, in EngineQuery eq)
        {
            if (csvvs.initialized == false)
            {
                csvvs = ReadSource(eq);
            }

            foreach (CSVValue csvv in csvvs.values)
            {
                eq.query = "INSERT INTO `stockplanner`.`source_archive` (`stock`, `write_date`, `current_value`, `quantity`) VALUES (" +
                            "'" + csvv.stock + "', " +
                            "'" + csvv.write_date + "', " +
                            csvv.current_value + ", " +
                            csvv.quantity +
                            "); ";

                Execute(eq);
            }
        }

        public static List<List<string>> ReadBuyList(EngineQuery eq)
        {
            eq.query = "SELECT stock, underage FROM stockplanner.compute_buy_list;";

            return Execute(eq);
        }

    }


}
