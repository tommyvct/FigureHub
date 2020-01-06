using System;
using System.Collections.Generic;
using System.Text;

namespace FigureHub.Models
{
    public enum MenuItemType
    {
        Overview,
        Transactions,
        Credit_Cards,
        Debit_Cards,

        Settings,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
