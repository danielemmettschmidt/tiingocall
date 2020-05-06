using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{
    class Parser
    {

        public string csvfilepath, manifestfilepath;

        public CSVValues readvalues;

        public ManifestValues manifestvalues;

        public bool isstillgood;

        public Parser(string initdir)
        {
            this.isstillgood = this.getfilenames(initdir);

            if (this.isstillgood == true)
            {
                this.isstillgood = this.BuildReadValues();
            }

            if (this.isstillgood == true)
            {
                this.isstillgood = this.BuildManifestValues();
            }

        }

        public bool getfilenames(string initdir)
        {
            try
            {
                this.csvfilepath = getfilename(initdir, ".csv");
                this.manifestfilepath = getfilename(initdir, ".manifest");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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

        public bool BuildReadValues()
        {
            try
            {
                this.readvalues = new CSVValues(this.csvfilepath);
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
                this.manifestvalues = new ManifestValues(this.manifestfilepath);
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
