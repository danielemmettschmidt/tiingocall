using System;
using System.Collections.Generic;
using System.Text;

namespace cleaner
{
    class ManifestValue
    {
        public string stock;
        public string targetpercentage;

        public ManifestValue()
        {
            this.stock = "";
            this.targetpercentage = "0";
        }

        public ManifestValue(string stck)
        {
            this.stock = stck;
            this.targetpercentage = "0";
        }

        public ManifestValue(string stck, string tp)
        {
            this.stock = stck;
            this.targetpercentage = tp;
        }

        ~ManifestValue()
        {
        }
    }
}
