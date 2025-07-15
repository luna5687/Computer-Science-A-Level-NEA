using Computer_Science_A_Level_NEA;
using System.Data.SQLite;
// Copyright 2025 Daniel Ian White

// Bobs email: bob@ampretia.co.uk password: passw0rdWibble Mailserver: mail.ampretia.co.uk

namespace NEA_protoype
{

    internal class Program
    {

        static void Main(string[] args)
        {
            File.Delete("Email_Archive.db"); // REMOVE THIS LATER WHEN ALL TABLE CREATION IS DONE
           
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
                                  "(1,'Metting')," +
                                  "(2,'Accounting')," +
                                  "(3,'Important')", connection).ExecuteNonQuery();


      
            }
            ConsoleInteraction.CheckConsoleExistance();
           
            ImapServer Server = new ImapServer("bob@ampretia.co.uk", "passw0rdWibble", "mail.ampretia.co.uk");
           





            Console.ReadLine();

        }


    }

}

