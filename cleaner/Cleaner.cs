using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{
    class cleaner
    {

        public string csvfile;

        public csvvalues values;

        private bool isgood;

        public cleaner(string initdir)
        {
            this.isgood = false;
            this.isgood = this.getcsvfilename(initdir);


        }

        public bool getcsvfilename(string initdir)
        {
            foreach (string filename in Directory.GetFiles(initdir))
            {
                if (filename.Contains(".csv"))
                {
                    this.csvfile = filename;
                    return true;
                }
            }

            return false;
        }

    }

    class csvvalues
    {
        public string[] stocks;
        public string[] prices;

        csvvalues()
        {
            this.stocks = new string[0];
            this.prices = new string[0];

        }

        public void add()
        {

        }

    }

}
