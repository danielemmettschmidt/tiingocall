using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{
    class cleaner
    {

        public string csvfile;

        public csvvalues readvalues;

        public csvvalues writevalues;

        private bool isstillgood;

        public cleaner(string initdir)
        {
            this.readvalues = new csvvalues();
            this.writevalues = new csvvalues();

            this.isstillgood = this.getcsvfilename(initdir);

            if(this.isstillgood == true)
            {
                this.isstillgood = this.buildreadvalues();
            }

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

        public bool buildreadvalues()
        {
            try
            {
                this.readvalues.add("test"); ffff // bookmark, build read values from here
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

    }

    class csvvalues
    {
        public string[] stocks;
        public string[] prices;

        public csvvalues()
        {
            this.stocks = new string[0];
            this.prices = new string[0];

        }

        public void add(string stock)
        {

            try
            {
                this.add(stock, "");
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public void add(string stock, string price)
        {
            if (this.stocks.Length != this.prices.Length)
            {
                throw new Exception("Cannot add: existing stocks and prices are unequal lengths.");
            }

            string[] newstocks = new string[this.stocks.Length];
            string[] newprices = new string[this.prices.Length];

            short ii = 0;

            foreach (string old in this.stocks)
            {
                newstocks[ii] = old;
                ii++;
            }

            ii = 0;

            foreach (string old in this.prices)
            {
                newprices[ii] = old;
                ii++;
            }

            this.stocks = new string[this.stocks.Length + 1];
            this.prices = new string[this.prices.Length + 1];

            foreach (string old in newstocks)
            {
                this.stocks[ii] = old;
                ii++;
            }

            ii = 0;

            foreach (string old in newprices)
            {
                this.prices[ii] = old;
                ii++;
            }

            this.stocks[ii] = stock;
            this.prices[ii] = price;

        }

    }

}
