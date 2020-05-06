using System;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{
    class CSVValue
    {
        public string stock, price;

        public CSVValue()
        {
            this.stock = "";
            this.price = "";
        }

        public CSVValue(string stck)
        {
            this.stock = stck;
            this.price = "";
        }

        public CSVValue(string stock, string price)
        {
            this.stock = stock;
            this.price = price;
        }

        ~CSVValue()
        {

        }

    }

}
