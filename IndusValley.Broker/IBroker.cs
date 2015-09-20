using IndusValley.Banking;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;

namespace IndusValley.Broker {
    public interface IBroker {
        IObservable<Money> Stock(string symbol);
        Task<Maybe<IOrder>> SubmitBuyOrder(string symbol, Account account, Money amount);
        Task<Maybe<IOrder>> CheckBuyOrder(Guid buyId);
        Task<Maybe<IOrder>> SubmitSellOrder(string symbol, Account account, double shares);
        Task<Maybe<IOrder>> CheckSellOrder(Guid sellId);
    }

    public interface IOrder {
        Guid Id { get; }
        Stock Stock { get; }
        Money Limit { get; }
        Money Slippage { get; }
        decimal Shares { get; }
        bool IsExecuted { get; }
    }
}