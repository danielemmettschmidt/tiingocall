using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using cleaner_driver;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace cleaner
{
    enum File_Status_Is
    {
        File_Found_and_Parsed,
        File_Found_but_Parse_Failed,
        No_File_Found,
        Failed
    }

    public class NoSuchFileException : Exception
    {
        public NoSuchFileException()
        {
        }

        public NoSuchFileException(string message)
            : base(message)
        {
        }

    }

    class Parser
    {

        public string csv_filepath, manifest_filepath, yearly_contribution_filepath;

        public BuyList buylist;

        public CSVValues readvalues;

        public ManifestValues manifestvalues;

        public EngineQuery eq;

        public bool isstillgood, has_csv_file, has_yca_file;

        public Parser(string initdir, EngineQuery ineq)
        {
            this.eq = ineq;

            this.isstillgood = this.getfilenames(initdir);
        }

        public void Initialize()
        {
            try
            {
                if (this.isstillgood == true)
                {
                    this.isstillgood = this.SetYCAValue();
                }

                if (this.isstillgood == true)
                {
                    this.isstillgood = this.BuildReadValues();
                }

                if (this.isstillgood == true)
                {
                    this.isstillgood = this.BuildManifestValues();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void ComputeBuyList()
        {
            if (this.isstillgood == true)
            {
                this.buylist.SetBuyList(MYSQLEngine.ReadBuyList(this.eq));
            }

            foreach (Buy b in this.buylist.buys)
            {
                Console.WriteLine(b.ToString());
            }
        }

        public bool getfilenames(string initdir)
        {


            this.has_csv_file = true;

            try
            {
                this.csv_filepath = getfilename(initdir, ".csv");
            }
            catch (NoSuchFileException nsfex)
            {
                this.csv_filepath = "";
                this.has_csv_file = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            this.has_yca_file = true;

            this.manifest_filepath = getfilename(initdir, ".manifest");

            try
            {
                this.yearly_contribution_filepath = getfilename(initdir, ".yca");
            }
            catch(NoSuchFileException nsfex)
            {
                this.yearly_contribution_filepath = "";
                this.has_yca_file = false;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private string getfilename(string dir, string search)
        {
            foreach (string filename in Directory.GetFiles(dir))
            {
                if (filename.Contains(search))
                {
                    return filename;
                }
            }

            throw new NoSuchFileException("No file with ending \"" + search + "\" found in " + dir + ".");
        }

        public bool SetYCAValue()
        {
            switch(this.ProcessYCAValue())
            {
                case File_Status_Is.File_Found_and_Parsed:
                    File.Delete(this.yearly_contribution_filepath);
                    return true;
                case File_Status_Is.No_File_Found:
                    return true;
                default:
                    return false;
            }                
        }

        private File_Status_Is ProcessYCAValue()  //   BUY LIST IS INITIALLY CONSTRUCTED HERE, BUT NOT POPULATED
        {
            short ii = 0;

            if (this.has_yca_file == true)
            {
                foreach (string line in File.ReadLines(this.yearly_contribution_filepath))
                {
                    if (ii == 0)
                    {
                        string instr = CSVValues.parse_decimal_str(line), parsedstr = "";

                        int sii = (instr.Length - 2);

                        foreach (char c in instr)
                        {
                            if (sii > 0)
                            {
                                parsedstr = parsedstr + c;
                            }

                            sii--;
                        }

                        try
                        {
                            int ycaint = Int32.Parse(parsedstr);

                            this.buylist = new BuyList(ycaint);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to construct buylist from file " + this.yearly_contribution_filepath + " exception message: " + ex.Message);
                            return File_Status_Is.File_Found_but_Parse_Failed;
                        }

                        try
                        {
                            MYSQLEngine.WriteYCA(this.eq, parsedstr);

                            return File_Status_Is.File_Found_and_Parsed;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                            return File_Status_Is.Failed;
                        }
                    }

                    ii++;
                }

                return File_Status_Is.Failed;
            }
            else
            {
                this.buylist = new BuyList(Int32.Parse(MYSQLEngine.ReadYCA(this.eq)));

                return File_Status_Is.No_File_Found;
            }
        }

        public bool BuildReadValues()
        {
            if(this.has_csv_file == true)
            {
                try
                {
                    this.readvalues = new CSVValues(this.csv_filepath);

                    File.Delete(this.csv_filepath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("\n\nNo CSV file was found, building portfolio values from database.");
                this.readvalues = MYSQLEngine.ReadSource(this.eq);
            }

            return true;
        }

        public bool BuildManifestValues()
        {
            try
            {
                this.manifestvalues = new ManifestValues(this.manifest_filepath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }





}
