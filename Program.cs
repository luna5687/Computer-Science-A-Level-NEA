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
            SQLiteDataReader DR = new SQLiteCommand("SELECT AccountName FROM Accounts", connection).ExecuteReader();
            if (DR.StepCount == 0)
            {
                DR.Close();
                Console.WriteLine("No accounts found \nAccount creation requred press enter to continue");
                Console.ReadLine();
                Console.Clear();
                Console.Write("Enter Account Username: ");
                string accountname = Console.ReadLine();
                Console.Write("Enter Account Password (Please note it is not hidden): ");

                string accountPassword = Console.ReadLine();
                while (accountPassword == "")
                {
                    Console.WriteLine("invalid password");
                    Console.Write("Enter Account Password (Please note it is not hidden): ");

                    accountPassword = Console.ReadLine();
                }
                new SQLiteCommand("INSERT INTO Accounts(AccountName,Password)" +
                                  "VALUES " +
                                  $"('{accountname}','{Encryption.Encrypt(accountPassword)}');", connection).ExecuteNonQuery();
                DR = new SQLiteCommand("SELECT AccountName FROM Accounts", connection).ExecuteReader();
                Console.Clear();
            }
            List<string> Accounts = new List<string>();
            while (DR.Read())
            {
                Accounts.Add(DR["AccountName"].ToString());
            }
            bool exit = false;
            int menuOption = 0;
            while (!exit)
            {
                if (menuOption == Accounts.Count)
                {
                    for (int i = 0; i < Accounts.Count; i++)
                    {
                        Console.Write("   " + Accounts[i] + "\n");
                    }
                    Console.Write(" > Exit");
                }
                else
                {
                    for (int i = 0; i < Accounts.Count; i++)
                    {
                        if (i == menuOption)
                        {
                            Console.Write(" > " + Accounts[i] + "\n");
                        }
                        else
                        {
                            Console.Write("   " + Accounts[i] + "\n");
                        }
                    }
                    Console.Write("   Exit");
                }
                string input = ConsoleInteraction.GetConsoleInput(true).ToUpper();

                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = Accounts.Count;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > Accounts.Count)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == Accounts.Count)
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write($"Please enter password for Account: {Accounts[menuOption]}.\nPress ENTER when input is empty to cancel\n" +
                            $"");
                        string EnteredPassword = Console.ReadLine();
                        bool passwordConfined = false;
                        DR = new SQLiteCommand($"SELECT Password FROM Accounts WHERE AccountName = '{Accounts[menuOption]}'", connection).ExecuteReader();
                        DR.Read();

                        while (EnteredPassword != "" && !passwordConfined)
                        {
                            if (EnteredPassword == Encryption.Decrypt(DR["Password"].ToString()))
                            {
                                DR.Close();
                                passwordConfined = true;
                                EmailAddressesMenu(Accounts[menuOption]);
                            }
                            else
                            {
                                Console.CursorTop = 0;
                                Console.CursorLeft = 0;
                                Console.WriteLine("Password invalid                              ");

                                Console.Write($"Please enter password for Account: {Accounts[menuOption]}.\nPress ENTER when input is empty to cancel                                                         \n                                                  ");
                                Console.CursorLeft = 0;
                                EnteredPassword = Console.ReadLine();
                            }
                        }
                        Console.Clear();
                    }

                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }
            connection.Close();
        }

        static void EmailAddressesMenu(string accountName)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Email_Archive.db;Version=3;New=True;Compress=True;");
            connection.Open();
            SQLiteDataReader DR = new SQLiteCommand("SELECT EmailAddress FROM Users", connection).ExecuteReader();
            if (DR.StepCount == 0)
            {
                DR.Close();

                Console.WriteLine("No EmailAddress found \nEmailAddress input requred press enter to continue");
                Console.ReadLine();
                Console.Clear();
                Console.Write("Enter Emailaddress: ");
                string EmailAddress = Console.ReadLine();
                Console.Write("Enter the email's mail server: ");
                string MailServer = Console.ReadLine();
                Console.Write("Enter EmailAddress Password (Please note it is not hidden): ");
                // TODO - validate password 
                string EmailPassword = Console.ReadLine();

                new SQLiteCommand("INSERT INTO Users(EmailAddress,Password,Mailserver,Account)" +
                                  "VALUES " +
                                  $"('{EmailAddress}','{Encryption.Encrypt(EmailPassword)}','{MailServer}','{accountName}');", connection).ExecuteNonQuery();
                DR = new SQLiteCommand("SELECT EmailAddress FROM Users", connection).ExecuteReader();
                Console.Clear();


            }
            List<string> EmaliAddreses = new List<string>();
            while (DR.Read())
            {
                EmaliAddreses.Add(DR["EmailAddress"].ToString());
            }
            bool exit = false;
            int menuOption = 0;
            Console.Clear();
            while (!exit)
            {
                if (menuOption == EmaliAddreses.Count)
                {
                    for (int i = 0; i < EmaliAddreses.Count; i++)
                    {
                        Console.Write("   " + EmaliAddreses[i] + "\n");
                    }
                    Console.WriteLine(" > Manage Emails");
                    Console.Write("   Exit");
                }
                else if (menuOption == EmaliAddreses.Count + 1)
                {
                    for (int i = 0; i < EmaliAddreses.Count; i++)
                    {
                        Console.Write("   " + EmaliAddreses[i] + "\n");
                    }
                    Console.WriteLine("   Manage Emails");
                    Console.Write(" > Exit");
                }
                else
                {
                    for (int i = 0; i < EmaliAddreses.Count; i++)
                    {
                        if (i == menuOption)
                        {
                            Console.Write(" > " + EmaliAddreses[i] + "\n");
                        }
                        else
                        {
                            Console.Write("   " + EmaliAddreses[i] + "\n");
                        }
                    }
                    Console.WriteLine("   Manage Emails");
                    Console.Write("   Exit");
                }
                string input = ConsoleInteraction.GetConsoleInput(true).ToUpper();

                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = EmaliAddreses.Count + 1;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > EmaliAddreses.Count + 1)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == EmaliAddreses.Count + 1)
                    {
                        exit = true;
                    }
                    else if (menuOption == EmaliAddreses.Count)
                    {
                        DR.Close();
                        EmailManagement(EmaliAddreses, connection, accountName);
                        DR = new SQLiteCommand("SELECT EmailAddress FROM Users", connection).ExecuteReader();
                        if (DR.StepCount == 0)
                        {
                            DR.Close();

                            Console.WriteLine("No EmailAddress found \nEmailAddress input requred press enter to continue");
                            Console.ReadLine();
                            Console.Clear();
                            Console.Write("Enter Emailaddress: ");
                            string EmailAddress = Console.ReadLine();
                            Console.Write("Enter the email's mail server: ");
                            string MailServer = Console.ReadLine();
                            Console.Write("Enter EmailAddress Password (Please note it is not hidden): ");
                            // TODO - validate password 
                            string EmailPassword = Console.ReadLine();
                            new SQLiteCommand("INSERT INTO Users(EmailAddress,Password,Mailserver,Account)" +
                                              "VALUES " +
                                              $"('{EmailAddress}','{Encryption.Encrypt(EmailPassword)}','{MailServer}','{accountName}');", connection).ExecuteNonQuery();
                            DR = new SQLiteCommand("SELECT EmailAddress FROM Users", connection).ExecuteReader();
                            Console.Clear();


                        }
                        EmaliAddreses = new List<string>();
                        while (DR.Read())
                        {
                            EmaliAddreses.Add(DR["EmailAddress"].ToString());
                        }
                    }
                    else
                    {
                        DR = new SQLiteCommand($"SELECT * FROM users WHERE EmailAddress = '{EmaliAddreses[menuOption]}'", connection).ExecuteReader();
                        DR.Read();
                        EmailMenu(DR["EmailAddress"].ToString(), Encryption.Decrypt(DR["Password"].ToString()), DR["Mailserver"].ToString());
                        Console.Clear();
                    }

                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }
            connection.Close();

            // work on text rank on email and automate archive
        }
        static void EmailMenu(string EmailAddress, string EmailPassword, string Mailserver)
        {
            ImapServer imapServer = new ImapServer(EmailAddress, EmailPassword, Mailserver);
            LoadedEmails emalis = new LoadedEmails(imapServer);
            emalis.EmailMenu();
        }
        static void EmailManagement(List<string> EmaliAddreses, SQLiteConnection connection, string accountName)
        {
            Console.Clear();
            bool exit = false;
            int menuOption = 0;
            while (!exit)
            {
                if (menuOption == 0)
                {
                    Console.WriteLine(" > Delete email");
                    Console.WriteLine("   Add email");
                    Console.Write("   Exit");
                }
                else if (menuOption == 1)
                {
                    Console.WriteLine("   Delete email");
                    Console.WriteLine(" > Add email");
                    Console.Write("   Exit");
                }
                else
                {
                    Console.WriteLine("   Delete email");
                    Console.WriteLine("   Add email");
                    Console.Write(" > Exit");
                }
                string input = ConsoleInteraction.GetConsoleInput(true).ToUpper();
                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = 2;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > 2)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == 0)
                    {
                        DeleteEmails(EmaliAddreses, connection);
                        exit = true;
                    }
                    else if (menuOption == 1)
                    {
                        AddEmail(EmaliAddreses, connection, accountName);
                    }
                    else
                    {
                        exit = true;
                    }
                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }
        }
        static void DeleteEmails(List<string> EmailAddresses, SQLiteConnection connection)
        {
            foreach (string EmailAddress in EmailAddresses)
            {
                new SQLiteCommand($"DELETE FROM Users WHERE EmailAddress == '{EmailAddress}'", connection).ExecuteNonQuery();
                new SQLiteCommand($"DELETE FROM Emails WHERE EmailAddress == '{EmailAddress}'", connection).ExecuteNonQuery();
            }
        }
        static void AddEmail(List<string> EmailAddresses, SQLiteConnection connection, string accountName)
        {
            Console.Clear();
            Console.Clear();
            Console.Write("Enter Emailaddress: ");
            string EmailAddress = Console.ReadLine();
            if (EmailAddresses.Contains(EmailAddress))
            {
                Console.WriteLine("Email address already exists");
                Console.Write("Enter Emailaddress: ");
                EmailAddress = Console.ReadLine();
            }
            Console.Write("Enter the email's mail server: ");
            string MailServer = Console.ReadLine();
            Console.Write("Enter EmailAddress Password (Please note it is not hidden): ");
            // TODO - validate password 
            string EmailPassword = Console.ReadLine();

            new SQLiteCommand("INSERT INTO Users(EmailAddress,Password,Mailserver,Account)" +
                              "VALUES " +
                              $"('{EmailAddress}','{Encryption.Encrypt(EmailPassword)}','{MailServer}','{accountName}');", connection).ExecuteNonQuery();

            Console.Clear();
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
                                  "Keywords varchar," +
                                  "EmailAddress varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Tags(" +
                                  "TagID int PRIMARY KEY," +
                                  "TagName varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE AssignedTags(" +
                                  "TagID int," +
                                  "EmailID int)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Accounts(" +
                                  "AccountName varchar PRIMARY KEY," +
                                  "Password varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("INSERT INTO Tags(tagID,TagName)" +
                                  "VALUES " +
                                  "(0,'Metting')," +
                                  "(1,'Accounting')," +
                                  "(2,'Important')", connection).ExecuteNonQuery();

                connection.Close();
            }
            ConsoleInteraction.CheckConsoleExistance();
            //AccountsMenu();

            // testing text rank delete after finishing 

            char[] Body = "Dear All\r\n\r\nTrip to Leonardo event\r\nTuesday 16th September\r\n\r\nWe (CANSAT teams) have been invited to attend a special event at Leonard Southampton to celebrate their new space technology. \r\n\r\nMore details below on the technology.\r\nDate: 16th September \r\nI will take you there and back in minibus.\r\n\r\nWill involve some talks and a tour and lunch\r\n\r\nWe have been given 12 tickets. So it is first come first serves.\r\n\r\nIf you would like to come on this trip then please email ME (not Leonardo!) letting me know that you want to come and any dietary requirements AS SOON AS POSSIBLE. \r\nThey want to know by 1st August ideally, but just let me know as soon as you can.\r\n\r\nALSO Leonardo have asked us to not tell anyone about this event till afterwards!!\r\nAny questions let me know.".ToCharArray();
            List<string> FilteredWords = new List<string>();
            string[] WordsToFilter = { " the ", "\n", "\r", " is ", " and ", " a " };
            string Temp = "";
            string text = "";

            foreach (string word in WordsToFilter)
            {
                for (int i = 0; i < Body.Length - word.Length; i++)
                {
                    Temp = "";
                    for (int j = 0; j < word.Length; j++)
                    {
                        Temp += Body[i + j];
                    }
                    if (Temp.ToLower() == "\r" || Temp.ToLower() == "\n")
                    {
                        for (int k = 0; k < word.Length; k++)
                        {
                            Body[i + k] = '.';
                        }
                    }
                    else if (Temp.ToLower() == word)
                    {
                        for (int k = 0; k < word.Length; k++)
                        {
                            Body[i + k] = ' ';
                        }
                    }
                }
            }
            foreach (char character in Body)
            { text += character; }
            string[] words = text.Split(' ');
            string[] TempArray;
            foreach (string word in words)
            {
                if (word != "")
                {
                    if (word.Contains('.') && word[word.Length - 1] != '.')
                    {
                        TempArray = word.Split('.');
                        foreach (string temp in TempArray)
                        {
                            if (temp != "" && temp == TempArray[TempArray.Length-1])
                            {
                                FilteredWords.Add(temp);
                            }
                            else if (temp != "")
                            {
                                FilteredWords.Add(temp+".");
                            }


                        }
                    }
                    else
                    {
                        FilteredWords.Add(word);
                    }
                    
                }
            }
            foreach (string word in FilteredWords)
            {
                Console.Write(word+" ");
                if (word.Contains('.'))
                {
                    Console.Write("\n");
                }
            }
            List<Graph> nodes = new List<Graph>();

            // start on incorperating graph

            Console.ReadLine();

        }


    }

}

