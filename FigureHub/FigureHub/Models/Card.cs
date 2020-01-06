using System;

namespace FigureHub.Models
{
    public class Card
    {
        protected int Balance;
        protected string Number
        {
            get
            {
                return Number;
            }
            set
            {
                if (ValidateCardNumber(value))
                    Number = value;
            }
        }
        protected DateTime MMYY { get; set; }
        protected string CVV
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
        protected string IssueBank;
        protected CardType Type;

        protected static bool ValidateCardNumber(string toValidate)
        {
            if (toValidate.Trim().Length != 16 || toValidate.Trim().Length != 19)
                return false;

            for (int i = 0; i < 16; i++)
            {
                if (!char.IsDigit(toValidate[i]))
                    return false;
            }

            return true;
        }

        public Card(string newNumber, DateTime newMMYY, string newCVV, string newIssuebank, CardType newType)
        {
            Number = newNumber;
            MMYY = newMMYY;
            CVV = newCVV;
            IssueBank = newIssuebank;
            Type = newType;
            Balance = 0;
        }
    }
}
