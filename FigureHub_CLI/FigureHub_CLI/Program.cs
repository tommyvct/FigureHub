using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using TommyShell;

namespace FigureHub_CLI
{
    class Program
    {
        static string homeDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FigureHub");
        static string db = Path.Combine(homeDir, "db.sqlite");

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                db = args[1];
            }

            Console.WriteLine("using " + db);
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + db);


            if (!File.Exists(db))
            {
                //Console.WriteLine(e);
                Console.WriteLine("DB does not exist. Would you like to create a new DB? (y/N)");

                if (Console.ReadKey().Key != ConsoleKey.Y)
                {
                    Console.WriteLine();
                    Console.WriteLine("Abort.");
                    Environment.Exit(1);
                }

                Console.WriteLine();

                try
                {
                    if (!Directory.Exists(homeDir))
                    {
                        Directory.CreateDirectory(homeDir);
                    }

                    //connection = new SQLiteConnection("Data Source=" + db);
                    connection.Open();

                    InitDB(connection);
                    Console.WriteLine("New database will be at " + db);
                }
                catch (Exception ee)
                {
                    Console.WriteLine("ERROR: Cannot create database file.");
                    Console.WriteLine(ee);
                    Console.WriteLine("Abort.");
                    Environment.Exit(1);
                }
            }
            else
            {
                //connection = new SQLiteConnection("Data Source=" + db);
                connection.Open();
            }

            //ManualDebug(connection);

            //Menu start = new Menu("Start");
            //start.AddItem(new Menu.Item("First Item", () => Console.WriteLine("First Item Choose!")));
            //start.AddItem(new Menu.Item("Second Item", () => Console.WriteLine("Second Item Choose!")));
            //start.AddItem(new Menu.Item("Go deeper", () =>
            //{
            //    Menu deep = new Menu("Deeper");
            //    deep.AddItem(new Menu.Item("Go deeper.First Item", () => Console.WriteLine("Go deeper.First Item Choose!")));
            //    deep.AddItem(new Menu.Item("Go deeper.Second Item", () => Console.WriteLine("Go deeper.Second Item Choose!")));
            //    deep.Start();
            //}));
            //start.Start();
            Menu.DebugAction = () => ManualDebug(connection);
            Menu start = new Menu("Start");
            start.AddItem(new Menu.Item("View transactions", () => ShowRecentTransactions()));
            start.AddItem(new Menu.Item("Add transactions" , () => AddTransactions()));
            start.AddItem(new Menu.Item("Manage cards"     , () => ManageCards()));
            start.Start();
        }

        private static void ManageCards()
        {
            Menu manageCards = new Menu("Manage cards");
            // display cards based on the usage
            manageCards.AddItem(new Menu.Item("Add a new card", () => AddNewCard()));

            throw new NotImplementedException();
        }

        private static void AddNewCard()
        {
            Menu addcard = new Menu("New Card");
            addcard.AddItem(new Menu.Item("Credit Card" , () => AddNewCardDetails(1)));
            addcard.AddItem(new Menu.Item("Debit Card"  , () => AddNewCardDetails(2)));
            addcard.AddItem(new Menu.Item("Prepaid Card", () => AddNewCardDetails(3)));
            addcard.AddItem(new Menu.Item("Charge Card" , () => AddNewCardDetails(4)));
            addcard.Start();

            throw new NotImplementedException();
        }



        ////////////////VIEW//////////////////////////

        private static void ShowRecentTransactions()
        {
            throw new NotImplementedException();
        }

        private static List<string> ShowTransactions(int from, int to = 0)
        {
            throw new NotImplementedException();
        }

        private static List<string> ShowTransactions(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        private static List<string> ShowTransactions(DateTime from)
        {
            throw new NotImplementedException();
        }

        private static List<string> ShowCards()
        {
            throw new NotImplementedException();
        }

        

        ////////////////Controller//////////////////////////

        private static void AddNewCardDetails(int type)
        {
            // 1 for Credit, 2 for Debit, 3 for Prepaid, 4 for Charge
            throw new NotImplementedException();
        }

        private static void AddBankAccount()
        {
            throw new NotImplementedException();
        }

        private static void AddTransactions()
        {
            // choose card, then fill in details
            throw new NotImplementedException();
        }

        private static void ModifyTransactions()
        {
            throw new NotImplementedException();
        }

        private static void ModifyBankAccount()
        {
            throw new NotImplementedException();
        }

        private static void ModifyCardDetails()
        {
            throw new NotImplementedException();
        }

        static void ManualDebug(SQLiteConnection connection)
        {
            using var cmd = new SQLiteCommand(connection);

            string expression = "";

            while (true)
            {
                Console.Write("SQL > ");
                expression = Console.ReadLine();

                if (expression == "quit" || expression == "exit")
                {
                    break;
                }
                else if (expression == "NUKE")
                {
                    Console.WriteLine("ARE YOU SURE? (y/N)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        Console.WriteLine();
                        connection.Close();
                        NukeDB();
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine();
                    }


                }
                else if (expression == "")
                {
                    continue;
                }
                else
                {
                    try
                    {
                        cmd.CommandText = expression;
                        Console.WriteLine("" + cmd.ExecuteNonQuery() + " rows updated.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR:" + e.Message);
                    }

                }
            }
        }

        static void InitDB(SQLiteConnection connection)
        {
            using var cmd = new SQLiteCommand(connection);

            try
            {
                cmd.CommandText = @"CREATE TABLE Cards 
                (
                    CardType TEXT,
                    CardNumber TEXT PRIMARY KEY,
                    CardName TEXT,
                    ValidThru DATE,
                    CardholderName TEXT,
                    CVV TEXT,
                    Currency CHAR[3] NOT NULL,
                    Balance REAL,
                    DueDate DATE,
                    DueBalance REAL,
                    Active BOOL NOT NULL
                );";
                cmd.ExecuteNonQuery();

                // rowid PRIMARY KEY
                cmd.CommandText = @"CREATE TABLE BankAccount 
                (
                    BankAccountNumber TEXT PRIMARY KEY,
                    Balance REAL,
                    Currency CHAR[3] NOT NULL,
                    Active BOOL NOT NULL,
                    Comments TEXT,
                    LinkedCardNumber TEXT,
                    FOREIGN KEY (LinkedCardNumber) REFERENCES Cards(CardNumber)
                );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE Transactions 
                (
                    Description TEXT,
                    Amount REAL NOT NULL,
                    Currency CHAR[3] NOT NULL,
                    Date DATE,
                    Pending BOOL,
                    ForeignCurrencyAmount REAL,
                    ForeignCurrency CHAR[3],
                    CardNumber TEXT,
                    BankAccountNumber TEXT,
                    FOREIGN KEY (CardNumber) REFERENCES Cards(CardNumber),
                    FOREIGN KEY (BankAccountNumber) REFERENCES BankAccount(BankAccountNumber)
                );";
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connection.Close();
                NukeDB();
                Environment.Exit(3);
            }

        }

        static void NukeDB()
        {
            File.Delete(db);
            Console.WriteLine("Peace.");
        }
    }

    
}