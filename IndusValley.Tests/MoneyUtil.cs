using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;

namespace IndusValley.Tests {
    public static class MoneyUtil {
        public static Money NinetyPercentOf(Money amount) {
            return amount
                .Distribute(10)
                .Take(9)
                .Sum(m => m.Amount);
        }

        public static Money TenPercentOf(Money amount) {
            return amount.Distribute(10).First();
        }
    }
}
