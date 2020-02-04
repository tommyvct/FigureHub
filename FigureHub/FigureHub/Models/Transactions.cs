using System;
using System.Collections.Generic;
using System.Text;

namespace FigureHub.Models
{
    class Transactions
    {
        // add location, merchant info, and maybe receipt?
        protected string description { get => description; set => description = value; }
        protected long amount { get => amount; set => amount = value; }
        protected string currency { get => currency; set => currency = value; }  // probably use ISO identifier instead of string?



        protected Transactions() { }

        /// <summary>
        /// For purchases, refund and deposit transactions.
        /// </summary>
        /// <param name="newdescription"></param>
        /// <param name="newCurrency"></param>
        /// <param name="newAmount"></param>
        public Transactions(string newDescription, string newCurrency, long newAmount)
        {
            description = newDescription;
            amount = newAmount;
            currency = newCurrency;
        }

        protected Transactions(PreAuthTransaction toCopy)
        {
            description = toCopy.description;
            amount = toCopy.amount;
            currency = toCopy.currency;
        }

        protected Transactions(Transactions toCopy)
        {
            description = toCopy.description;
            amount = toCopy.amount;
            currency = toCopy.currency;
        }
    }

    class PreAuthTransaction : Transactions
    {
        protected DateTime PreAuthExpireDate;



        public PreAuthTransaction(string newDescription, string newCurrency, long newAmount, DateTime newPreAuthExpireDate)
            : base(newDescription, newCurrency, newAmount)
        {
            PreAuthExpireDate = newPreAuthExpireDate;
        }
    }

    class PreAuthTransactionCompleted : Transactions
    {
        protected PreAuthTransaction completed;



        public PreAuthTransactionCompleted(PreAuthTransaction newCompleted, long newAmount)
            : base(newCompleted)
        {
            base.amount = newAmount;
        }
    }
}
