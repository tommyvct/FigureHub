using System;
using System.Collections.Generic;
using System.Linq;

namespace TommyShell
{
    public abstract class TommyShell
    {
        public static Action DebugAction { get; set; } = null;
        public static char DebugSwitch { get; set; } = 'D';
        public static char GoUpSwitch { get; set; } = 'C';

        public string title;

        protected TommyShell(string title)
        {
            this.title = title;
        }

        private TommyShell() { }

        protected void PrintTitle(bool newScreen)
        {
            if (newScreen)
            {
                Console.Clear();
            }

            Console.WriteLine(title);
            foreach (char _ in title)
            {
                Console.Write('=');
            }
            Console.WriteLine();
            Console.WriteLine();
        }

    }

    public abstract class ShellItem
    {
        public string label;
        public bool refreshAfterAction;

        protected ShellItem(string label, bool refreshAfterAction)
        {
            this.label = label;
            this.refreshAfterAction = refreshAfterAction;
        }

        private ShellItem() { }
    }

    public class Menu : TommyShell
    {
        public List<Item> items;

        public Menu(string title) : base(title) 
        {
            items = new List<Item>();
        }

        public Menu(string title, List<Item> items) : base(title) 
        {
            this.items = items;
        }

        public void Start(bool newScreen = true)
        {
            while (true)  // main loop
            {
                PrintTitle(newScreen);

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
                        break;
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
                            if (items[i - 1].refreshAfterAction)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }

        public void AddItem(Item toAdd)
        {
            items.Add(toAdd);
        }

        public class Item : ShellItem
        {
            public Action action;

            public Item(string label, Action action, bool refreshAfterAction = false) : base(label, refreshAfterAction)
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

    public class Form : TommyShell
    {
        public List<Item> items;

        public Form(string title) : base(title) 
        {
            items = new List<Item>();
        }

        public Form(string title, List<Item> items) : base(title) 
        {
            this.items = items;
        }

        public void Start(bool newScreen = true)
        {
            while (true) // maybe add function of refreshAfterAction?
            {
                PrintTitle(newScreen);

                for (int i = 0; i < items.Count; i++)  // display items
                {
                    while (true)
                    {
                        Console.Write("    " + (i + 1).ToString() + ". " + items[i].label + ": ");
                        string temp = Console.ReadLine();
                        
                        if (items[i].isValid(temp))
                        {
                            items[i].input = temp;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                    }
                }

                while (true)
                {
                    Console.Write("Would you like to make corrections? (Y/n) ");

                    var key = Console.ReadKey().Key;

                    if (key == ConsoleKey.Y || key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        Console.Write($"Which field to correct? (1 ...{items.Count} , [C]ancel)  ");
                        string s = Console.ReadLine();

                        if (s == "")
                        {
                            continue;
                        }
                        else if (char.ToUpper(s[0]) == 'C')
                        {
                            break;
                        }
                        else
                        {
                            int i;

                            try
                            {
                                i = Convert.ToInt32(s);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Invalid input.");
                                continue;
                            }

                            if (!Enumerable.Range(1, items.Count).Contains(i))
                            {
                                Console.WriteLine("Invalid input.");
                                continue;
                            }
                            else
                            {
                                while (true)
                                {
                                    Console.Write("    " + (i).ToString() + ". " + items[i - 1].label + ": ");
                                    string temp = Console.ReadLine();

                                    if (items[i - 1].isValid(temp))
                                    {
                                        items[i - 1].input = temp;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input.");
                                    }
                                }
                            }
                        }
                    }
                    else if (key == ConsoleKey.N)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid input.");
                        continue;
                    }
                }
            }
        }

        public void AddItem(Item toAdd)
        {
            items.Add(toAdd);
        }

        public class Item : ShellItem
        {
            private Func<string, bool> valid;
            public string input;

            public Item(string label, Func<string, bool> valid, bool refreshAfterAction = false) : base(label, refreshAfterAction)
            {
                this.label = label;
                this.valid = valid;
            }

            public bool isValid(string s)
            {
                return valid(s);
            }
        }
    }

    public class Test
    {
        public static void MenuTest()
        {
            Menu start = new Menu("Start");
            start.AddItem(new Menu.Item("First Item", () => Console.WriteLine("First Item Choose!")));
            start.AddItem(new Menu.Item("Second Item", () => Console.WriteLine("Second Item Choose!")));
            start.AddItem(new Menu.Item("Go deeper", () =>
            {
                Menu deep = new Menu("Deeper");
                deep.AddItem(new Menu.Item("Go deeper.First Item", () => Console.WriteLine("Go deeper.First Item Choose!")));
                deep.AddItem(new Menu.Item("Go deeper.Second Item", () => Console.WriteLine("Go deeper.Second Item Choose!")));
                deep.Start();
            }, true));
            start.Start();
        }
    }
}
