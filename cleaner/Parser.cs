using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using cleaner_driver;
using System.Diagnostics.Contracts;

namespace cleaner
{
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

            try
            {
                this.yearly_contribution_filepath = getfilename(initdir, ".yca");
            }
            catch(Exception ex)
            {
                this.yearly_contribution_filepath = "";
                this.has_yca_file = false;
            }

            this.has_yca_file = true;
            
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
                            return false;
                        }

                        try
                        {
                            MYSQLEngine.WriteYCA(this.eq, parsedstr);

                            //////////////////////////////////////////////////////write function to delete yca file

                            return true;
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            else
            {
                //////////////////////////////////////////////////////////////////pull value from yca view
                
                
                return false;
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
