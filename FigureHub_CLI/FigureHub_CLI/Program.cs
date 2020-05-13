using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

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
            SQLiteConnection connection;

            do
            {
                try
                {
                    connection = new SQLiteConnection("Data Source=" + db);
                    connection.Open();
                }
                catch (Exception e)
                {
                    connection = null;
                    //Console.WriteLine(e);
                    Console.WriteLine("DB does not exist. Would you like to create a new DB? (y/N)");

                    if (Console.ReadKey().Key != ConsoleKey.Y)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Abort.");
                        Environment.Exit(1);
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                    try
                    {
                        if (!Directory.Exists(homeDir))
                        {
                            Directory.CreateDirectory(homeDir);
                        }

                        SQLiteConnection.CreateFile(db);
                        InitDB(connection);
                        Console.WriteLine("New database will be at " + db);
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine("ERROR: Cannot create database file.");
                        Console.WriteLine(e);
                        Console.WriteLine("Abort.");
                    }

                }

            }
            while (connection == null);

            ManualDebug(connection);

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
                        File.Delete(db);
                        Console.WriteLine("Peace.");
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
                LinkedCardNumber FOREIGN KEY REFERENCE Cards(CardNumber)
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
                CardNumber FOREIGN KEY REFERENCE Cards(CardNumber),
                BankAccountNumber FOREIGN KEY REFERENCE BankAccount(BankAccountNumber)
            );";
            cmd.ExecuteNonQuery();


        }
    }
}
