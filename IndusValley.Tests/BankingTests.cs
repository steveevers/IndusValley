using IndusValley.Banking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SE.Money;
using System;
using System.Threading;

namespace IndusValley.Tests {
    [TestClass]
    public class BankingTests {
        [TestMethod]
        public void CanDeposit() {
            var bank = new Bank();
            var account = bank.OpenAccount();
            account.Deposit(10);

            Assert.AreEqual(10, account.Balance.Amount);
        }

        [TestMethod]
        public void CanWithdraw() {
            var bank = new Bank();
            var account = bank.OpenAccount();
            account.Deposit(10);
            account.Withdraw(5);

            Assert.AreEqual(5M, account.Balance.Amount);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void WithdrawEnforcesBalance() {
            var bank = new Bank();
            var account = bank.OpenAccount();
            account.Deposit(5M);
            account.Withdraw(10M);
        }

        [TestMethod]
        public void CanAdjust() {
            var bank = new Bank();
            var account = bank.GetAccount(Guid.Empty);

            account.Adjust(10);
            Assert.AreEqual(10, account.Available);

            account.Adjust(-5);
            Assert.AreEqual(5, account.Available);

            account.Adjust(0);
            Assert.AreEqual(5, account.Available);
        }

        [TestMethod]
        public void CanSplitAccount() {
            var bank = new Bank();
            var account = bank.OpenAccount(100);
            var split = bank.SplitAccount(account, 50);

            Assert.AreEqual(50, account.Balance.Amount);
            Assert.AreEqual(50, split.Balance.Amount);
        }

        [TestMethod]
        public void CanReserve() {
            var bank = new Bank();
            var account = bank.OpenAccount(100);

            using (var reservation = account.Reserve(50, TimeSpan.FromSeconds(1))) {
                Assert.IsTrue(reservation.Awarded);
                Assert.AreEqual(50, account.Available);

                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.AreEqual(100, account.Available);
            }
        }

        [TestMethod]
        public void CanReserveMultiple() {
            var bank = new Bank();
            var account = bank.OpenAccount(100);

            using (var reserve50 = account.Reserve(50, TimeSpan.FromSeconds(1)))
            using (var reserve25 = account.Reserve(25, TimeSpan.FromSeconds(2))) {

                Assert.AreEqual(true, reserve50.Awarded);
                Assert.AreEqual(true, reserve25.Awarded);
                Assert.AreEqual(25, account.Available);

                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.AreEqual(75, account.Available);

                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.AreEqual(100, account.Available);
            }
        }

        [TestMethod]
        public void CanReturnReservation() {
            var bank = new Bank();
            var account = bank.OpenAccount(100);

            using (var reservation = account.Reserve(50, TimeSpan.MaxValue)) {
                Assert.IsTrue(reservation.Awarded);
                Assert.AreEqual(50, account.Available.Amount);
            }

            Assert.AreEqual(100, account.Available.Amount);
        }
    }
}
