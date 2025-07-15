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
            ConsoleInteraction.CheckConsoleExistance();
            ImapServer Server = new ImapServer("bob@ampretia.co.uk", "passw0rdWibble", "mail.ampretia.co.uk");

            File.Delete("Email_Archive.db");
            if (!File.Exists("Email_Archive.db"))
            {
                SQLiteConnection.CreateFile("Email_Archive.db");
                SQLiteConnection connection = new SQLiteConnection("Data Source=Email_Archive.db;Version=3;New=True;Compress=True;");
                connection.Open();
                new SQLiteCommand("CREATE TABLE Users (" +
                                  "EmailAddress varchar PRIMARY KEY," +
                                  "Password varchar," +
                                  "Mailserver varchar," +
                                  "Account varchar)",connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Emails(" +
                                  "EmailID int PRIMARY KEY," +
                                  "Sender varchar," +
                                  "Recipient varchar," +
                                  "Subject varchar," +
                                  "TextBody varchar," +
                                  "Keywords)",connection ).ExecuteNonQuery();
            }
            
           

        }


    }

}

