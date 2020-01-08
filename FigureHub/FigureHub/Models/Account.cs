using System;
using System.Collections.Generic;
using System.Text;

namespace FigureHub.Models
{
    public enum AccountType
    {
        Credit,
        Cheque,
        Saving
    }

    class Account
    {
        protected AccountType type;
        protected string currency;
        protected long balance;
        protected double interestRate;  // only for infomation

        /// <summary>
        /// Construct an Account. Could be Credit, Cheque or saving account.
        /// A credit card can have multiple Credit accounts, usually in different currency.
        /// A debit card can also have multiple cheque and saving account, in the same or different currency.
        /// </summary>
        /// <param name="newType"></param>
        /// <param name="newCurrency"></param>
        /// <param name="newinterestRate"></param>
        public Account(AccountType newType, string newCurrency, double newinterestRate)
        {
            type = newType;
            currency = newCurrency;
            balance = 0;
            interestRate = newinterestRate;
        }
    }
}
