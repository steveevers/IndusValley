using SE.Money;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IndusValley.Banking {
    public class Account {
        private List<Reservation> reservations;
        
        internal Account() {
            this.Balance = new Money(0M, Currency.FromCurrencyCode(CurrencyCode.USD));

            this.reservations = new List<Reservation>();
        }

        public Guid Id { get; internal set; } 
        public Money Balance { get; internal set; }
        public Money Available {
            get {
                Money available = new Money(this.Balance);
                DateTimeOffset now = DateTimeOffset.Now;
                foreach (var r in reservations) {
                    if (r.Expiration > now)
                        available -= r.Amount;
                }

                reservations = reservations
                    .Where(r => r.Expiration >= now)
                    .ToList();

                return available;
            }
        }

        public void Adjust(Money amount) {
            this.Balance += amount;
        }

        public void Deposit(Money amount) {
            if (amount <= 0) throw new ArgumentException("must deposit non-negative amounts");

            this.Balance += amount;
        }

        public void Withdraw(Money amount) {
            if (this.Available < amount) throw new ArgumentException("Cannot withdraw more than available balance");

            this.Balance -= amount;
        }

        public Reservation Reserve(Money amount, TimeSpan interval) {
            if (this.Available < amount) {
                return Reservation.Deny();
            }

            var reservation = DateTimeOffset.MaxValue - DateTimeOffset.Now < interval
                ? Reservation.Award(this, DateTimeOffset.MaxValue, amount)
                : Reservation.Award(this, DateTimeOffset.Now.Add(interval), amount);

            this.reservations.Add(reservation);
            return reservation;
        }

        public void Release(Reservation reservation) {
            this.reservations.RemoveAll(r => r.Id == reservation.Id);
        }
    }
}