using System;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{
    class CSVValue
    {
        public string stock, current_value, quantity, write_date;

        public CSVValue()
        {
            this.stock = "";
            this.current_value = "";
            this.quantity = "";
        }

        public CSVValue(string stck)
        {
            this.stock = stck;
            this.current_value = "";
            this.quantity = "";
        }

        public CSVValue(string stock, string prce, string quant)
        {
            this.stock = stock;
            this.current_value = prce;
            this.quantity = quant;
        }

        public CSVValue(string stock, string prce, string quant, string dte)
        {
            this.stock = stock;
            this.current_value = prce;
            this.quantity = quant;
            this.write_date = dte;
        }

        ~CSVValue()
        {

        }

    }

}
