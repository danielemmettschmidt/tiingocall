using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleaner_driver
{
    class Buy
    {
        public string stock;
        public int underage, dollar_amount;

        public Buy(string stck, int undrge, int da)
        {
            this.stock = stck;
            this.underage = undrge;
            this.dollar_amount = da;
        }

        public Buy(string stck, int undrge, int wght, int dlyca)
        {
            this.stock = stck;
            this.underage = undrge;

            this.dollar_amount = ((undrge * dlyca) / wght);
        }

        public string ToString()
        {
            string ret = this.stock + " - $";

            string reference = ("" + this.dollar_amount);

            int dec = reference.Length -2;

            short ii = 0;

            foreach(char c in reference)
            {
                if (ii == dec)
                {
                    ret = ret + ".";
                }

                ret = ret + c;

                ii++;
            }

            return ret;

        }

        ~Buy()
        {

        }
    }
}
