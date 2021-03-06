﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.OleDb;
using cleaner_driver;

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
        public short stockcolnum, valcolnum, quantcolnum, commas;
        public bool initialized;
        public string id;
        public CSVValue[] values;

        public CSVValues()
        {
            this.commas = 0;
            this.stockcolnum = -1;
            this.valcolnum = -1;
            this.quantcolnum = -1;

            this.values = new CSVValue[0];
            this.initialized = false;
        }

        public CSVValues(List<List<string>> input)
        {
            this.commas = 0;
            this.stockcolnum = -1;
            this.valcolnum = -1;
            this.quantcolnum = -1;

            this.values = new CSVValue[0];
            foreach (List<string> row in input)
            {
                if (row.Count != 4)
                {
                    throw new Exception(row.Count + " is too many cells for row identified by first cell " + row[0] + " and last cell " + row[row.Count]);
                }
                this.add(row[0], row[1], row[2], row[3]);
            }

            this.initialized = true;
        }

        public CSVValues(string csvfilepath)
        {
            this.commas = 0;
            this.stockcolnum = -1;
            this.valcolnum = -1;
            this.quantcolnum = -1;

            this.values = new CSVValue[0];

            short ii = 0;

            foreach (string lline in File.ReadLines(csvfilepath))
            {
                string line = cleanquotes(lline);

                if (ii == 0)
                {
                    this.grabindexes(line);
                }
                else
                {                    
                    if (countcommas(line) == this.commas)
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

            this.initialized = true;
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

        private string cleanquotes(string inp)
        {
            string ret = "", last = "";

            int on = 1;

            foreach (char c in inp)
            {
                ret = ret + last;

                if (c == '\"')
                {
                    on = on * -1;
                }
                else
                {
                    if (c != ',' || (c == ',' && on == 1))
                    {
                        last = "" + c;
                    }
                }
            }

            if (last != ",")
            {
                ret = ret + last;
            }

            return ret;
        }

        private short countcommas(string inp)
        {
            short commas = 0;

            foreach (char c in inp)
            {
                if (c == ',')
                {
                    commas++;
                }
            }

            return commas;
        }

        public void grabindexes(string titleline)
        {
            short ii = 0;

            this.commas = countcommas(titleline);

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

            if (this.stockcolnum == -1)
            {
                throw new Exception("Failed to find stock symbol column.");
            }

            if (this.valcolnum == -1)
            {
                throw new Exception("Failed to find current value column.");
            }

            if (this.quantcolnum == -1)
            {
                throw new Exception("Failed to find quantity column.");
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
            this.add(stock,prce,quant,"");
        }

        public void add(string stock, string prce, string quant, string dte)
        {
            if(dte != "")
            {
                DateTime process = DateTime.Parse(dte);

                dte = process.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                dte = MYSQLEngine.timenow();
            }

            CSVValue[] newvalues = new CSVValue[this.values.Length];

            short ii = 0;

            foreach (CSVValue old in this.values)
            {
                newvalues[ii] = new CSVValue(old.stock,old.current_value,old.quantity, old.write_date);
                ii++;
            }

            this.values = new CSVValue[this.values.Length + 1];

            ii = 0;

            foreach (CSVValue newv in newvalues)
            {
                this.values[ii] = new CSVValue(newv.stock,newv.current_value,newv.quantity, newv.write_date);
                ii++;
            }

            this.values[ii] = new CSVValue(stock, prce, quant, dte);

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


        public static string parse_decimal_str(string decnum)
        {
            try { return parse_decimal_str("", decnum); }
            catch (Exception ex) { throw new Exception("Failed to parse decimal string " + decnum); }
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
