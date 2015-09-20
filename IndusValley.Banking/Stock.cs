using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndusValley.Banking {
    public class Stock {
        public string Symbol { get; set; }
        public decimal Shares { get; set; }

        public void Purchase(decimal shares) {
            this.Shares += shares;
        }

        public void Sell(decimal shares) {
            this.Shares -= shares;
        }
    }
}
