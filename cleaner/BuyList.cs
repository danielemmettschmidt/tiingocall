using System;
using System.Collections.Generic;
using System.Text;

namespace cleaner_driver
{
    class BuyList
    {
        private int YCA, underageweight;
        public Buy[] buys;

        public BuyList(int inyca)
        {
            this.YCA = inyca;
            this.buys = new Buy[0];
        }

        public void SetBuyList(List<List<string>> qresult)
        {
            int dailycontributionamount = ( ( this.YCA / 52 ) / 3 );

            this.underageweight = 0;
            List<int> underages = new List<int>();

            foreach(List<string> row in qresult)
            {
                if (row.Count != 2)
                {
                    string buildex = "";

                    foreach (string cell in row)
                    {
                        buildex = "<" + buildex + ">"; 
                    }

                    throw new Exception("Incorrect number of cells found in row " + buildex);
                }

                try
                {
                    int underooski = Int32.Parse(row[1]);

                    underages.Add(underooski);

                    this.underageweight = this.underageweight + underooski;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            for (short ii = 0; ii < qresult.Count; ii++)
            {
                this.add(qresult[ii][0], underages[ii], dailycontributionamount);
            }

            while (this.buys[buys.Length - 1].dollar_amount < 100)
            {
                recompute(dailycontributionamount);
            }

        }

        private void recompute(int dlyca)
        {
            this.underageweight = 0;

            int cap = (this.buys.Length -1), ii = 0;
            
            Buy[] referencebuys = new Buy[cap];

            foreach (Buy refbuy in this.buys)
            {
                if(ii < cap)
                {
                    referencebuys[ii] = new Buy(refbuy.stock, refbuy.underage, 0);
                    underageweight = underageweight + refbuy.underage;
                }

                ii++;
            }

            this.buys = new Buy[0];

            foreach (Buy refbuy in referencebuys)
            {
                add(refbuy.stock, refbuy.underage, dlyca);
            }

        }

        private void add(string stonk, int undrge, int dlyca)
        {

            Buy[] inptbuys = new Buy[this.buys.Length];

            short ii = 0;

            foreach (Buy inptb in this.buys)
            {
                inptbuys[ii] = new Buy(inptb.stock, inptb.underage, inptb.dollar_amount);
                ii++;
            }

            this.buys = new Buy[this.buys.Length + 1];

            ii = 0;

            foreach (Buy otptb in inptbuys)
            {
                this.buys[ii] = new Buy(otptb.stock, otptb.underage, otptb.dollar_amount);
                ii++;
            }

            this.buys[ii] = new Buy(stonk,undrge,this.underageweight,dlyca);
        }

        ~BuyList()
        {

        }


    }
}
