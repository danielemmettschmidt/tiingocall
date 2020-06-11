using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleaner_driver
{
    class BuyList
    {
        private int YCA, underageweight;
        public Buy[] buys;
        private Buy[] discards;
        private int dlyca;

        public BuyList(int inyca)
        {
            this.YCA = inyca;
            this.buys = new Buy[0];
            this.discards = new Buy[0];
            this.dlyca = -1;
        }

        public void SetBuyList(List<List<string>> qresult)
        {
            this.dlyca = ( ( this.YCA / 52 ) / 3 );

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
                this.add(qresult[ii][0], underages[ii]);
            }

            while (this.buys[buys.Length - 1].dollar_amount < (this.dlyca / 10) || this.buys[buys.Length-1].dollar_amount < 105)
            {
                this.recompute();
            }

            int pot = this.buys[buys.Length - 1].dollar_amount;

            this.recompute();

            this.randombuys(pot);
        }

        private void randombuys(int pot)
        {
            if(pot > 105)
            {
                int first = 105;

                pot = pot - 105;

                while(pot % 105 != 0)
                {
                    first++;
                    pot++;
                }

                randomadd(first);
            }

            while (pot > 0)
            {
                this.randomadd(105);

                pot = pot - 105;
            }

            this.discards = new Buy[0];
        }

        private void randomadd(int buy)
        {
            int index = new Random().Next(0, (this.discards.Length - 1));

            this.discards[index].dollar_amount = buy;

            this.add(this.discards[index]);

            this.burndiscard(index);
        }

        private void burndiscard(int index)
        {
            int ii = 0;

            Buy[] copy = this.discards;

            this.discards = new Buy[0];

            foreach(Buy b in copy)
            {
                if (ii != index)
                {
                    this.addtodiscards(b);
                }

                ii++;
            }
        }

        private void recompute()
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
                else
                {
                    refbuy.dollar_amount = 0;

                    this.addtodiscards(refbuy);
                }

                ii++;
            }

            this.buys = new Buy[0];

            foreach (Buy refbuy in referencebuys)
            {
                add(refbuy.stock, refbuy.underage);
            }

        }

        private void addtodiscards(Buy add)
        {
            Buy[] copy = this.discards;

            this.discards = new Buy[this.discards.Length + 1];

            int ii = 0;

            foreach (Buy b in copy)
            {
                this.discards[ii] = b;

                ii++;
            }

            this.discards[this.discards.Length - 1] = add;
        }

        private void add(string stonk, int undrge)
        {
            this.add(new Buy(stonk, undrge, this.underageweight, this.dlyca));
        }

        private void add(Buy add)
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

            this.buys[ii] = add;
        }

        ~BuyList()
        {

        }


    }
}
