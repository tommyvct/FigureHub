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
        string name;
        AccountType type;
        string currency;
        long balance;
        double interestRate;  // only for infomation

        /// <summary>
        /// Construct an Account. Could be Credit, Cheque or saving account.
        /// A credit card can have multiple Credit accounts, usually in different currency.
        /// A debit card can also have multiple cheque and saving account, in the same or different currency.
        /// </summary>
        /// <param name="newType"></param>
        /// <param name="newCurrency"></param>
        /// <param name="newinterestRate"></param>
        Account(string newname, AccountType newType, string newCurrency, double newinterestRate)
        {
            name = newname;
            type = newType;
            currency = newCurrency;
            balance = 0;
            interestRate = newinterestRate;
        }
    }
}
