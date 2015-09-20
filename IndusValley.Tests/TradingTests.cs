using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndusValley.Broker;
using IndusValley.Banking;
using Miscellaneous;

namespace IndusValley.Tests {
    [TestClass]
    public class TradingTests {
        [TestMethod]
        public void BuyLogic() {
            IBroker broker = new TestBrokerage();
            var bank = new Bank();
            var account = bank.GetAccount(Guid.Empty);
            var stock = bank.GetStock("TST");

            account.Deposit(100);

            Maybe<IOrder> submitted = null;
            using (var reservation = account.Reserve(50, TimeSpan.MaxValue)) {
                Assert.IsTrue(reservation.Awarded);

                submitted = broker.SubmitBuyOrder("TST", account, 50).Result;

                Assert.IsFalse(submitted.Value.IsExecuted);
                Assert.IsFalse(submitted.Value.IsExecuted);
            }
            
            broker.CheckBuyOrder(submitted.Value.Id).Result
                .Match(
                    none: () => Assert.Fail("order not fulfilled"),
                    some: o => {
                        account.Adjust(o.Slippage);
                        stock.Purchase(o.Shares);

                        Assert.AreEqual(105, account.Available);
                    }); 
        }

        [TestMethod]

        public void SellLogic() {
            IBroker broker = new TestBrokerage();
            var bank = new Bank();
            var account = bank.GetAccount(Guid.Empty);
            var stock = bank.GetStock("TST");

            account.Deposit(100);

            var submitted = broker.SubmitSellOrder("TST", account, 1).Result;
            Assert.IsTrue(submitted.HasValue);
            Assert.IsFalse(submitted.Value.IsExecuted);
            
            broker.CheckSellOrder(submitted.Value.Id).Result
                .Match(
                    none: () => Assert.Fail("order not fulfilled"),
                    some: o => {
                        account.Adjust(o.Slippage);
                        stock.Sell(o.Shares);

                        Assert.AreEqual(109, account.Available);
                    });
        }
    }
}
