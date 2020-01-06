﻿using System;

namespace FigureHub.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }

    public enum CardType
    {
        Credit,
        Debit
    }
}
