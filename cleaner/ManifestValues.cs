using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{

    enum char_is
    {
        a_number,
        a_decimal,
        other
    }

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

        private char_is charclassify(char c)
        {
            if(char.IsNumber(c) == true)
            {
                return char_is.a_number;
            }
            else if(c == '.')
            {
                return char_is.a_decimal;
            }
            else
            {
                return char_is.other;
            }
        }

        private string parsetpstr(string stock, string tpstr)
        {
            if(tpstr == "0")
            {
                return tpstr;
            }

            short dii = -1;  // iterator for chars after decimal, -1 means no decimals yet found

            string buildstring = "";
            bool foundnonzero = false;

            foreach (char c in tpstr)
            {
                switch (charclassify(c))
                {
                    case char_is.a_number:

                        if (c != '0')
                        {
                            foundnonzero = true;
                        }

                        if (foundnonzero == true)
                        {
                            buildstring = buildstring + c;
                        }

                        if (dii != -1)
                        {
                            dii++;
                        }

                        break;

                    case char_is.a_decimal:
                        if (dii != -1)
                        {
                            throw new Exception("Multiple decimals found in target percentage for row \"" + stock + "," + tpstr + "\".");
                        }

                        dii = 0;
                        break;

                    default:
                        break;
                }
            }

            if(buildstring == "")
            {
                return "0";
            }
            else if (dii > 4)
            {
                throw new Exception("Too many digits past the decimal found in target percentage for row \"" + stock + "," + tpstr + "\"; maximum is four.");
            }
            else if (dii != -1)
            {
                for (; dii < 4; dii++)
                {
                    buildstring = buildstring + '0';
                }

            }
            else
            {
                buildstring = buildstring + "0000";
            }

            return buildstring;
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

            this.values[ii] = new ManifestValue(stock, parsetpstr(stock, tpstr));
        }

    }



}
