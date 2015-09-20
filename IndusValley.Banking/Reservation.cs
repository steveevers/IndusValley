using SE.Money;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndusValley.Banking {
    public class Reservation : IDisposable {
        private Account account;
        
        public readonly Guid Id;
        public readonly DateTimeOffset Expiration;
        public readonly Money Amount;
        public readonly bool Awarded;

        private Reservation(Account account, DateTimeOffset expiration, Money amount) : this(true) {
            this.account = account;
            this.Expiration = expiration;
            this.Amount = amount;
        }

        private Reservation(bool awarded) {
            this.Id = Guid.NewGuid();
            this.Awarded = awarded;
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposing) {
                return;
            }

            if (!this.Awarded)
                return;
            
            if (this.account != null)
                this.account.Release(this);
        }

        internal static Reservation Deny() {
            return new Reservation(false);
        }

        internal static Reservation Award(Account account, DateTimeOffset expiration, Money amount) {
            return new Reservation(account, expiration, amount);
        }
    }
}
