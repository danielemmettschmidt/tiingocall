using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{

    class ManifestValues
    {
        public ManifestValue[] values;

        public ManifestValues(string filepath)
        {
            this.values = new ManifestValue[0];

            bool foundnull = false, foundtp = false;

            // check manifest

            foreach (string line in File.ReadLines(filepath))
            {
                short length = (short)line.Split(',').Length;

                if (length == 2)
                {
                    foundtp = true;
                }
                else if (length == 1)
                {
                    foundnull = true;
                }
                else
                {
                    this.values = new ManifestValue[0];
                    throw new Exception("ManifestValues constructor found line \"" + line + "\" with too many commas.");
                }

                if (foundtp == true)
                {
                    if (foundnull == true)
                    {
                        this.values = new ManifestValue[0];
                        throw new Exception("ManifestValues constructor found both null and present percentage columns");
                    }

                    this.add(line.Split(',')[0], line.Split(',')[1]);
                }
                else
                {
                    this.add(line);
                }
            }

            return;
        }

        ~ManifestValues()
        {

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

        public void add(string stock, string tpstr)
        {
            
            ManifestValue[] newvalues = new ManifestValue[this.values.Length];

            short ii = 0;

            foreach (ManifestValue old in this.values)
            {
                newvalues[ii] = new ManifestValue(old.stock, old.targetpercentage);
                ii++;
            }

            this.values = new ManifestValue[this.values.Length + 1];

            ii = 0;

            foreach (ManifestValue newv in newvalues)
            {
                this.values[ii] = new ManifestValue(newv.stock, newv.targetpercentage);
                ii++;
            }

            this.values[ii] = new ManifestValue(stock, CSVValues.parse_decimal_str(stock, tpstr));
        }
    }



}
