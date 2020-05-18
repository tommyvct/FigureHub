using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TommyShell;

namespace FigureHub_CLI
{
    class FigureHub_CLI
    {
        static string homeDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FigureHub");
        static string db = Path.Combine(homeDir, "db.sqlite");
        static SQLiteConnection connection = new SQLiteConnection("Data Source=" + db);
        static int[] MonthLength = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                db = args[1];
            }

            Console.WriteLine("using " + db);


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


            ManualDebug(connection);
            Menu.DebugAction = () => ManualDebug(connection);

            AddNewCard();

            //Test.MenuTest();

            //Menu start = new Menu("Start");
            //start.AddItem(new Menu.Item("View transactions", () => ShowRecentTransactions()));
            //start.AddItem(new Menu.Item("Add transactions", () => AddTransactions()));
            //start.AddItem(new Menu.Item("Manage cards", () => ManageCards()));
            //start.Start();
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
            addcard.AddItem(new Menu.Item("Credit Card" , () => AddNewCardDetails(1), true));
            addcard.AddItem(new Menu.Item("Debit Card"  , () => AddNewCardDetails(2), true));
            addcard.AddItem(new Menu.Item("Prepaid Card", () => AddNewCardDetails(3), true));
            addcard.AddItem(new Menu.Item("Charge Card" , () => AddNewCardDetails(4), true));
            addcard.Start();
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

        /// <summary>
        /// Proved working
        /// </summary>
        /// <param name="type"></param>
        private static void AddNewCardDetails(int type)
        {


            List<Form.Item> formItems = new List<Form.Item>
            {
                new Form.Item("Card Name", (string s) => {return s == "" ? false : true;}),
                new Form.Item("Card Number", (string s) => {return s == "" ? false : true;}),
                new Form.Item("Valid Thru", (string s) =>
                {
                    try
                    {
                        if (Convert.ToInt32(s) < 0100 || Convert.ToInt32(s) > 1299)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (FormatException)
                    {
                        return false;
                    }

                }),
                new Form.Item("CVV", (string s) =>
                {
                    try
                    {
                        if (Convert.ToInt32(s) < 0 || Convert.ToInt32(s) > 9999)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (FormatException)
                    {
                        return false;
                    }
                }),
                new Form.Item("Cardholder Name", (string s) => {return s == "" ? false : true;}),
                new Form.Item("Balance", (string s) => {return (float.TryParse(s, out _)) ? true : false;}),
                new Form.Item("Currency (XXX)", (string s) => {return (s.All(char.IsLetter) && s.Length == 3) ? true : false;}),
                new Form.Item("Credit Limit", (string s) => {return (float.TryParse(s, out _)) ? true : false;}),
                new Form.Item("Cycle Date (DD)", (string s) => 
                {
                    try
                    {
                        if (Convert.ToInt32(s) < 0 || Convert.ToInt32(s) > 28) // avoid February and leap problem
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (FormatException)
                    {
                        return false;
                    }
                }),
                new Form.Item("Current Bill Due Date(MMDD)", (string s) => 
                {
                    try
                    {
                        int month = Convert.ToInt32(s) / 100;
                        int day   = Convert.ToInt32(s) % 100;
                        
                        if (MonthLength[month] < day || day < 1)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }),
                new Form.Item("Current Bill Balance", (string s) => {return (float.TryParse(s, out _)) ? true : false;})
                //new Form.Item("", (string s) => {}),
            };
            Form form = new Form("Enter the following information: ", formItems);
            form.Start();

            string CardName       = formItems[0].input;
            string CardNumber     = formItems[1].input;
            string ValidThru      = formItems[2].input;
            string CVV            = formItems[3].input;
            string CardholderName = formItems[4].input;
            string Balance        = formItems[5].input;
            string Currency       = formItems[6].input;
            string CreditLimit    = formItems[7].input;
            string CycleDate      = formItems[8].input;
            string DueDate        = formItems[9].input;
            string DueBalance     = formItems[10].input;

            //1 for Credit, 2 for Debit, 3 for Prepaid, 4 for Charge
            string CardType = type switch
            {
                1 => "Credit",
                2 => "Debit",
                3 => "Prepaid",
                4 => "Charge",
                _ => "",
            };

            string expression = $"INSERT INTO Cards (CardType, CardNumber, CardName, ValidThru, CardholderName, CVV, Currency, Balance, CreditLimit, CycleDate, DueDate, DueBalance, Active) " +
                           $"VALUES (\'{CardType}\', \'{CardNumber}\', \'{CardName}\', \'{ValidThru}\', \'{CardholderName}\', \'{CVV}\', \'{Currency}\', \'{Balance}\', \'{CreditLimit}\', \'{CycleDate}\', \'{DueDate}\', \'{DueBalance}\', \'1\')";

            //Console.WriteLine(expression);
            using var cmd = new SQLiteCommand(expression, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR:" + e.Message);
            }
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
            //using var cmd = new SQLiteCommand(connection);
            Console.Clear();

            string[] expression;

            while (true)
            {
                Console.Write("> ");
                expression = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (expression.Length == 0)
                {
                    continue;
                }
                else if (expression[0] == "quit" || expression[0] == "exit")
                {
                    break;
                }
                else if (expression.Length == 0)
                {
                    continue;
                }
                else if (expression[0] == "NUKE")
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
                else if (expression[0] == "sqlite")
                {
                    try
                    {
                        ProcessStartInfo p = new ProcessStartInfo(".\\sqlite3\\sqlite3.exe")
                        {
                            UseShellExecute = true,
                            CreateNoWindow = false,
                            WindowStyle = ProcessWindowStyle.Normal,
                            Arguments = (expression.Length == 1) ? db : string.Join(' ', expression.Skip(1))
                        };
                        Process.Start(p);//.WaitForExit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        foreach (string s in expression)
                        {
                            Console.WriteLine(s, ' ');
                        }
                    }
                }
                else
                {
                    Console.WriteLine("command not found.");
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
                    CardType        TEXT,
                    CardNumber      TEXT PRIMARY KEY,
                    CardName        TEXT,
                    ValidThru       DATE,
                    CardholderName  TEXT,
                    CVV             TEXT,
                    Currency        CHAR[3] NOT NULL,
                    Balance         REAL,

                    CreditLimit     REAL,
                    CycleDate       DATE,
                    DueDate         DATE,
                    DueBalance      REAL,
                    Active          BOOL NOT NULL
                );";
                cmd.ExecuteNonQuery();

                // rowid PRIMARY KEY
                cmd.CommandText = @"CREATE TABLE BankAccount 
                (
                    BankAccountNumber              TEXT PRIMARY KEY,
                    Balance                        REAL,
                    Currency                       CHAR[3] NOT NULL,
                    Active                         BOOL NOT NULL,
                    AccountName                    TEXT,
                    InstitutionNumber              TEXT,
                    TransitNumber                  TEXT,
                    Comments                       TEXT,
                    LinkedCardNumber               TEXT,
                    FOREIGN KEY (LinkedCardNumber) REFERENCES Cards(CardNumber)
                );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE Transactions 
                (
                    Description                     TEXT,
                    Amount                          REAL,
                    Date                            DATE,
                    Pending                         BOOL,
                    ForeignCurrencyAmount           REAL,
                    ForeignCurrency                 CHAR[3],
                    CardNumber                      TEXT,
                    BankAccountNumber               TEXT,
                    FOREIGN KEY (CardNumber)        REFERENCES Cards(CardNumber),
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