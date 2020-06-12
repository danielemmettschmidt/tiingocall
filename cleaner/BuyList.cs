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
        private int dlyca, pot;

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
            this.pot = this.dlyca;

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

                this.pot = this.pot - this.buys[this.buys.Length - 1].dollar_amount;
            }

            this.emptypot();

            while (this.buys[buys.Length - 1].dollar_amount < (this.dlyca / 10) || this.buys[buys.Length-1].dollar_amount < 105)
            {
                this.recompute();
            }

            this.randombuys(this.buys[buys.Length - 1].dollar_amount);
        }

        private void displaybuyssum()
        {
            int sum = 0;

            foreach(Buy b in this.buys)
            {
                sum = sum + b.dollar_amount;
            }

            Console.WriteLine("Sum is " + sum);
        }

        private void randombuys(int lastbuy)
        {
            this.displaybuyssum();

            this.droplastbuy();

            this.displaybuyssum();

            if (lastbuy > 105)
            {
                int first = 105;

                lastbuy = lastbuy - 105;

                while(lastbuy % 105 != 0)
                {
                    first++;
                    lastbuy--;
                }

                randomadd(first);

                this.displaybuyssum();
            }

            while (lastbuy > 0)
            {
                this.randomadd(105);

                lastbuy = lastbuy - 105;

                this.displaybuyssum();
            }

            this.discards = new Buy[0];
        }

        private void droplastbuy()
        {
            droplastbuy(false);
        }

        private void droplastbuy(bool Recalculate)
        {
            this.underageweight = 0;

            int cap = (this.buys.Length - 1), ii = 0;

            Buy[] copy = this.buys;

            this.buys = new Buy[0];

            foreach (Buy b in copy)
            {
                if (ii < cap)
                {
                    if (Recalculate == true)
                    {
                        this.add(new Buy(b.stock, b.underage, 0));

                        this.underageweight = this.underageweight + b.underage;
                    }
                    else
                    {
                        this.add(b);
                    }
                }
                else
                {
                    b.dollar_amount = 0;

                    this.addtodiscards(new Buy(b.stock, -1, b.dollar_amount));
                }

                ii++;
            }

            if(Recalculate == false)
            {
                return;
            }

            copy = this.buys;

            this.buys = new Buy[0];

            foreach (Buy b in copy)
            {
                add(b.stock, b.underage);

                this.pot = this.pot - this.buys[this.buys.Length - 1].dollar_amount;               
            }
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
            this.pot = this.dlyca;

            this.droplastbuy(true);

            this.emptypot();
        }

        private void emptypot()
        {
            int ii = 0, incrementor = 1;

            while(incrementor * this.buys.Length < this.pot)
            {
                incrementor++;
            }

            while ((this.pot - incrementor) > 0)
            {
                this.buys[ii].dollar_amount = this.buys[ii].dollar_amount + incrementor;

                this.pot = this.pot - incrementor;

                ii++;
            }

            this.buys[ii].dollar_amount = this.buys[ii].dollar_amount + this.pot;

            this.pot = 0;
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
