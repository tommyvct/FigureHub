using System;
using System.Collections.Generic;

namespace FigureHub.Models
{
    public enum CardType
    {
        Credit,
        Charge,
        Debit
    }

    public class Card
    {
        long CreditLimit;
        long AvailCredit;
        long Balance;            // non-converted multicurrency balance?????
        long MinimumPayment;
        DateTime BillingDate;
        DateTime Payday;


        string Number
        {
            get
            {
                return Number;
            }
            set
            {
                Number = value;
            }
        }
        DateTime MMYY { get; set; }
        string CVV
        {
            get
            {
                return CVV;
            }
            set
            {
                if (CVV.Length == 3)
                    CVV = value;
            }
        }
        string name;

        List<Account> AccountList;

        string IssueBank;
        CardType Type;

        Card(string newName, string newNumber, DateTime newMMYY, string newCVV, string newIssuebank, CardType newType)
        {
            name = newName;
            Number = newNumber;
            MMYY = newMMYY;
            CVV = newCVV;
            IssueBank = newIssuebank;
            Type = newType;
            Balance = 0;
        }

        void AddAccount(Account[] newAccounts)
        {
            foreach (var a in newAccounts)
            {
                AccountList.Add(a);
            }
        }


    }
}
