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

        public CSVValues writevalues;

        private bool isstillgood;

        public Parser(string initdir)
        {
            this.readvalues = new CSVValues();
            this.writevalues = new CSVValues();

            this.isstillgood = this.getfilenames(initdir);

            if (this.isstillgood == true)
            {
                this.isstillgood = this.BuildReadValues();
            }

            if (this.isstillgood == true)
            {
                this.isstillgood = this.BuildWriteValues();
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
                this.readvalues.Build(this.csvfilepath, BuildDirective.Read);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public bool BuildWriteValues()
        {
            try
            {
                this.writevalues.Build(this.manifestfilepath, BuildDirective.Write);  /// bookmark
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
