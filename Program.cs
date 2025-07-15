using Computer_Science_A_Level_NEA;
using System.Data.SQLite;
// Copyright 2025 Daniel Ian White

// Bobs email: bob@ampretia.co.uk password: passw0rdWibble Mailserver: mail.ampretia.co.uk

namespace NEA_protoype
{

    internal class Program
    {
        static void AccountsMenu()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Email_Archive.db;Version=3;New=True;Compress=True;");
            connection.Open();
            SQLiteDataReader DR = new SQLiteCommand("SELECT AccountName FROM Accounts",connection).ExecuteReader();
            if (DR.StepCount == 0 )
            {
                DR.Close();
                Console.WriteLine("No accounts found \nAccount creation requred press enter to continue");
                Console.ReadLine();
                Console.Clear();
                Console.Write("Enter Account Username: ");
                string accountname = Console.ReadLine();
                Console.Write("Enter Account Password (Please note it is not hidden): ");
                string accountPassword = Console.ReadLine();
                new SQLiteCommand("INSERT INTO Accounts(AccountName,Password)" +
                                  "VALUES " +
                                  $"('{accountname}','{accountPassword}');",connection).ExecuteNonQuery();
                DR = new SQLiteCommand("SELECT AccountName FROM Accounts", connection).ExecuteReader();
                Console.Clear();
            }
            List<string> Accounts = new List<string>();
           while (DR.Read())
            {
                Console.WriteLine(DR["AccountName"]);
            }
            // to finish accounts menu 
            // call email menu
            
            
            connection.Close();
            
        }
        static void Main(string[] args)
        {
            
           
            if (!File.Exists("Email_Archive.db"))
            {
                SQLiteConnection.CreateFile("Email_Archive.db");
                SQLiteConnection connection = new SQLiteConnection("Data Source=Email_Archive.db;Version=3;New=True;Compress=True;");
                connection.Open();
                new SQLiteCommand("CREATE TABLE Users (" +
                                  "EmailAddress varchar PRIMARY KEY," +
                                  "Password varchar," +
                                  "Mailserver varchar," +
                                  "Account varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Emails(" +
                                  "EmailID int PRIMARY KEY," +
                                  "Sender varchar," +
                                  "Recipient varchar," +
                                  "Subject varchar," +
                                  "TextBody varchar," +
                                  "Keywords)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Tags(" +
                                  "TagID int PRIMARY KEY," +
                                  "TagName varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE AssignedTags(" +
                                  "TagID int," +
                                  "EmailID int)",connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Accounts(" +
                                  "AccountName varchar PRIMARY KEY," +
                                  "Password varchar)",connection ).ExecuteNonQuery();
                new SQLiteCommand("INSERT INTO Tags(tagID,TagName)" +
                                  "VALUES " +
                                  "(0,'Metting')," +
                                  "(1,'Accounting')," +
                                  "(2,'Important')", connection).ExecuteNonQuery();

                connection.Close();
            }
            ConsoleInteraction.CheckConsoleExistance();
            AccountsMenu();
            ImapServer Server = new ImapServer("bob@ampretia.co.uk", "passw0rdWibble", "mail.ampretia.co.uk");
            






            Console.ReadLine();

        }


    }

}

