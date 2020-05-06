using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{

    class CSVValues
    {
        public short stockcolnum, pricecolnum;
        public string id;
        public CSVValue[] values;

        public CSVValues(string csvfilepath)
        {
            this.values = new CSVValue[0];

            short ii = 0;

            foreach (string line in File.ReadLines(csvfilepath))
            {
                if (ii == 0)
                {
                    this.grabindexes(line);
                }
                else
                {
                    if (ii == 1)
                    {
                        this.id = line.Split(',')[0];
                    }

                    if (line.Split(',')[0] == this.id)
                    {
                        try
                        {
                            this.add(line.Split(',')[this.stockcolnum], line.Split(',')[this.pricecolnum]);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                ii++;
            }
        }

        ~CSVValues()
        {

        }

        public void grabindexes(string titleline)
        {
            short ii = 0;

            foreach (string cell in titleline.ToLower().Split(','))
            {
                if (cell.Contains("symbol"))
                {
                    this.stockcolnum = ii;
                }

                if (cell.Contains("current value"))
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
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void add(string stock, string price)
        {


            CSVValue[] newvalues = new CSVValue[this.values.Length];

            short ii = 0;

            foreach (CSVValue old in this.values)
            {
                newvalues[ii] = new CSVValue(old.stock,old.price);
                ii++;
            }

            this.values = new CSVValue[this.values.Length + 1];

            ii = 0;

            foreach (CSVValue newv in newvalues)
            {
                this.values[ii] = new CSVValue(newv.stock,newv.price);
                ii++;
            }

            this.values[ii] = new CSVValue(stock, price);

        }

    }

    

}
