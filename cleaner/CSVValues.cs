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

    class CSVValues
    {
        public short stockcolnum, valcolnum, quantcolnum;
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
                            string stck = parsesymbol(line.Split(',')[this.stockcolnum]);

                            if (stck != "")
                            {
                                string crrent_value = parse_decimal_str(stck, line.Split(',')[this.valcolnum]);
                                string qant = parse_decimal_str(stck, line.Split(',')[this.quantcolnum]);
                                this.add(stck, crrent_value, qant);
                            }
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

        private static string parsesymbol(string input)
        {
            string read = "";

            foreach (char c in input.ToUpper())
            {
                if (
                        ( (int)c >= (int)('0') && (int)c <= (int)('9') ) ||
                        ( (int)c >= (int)('A') && (int)c <= (int)('Z') )
                   )
                {
                    read = read + c;
                }
            }
            
            if (read.Contains("CORE"))
            {
                return "!CASH!";
            }

            if (read.Length > 7)
            {
                return "";
            }            

            return read;
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
                    this.valcolnum = ii;
                }

                if (cell.Contains("quantity"))
                {
                    this.quantcolnum = ii;
                }

                ii++;
            }
        }

        public void add(string stock)
        {
            try
            {
                this.add(stock, "", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void add(string stock, string prce, string quant)
        {


            CSVValue[] newvalues = new CSVValue[this.values.Length];

            short ii = 0;

            foreach (CSVValue old in this.values)
            {
                newvalues[ii] = new CSVValue(old.stock,old.current_value,old.quantity);
                ii++;
            }

            this.values = new CSVValue[this.values.Length + 1];

            ii = 0;

            foreach (CSVValue newv in newvalues)
            {
                this.values[ii] = new CSVValue(newv.stock,newv.current_value,newv.quantity);
                ii++;
            }

            this.values[ii] = new CSVValue(stock, prce, quant);

        }

        private static char_is charclassify(char c)
        {
            if (char.IsNumber(c) == true)
            {
                return char_is.a_number;
            }
            else if (c == '.')
            {
                return char_is.a_decimal;
            }
            else
            {
                return char_is.other;
            }
        }

        public static string parse_decimal_str(string stock, string decnum)
        {
            if (decnum == "0")
            {
                return decnum;
            }

            short dii = -1;  // iterator for chars after decimal, -1 means no decimals yet found

            string buildstring = "";
            bool foundnonzero = false;

            foreach (char c in decnum)
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
                            throw new Exception("Multiple decimals found in number for row \"" + stock + "," + decnum + "\".");
                        }

                        dii = 0;
                        break;

                    default:
                        break;
                }
            }

            if (buildstring == "")
            {
                return "0";
            }
            else if (dii > 4)
            {
                throw new Exception("Too many digits past the decimal found in number for row \"" + stock + "," + decnum + "\"; maximum is four.");
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

    }

}
