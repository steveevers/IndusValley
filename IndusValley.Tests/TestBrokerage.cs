using IndusValley.Banking;
using IndusValley.Broker;
using SE.Miscellaneous;
using SE.Money;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndusValley.Tests {
    public class TestOrder : IOrder {
        public Guid Id { get; internal set; }
        public bool IsExecuted { get; internal set; }
        public Money Limit { get; internal set; }
        public decimal Shares { get; internal set; }
        public Money Slippage { get; internal set; }
        public Stock Stock { get; internal set; }
    }

    public class TestBrokerage : IBroker {
        private Dictionary<Guid, IOrder> buyOrders = new Dictionary<Guid, IOrder>();
        private Dictionary<Guid, IOrder> sellOrders = new Dictionary<Guid, IOrder>();
        
        public async Task<Maybe<IOrder>> SubmitBuyOrder(string symbol, Account account, Money amount) {
            await Task.Yield();

            var id = Guid.NewGuid();
            var slippage = MoneyUtil.TenPercentOf(amount);
            var order = new TestOrder() {
                Id = id, 
                Slippage = slippage, 
                IsExecuted = false
            };

            buyOrders.Add(id, order);

            return Maybe<IOrder>.Some(order);
        }

        public async Task<Maybe<IOrder>> CheckBuyOrder(Guid buyId) {
            await Task.Yield();

            var order = ((TestOrder)buyOrders[buyId]);
            order.IsExecuted = true;

            return Maybe<IOrder>.Some(buyOrders[buyId]);
        }

        public IObservable<Money> Stock(string symbol) {
            throw new NotImplementedException();
        }

        public async Task<Maybe<IOrder>> SubmitSellOrder(string symbol, Account account, double shares) {
            await Task.Yield();

            var id = Guid.NewGuid();
            var slippage = MoneyUtil.NinetyPercentOf(new Money(shares * 10, Currency.FromCurrencyCode(CurrencyCode.USD)));
            var order = new TestOrder() {
                Id = id, 
                Slippage = slippage, 
                IsExecuted = false
            };

            sellOrders.Add(id, order);

            return Maybe<IOrder>.Some(order);
        }

        public async Task<Maybe<IOrder>> CheckSellOrder(Guid sellId) {
            await Task.Yield();
            
            var order = ((TestOrder)sellOrders[sellId]);
            order.IsExecuted = true;
            
            return Maybe<IOrder>.Some(order);
        }
    }
}
