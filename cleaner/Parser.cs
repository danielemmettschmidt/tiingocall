using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using cleaner_driver;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace cleaner
{
    enum YCA_File_Status_Is
    {
        File_Found_and_Parsed,
        File_Found_but_Parse_Failed,
        No_File_Found,
        Failed
    }

    class Parser
    {

        public string csv_filepath, manifest_filepath, yearly_contribution_filepath;

        public BuyList buylist;

        public CSVValues readvalues;

        public ManifestValues manifestvalues;

        public EngineQuery eq;

        public bool isstillgood, has_yca_file;

        public Parser(string initdir, EngineQuery ineq)
        {
            this.eq = ineq;

            this.isstillgood = this.getfilenames(initdir);

            if(this.isstillgood == true)
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
            try
            {
                this.csv_filepath = getfilename(initdir, ".csv");
                this.manifest_filepath = getfilename(initdir, ".manifest");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            this.has_yca_file = true;

            try
            {
                this.yearly_contribution_filepath = getfilename(initdir, ".yca");
            }
            catch(Exception ex)
            {
                this.yearly_contribution_filepath = "";
                this.has_yca_file = false;
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

            throw new Exception("No file with ending \"" + search + "\" found in " + dir + ".");
        }

        public bool SetYCAValue()
        {
            switch(this.ProcessYCAValue())
            {
                case YCA_File_Status_Is.File_Found_and_Parsed:
                    File.Delete(this.yearly_contribution_filepath);
                    return true;
                case YCA_File_Status_Is.No_File_Found:
                    return true;
                default:
                    return false;
            }                
        }

        private YCA_File_Status_Is ProcessYCAValue()
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
                            return YCA_File_Status_Is.File_Found_but_Parse_Failed;
                        }

                        try
                        {
                            MYSQLEngine.WriteYCA(this.eq, parsedstr);

                            return YCA_File_Status_Is.File_Found_and_Parsed;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                            return YCA_File_Status_Is.Failed;
                        }
                    }

                    ii++;
                }

                return YCA_File_Status_Is.Failed;
            }
            else
            {
                this.buylist = new BuyList(Int32.Parse(MYSQLEngine.ReadYCA(this.eq)));

                return YCA_File_Status_Is.No_File_Found;
            }
        }

        public bool BuildReadValues()
        {
            try
            {
                this.readvalues = new CSVValues(this.csv_filepath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
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
