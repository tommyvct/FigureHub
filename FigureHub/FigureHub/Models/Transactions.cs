using System;
using System.Collections.Generic;
using System.Text;

namespace FigureHub.Models
{
    public abstract class Transactions
    {
        protected long amount;
        protected string currency;  // probably use ISO identifier instead of string?

        public Transactions(long newAmount, string newCurrency)
        {
            amount = newAmount;
            currency = newCurrency;
        }

    }
}
