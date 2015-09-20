using IndusValley.Banking;
using SE.Miscellaneous;
using SE.Money;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndusValley.Broker {
    public class Brokerage : IBroker {
        public Task<Maybe<IOrder>> CheckBuyOrder(Guid buyId) {
            throw new NotImplementedException();
        }

        public Task<Maybe<IOrder>> CheckSellOrder(Guid sellId) {
            throw new NotImplementedException();
        }

        public IObservable<Money> Stock(string symbol) {
            throw new NotImplementedException();
        }

        public Task<Maybe<IOrder>> SubmitBuyOrder(string symbol, Account account, Money amount) {
            throw new NotImplementedException();
        }

        public Task<Maybe<IOrder>> SubmitSellOrder(string symbol, Account account, double shares) {
            throw new NotImplementedException();
        }
    }
}