using System;
using System.Collections.Generic;

namespace TommyShell
{
    public class Menu
    {
        public string title;
        public List<Item> items;
        public static Action DebugAction { get; set; } = null;
        public static char DebugSwitch { get; set; } = 'D';
        public static char GoUpSwitch { get; set; } = 'C';

        public Menu(string title)
        {
            this.title = title;
            this.items = new List<Item>();
        }

        public Menu(string title, List<Item> items)
        {
            this.title = title;
            this.items = items;
        }

        private Menu() { }

        public void Start(bool newScreen = true)
        {
            while (true)  // main loop
            {
                if (newScreen)
                {
                    Console.Clear();
                }

                Console.WriteLine(title);
                foreach (char c in title)
                {
                    Console.Write('=');
                }
                Console.WriteLine();
                Console.WriteLine();

                for (int i = 0; i < items.Count; i++)  // display items
                {
                    Console.WriteLine("    " + (i + 1).ToString() + ". " + items[i].label);
                }

                Console.WriteLine();

                while (true)  // interaction loop
                {
                    Console.Write($"Choose from 1 to {items.Count}, or {GoUpSwitch} for top: ");

                    char key = char.ToUpper(Console.ReadKey().KeyChar);
                    Console.WriteLine();

                    if (key == GoUpSwitch)
                    {
                        return; // to previous menu loop
                    }
                    else if (key == DebugSwitch)
                    {
                        DebugAction?.Invoke();
                    }
                    else
                    {
                        int i = (int)char.GetNumericValue(key);
                        if (i == -1 || i > items.Count)
                        {
                            Console.WriteLine("Invalid answer.");
                            continue;
                        }
                        else
                        {
                            items[i - 1].Action();

                        }
                    }
                }
            }
        }

        public void AddItem(Item toAdd)
        {
            items.Add(toAdd);
        }

        public class Item
        {
            public string label;
            public Action action;

            public Item(string label, Action action)
            {
                this.label = label;
                this.action = action;
            }

            public void Action()
            {
                action?.Invoke();
            }
        }
    }
}
