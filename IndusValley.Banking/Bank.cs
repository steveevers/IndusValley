using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Types;

namespace IndusValley.Banking {
    public class Bank {
        public Account OpenAccount() {
            return new Account();
        }

        public Account OpenAccount(Money seedAmount) {
            var account = new Account();
            account.Deposit(seedAmount);

            return account;
        }

        public Account SplitAccount(Account existingAccount, Money amount) {
            existingAccount.Withdraw(amount);

            return this.OpenAccount(amount);
        }

        public Account GetAccount(Guid id) {
            return new Account() { Id = id };
        }

        public Stock GetStock(string symbol) {
            return new Stock { Symbol = symbol };
        }
    }
}
