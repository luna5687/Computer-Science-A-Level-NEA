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
                while (accountPassword == "")
                {
                    Console.WriteLine("invalid password");
                    Console.Write("Enter Account Password (Please note it is not hidden): ");

                    accountPassword = Console.ReadLine();
                }
                new SQLiteCommand("INSERT INTO Accounts(AccountName,Password)" +
                                  "VALUES " +
                                  $"('{accountname}','{Encryption.Encrypt(accountPassword)}');",connection).ExecuteNonQuery();
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
                    for (int i = 0; i < Accounts.Count;i++)
                    {
                        Console.Write("   " + Accounts[i]+"\n");
                    }
                    Console.Write(" > Exit");
                }
                else
                {
                    for (int i = 0;i < Accounts.Count;i++)
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
                else if (input == "\r"||input=="")
                {
                     if (menuOption  == Accounts.Count)
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
        static void CreateNewEmail(string AccountName)
        {
            
        }
        static void EmailAddressesMenu(string accountName)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Email_Archive.db;Version=3;New=True;Compress=True;");
            connection.Open();
            SQLiteDataReader DR = new SQLiteCommand("SELECT EmailAddress FROM Users", connection).ExecuteReader();
            if (DR.StepCount == 0)
            {
                

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
                                  $"('{EmailAddress}','{Encryption.Encrypt(EmailPassword)}','{MailServer}','{accountName}');", connection).ExecuteNonQuery(); // has issue with database being 'locked'

                Console.Clear();

             
            }
            List<string> EmaliAddreses = new List<string>();
            while (DR.Read())
            {
                EmaliAddreses.Add(DR["EmailAddress"].ToString());
            }
            bool exit = false;
            int menuOption = 0;
            while (!exit)
            {
                if (menuOption == EmaliAddreses.Count)
                {
                    for (int i = 0; i < EmaliAddreses.Count; i++)
                    {
                        Console.Write("   " + EmaliAddreses[i] + "\n");
                    }
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
                    Console.Write("   Exit");
                }
                string input = ConsoleInteraction.GetConsoleInput(true).ToUpper();

                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = EmaliAddreses.Count;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > EmaliAddreses.Count)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r"||input=="")
                {
                    if (menuOption == EmaliAddreses.Count)
                    {
                        exit = true;
                    }
                    else
                    {
                        DR = new SQLiteCommand($"SELECT * FROM users WHERE EmailAddress = '{EmaliAddreses[menuOption]}'",connection).ExecuteReader();
                        DR.Read();
                        EmailMenu(DR["EmailAddress"].ToString(), Encryption.Decrypt(DR["Password"].ToString()), DR["Mailserver"].ToString());
                        Console.Clear();
                    }

                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }
            connection.Close();
            // display email
            // work on text rank on email and automate archive
        }
        static void EmailMenu(string EmailAddress,string EmailPassword,string Mailserver)
        {
            ImapServer imapServer = new ImapServer(EmailAddress,EmailPassword, Mailserver);
            LoadedEmails emalis = new LoadedEmails(imapServer);
            emalis.EmailMenu();
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
      
            






            Console.ReadLine();

        }


    }

}

