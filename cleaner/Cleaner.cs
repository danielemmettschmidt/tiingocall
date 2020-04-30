using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{
    class cleaner
    {

        public string csvfilepath;

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
                    this.csvfilepath = filename;
                    return true;
                }
            }

            return false;
        }

        public bool buildreadvalues()
        {
            try
            {
                this.readvalues.build(this.csvfilepath);
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
        public short stockcolnum, pricecolnum;
        public string id;
        public string[] stocks, prices;

        public csvvalues()
        {
            this.stocks = new string[0];
            this.prices = new string[0];
        }

        public void build(string csvfilepath)
        {
            short ii = 0;

            foreach (string line in File.ReadLines(csvfilepath))
            {
                if(ii == 0)
                {
                    this.grabindexes(line);
                }
                else
                {
                    if(ii == 1)
                    {
                        this.id = line.Split(',')[0];
                    }

                    if (line.Split(',')[0] == this.id)
                    {
                        try
                        {
                            this.add(line.Split(',')[this.stockcolnum], line.Split(',')[this.pricecolnum]);
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                ii++;
            }
        }

        public void grabindexes(string titleline)
        {
            short ii = 0;

            foreach (string cell in titleline.ToLower().Split(','))
            {
                if(cell.Contains("symbol"))
                {
                    this.stockcolnum = ii;
                }

                if(cell.Contains("current value"))
                {
                    this.pricecolnum = ii;
                }

                ii++;
            }
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

            ii = 0;

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

    class filebuffer
    {

    }

}
