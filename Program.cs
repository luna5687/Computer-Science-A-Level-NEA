using Computer_Science_A_Level_NEA;
// Copyright 2025 Daniel Ian White


// Bobs email: bob@ampretia.co.uk password: passw0rdWibble Mailserver: mail.ampretia.co.uk

namespace NEA_protoype
{

    internal class Program
    {
        static void AccountsMenu(ref SQLDataBase DataBase)
        {
            List<string[]> temp;


            List<string> Accounts = new List<string>();
            temp = DataBase.ExecuteQuery("SELECT AccountName FROM Accounts");
            if (temp == null)
            {
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
                DataBase.ExecuteNonQuery("INSERT INTO Accounts(AccountName,Password)" +
                                  "VALUES " +
                                  $"('{accountname}','{Encryption.Encrypt(accountPassword)}');");
                temp = DataBase.ExecuteQuery("SELECT AccountName FROM Accounts");
            }

            Accounts = new List<string>();
            foreach (string[] s in temp)
            {
                Accounts.Add(s[0].ToString());
            }
            temp = null;











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
                        temp = DataBase.ExecuteQuery($"SELECT Password FROM Accounts WHERE AccountName = '{Accounts[menuOption]}'");
                        while (EnteredPassword != "" && !passwordConfined)
                        {
                            if (EnteredPassword == Encryption.Decrypt(temp[0][0]))
                            {

                                passwordConfined = true;
                                EmailAddressesMenu(Accounts[menuOption], ref DataBase);
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

        }

        static void EmailAddressesMenu(string accountName, ref SQLDataBase DataBase)
        {
            List<string[]> temp;
            temp = DataBase.ExecuteQuery("SELECT EmailAddress FROM Users");
            if (temp == null)
            {
                Console.WriteLine("No EmailAddress found \nEmailAddress input requred press enter to continue");
                Console.ReadLine();
                Console.Clear();
                CreateEmail(ref DataBase, accountName);
                temp = DataBase.ExecuteQuery("SELECT EmailAddress FROM Users");
            }

            List<string> EmaliAddreses = new List<string>();
            foreach (string[] s in temp)
            {
                EmaliAddreses.Add(s[0].ToString());
            }
            temp = null;
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
                        EmailManagement(EmaliAddreses, ref DataBase, accountName);
                    }
                    else
                    {

                        temp = DataBase.ExecuteQuery($"SELECT * FROM users WHERE EmailAddress = '{EmaliAddreses[menuOption]}'");
                        EmailMenu(temp[0][0], Encryption.Decrypt(temp[0][1]), temp[0][2]);
                        temp = null;
                        Console.Clear();
                    }

                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }


            // work on text rank on email and automate archive
        }
        static void CreateEmail(ref SQLDataBase DataBase, string accountName)
        {
            Console.Write("Enter Emailaddress: ");
            string EmailAddress = Console.ReadLine();
            Console.Write("Enter the email's mail server: ");
            string MailServer = Console.ReadLine();
            Console.Write("Enter EmailAddress Password (Please note it is not hidden): ");
            // TODO - validate password 
            string EmailPassword = Console.ReadLine();

            DataBase.ExecuteNonQuery("INSERT INTO Users(EmailAddress,Password,Mailserver,Account)" +
                              "VALUES " +
                              $"('{EmailAddress}','{Encryption.Encrypt(EmailPassword)}','{MailServer}','{accountName}');");
            Console.Clear();
        }
        static void EmailMenu(string EmailAddress, string EmailPassword, string Mailserver)
        {
            ImapServer imapServer = new ImapServer(EmailAddress, EmailPassword, Mailserver);
            LoadedEmails emalis = new LoadedEmails(imapServer);
            emalis.EmailMenu();
        }
        static void EmailManagement(List<string> EmaliAddreses, ref SQLDataBase DataBase, string accountName)
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
                        DeleteEmails(EmaliAddreses, ref DataBase);
                        exit = true;
                    }
                    else if (menuOption == 1)
                    {
                        AddEmail(EmaliAddreses, ref DataBase, accountName);
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
        static void DeleteEmails(List<string> EmailAddresses, ref SQLDataBase DataBase)
        {
            bool exit = false;
            int menuOption = 0;
            Console.Clear();
            while (!exit)
            {
                if (menuOption == EmailAddresses.Count)
                {
                    for (int i = 0; i < EmailAddresses.Count; i++)
                    {
                        Console.Write("   " + EmailAddresses[i] + "\n");
                    }

                    Console.Write(" > Exit");
                }

                else
                {
                    for (int i = 0; i < EmailAddresses.Count; i++)
                    {
                        if (i == menuOption)
                        {
                            Console.Write(" > " + EmailAddresses[i] + "\n");
                        }
                        else
                        {
                            Console.Write("   " + EmailAddresses[i] + "\n");
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
                        menuOption = EmailAddresses.Count;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > EmailAddresses.Count)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {

                    if (menuOption == EmailAddresses.Count)
                    {
                        exit = true;
                    }
                    else
                    {

                        DataBase.ExecuteNonQuery($"DELETE FROM Users WHERE EmailAddress == '{EmailAddresses[menuOption]}'");

                        Console.Clear();
                    }

                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }
        }
        static void AddEmail(List<string> EmailAddresses, ref SQLDataBase DataBase, string accountName)
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
            DataBase.ExecuteNonQuery("INSERT INTO Users(EmailAddress,Password,Mailserver,Account)" +
                              "VALUES " +
                              $"('{EmailAddress}','{Encryption.Encrypt(EmailPassword)}','{MailServer}','{accountName}');");


            Console.Clear();
        }
        static void Main(string[] args)
        {
            string[] InitalTable = {"CREATE TABLE Users (" +
                                    "EmailAddress varchar PRIMARY KEY," +
                                     "Password varchar," +
                                     "Mailserver varchar," +
                                     "Account varchar)",

                                     "CREATE TABLE Emails(" +
                                     "EmailID int PRIMARY KEY," +
                                     "Sender varchar," +
                                     "Recipient varchar," +
                                      "Subject varchar," +
                                     "TextBody varchar," +
                                      "Keywords varchar," +
                                     "EmailAddress varchar)",

                                     "CREATE TABLE Tags(" +
                                     "TagID int PRIMARY KEY," +
                                     "TagName varchar)",

                                     "CREATE TABLE AssignedTags(" +
                                     "TagID int," +
                                     "EmailID int)",

                                     "CREATE TABLE Accounts(" +
                                     "AccountName varchar PRIMARY KEY," +
                                     "Password varchar)",

                                     "INSERT INTO Tags(tagID,TagName)" +
                                     "VALUES " +
                                     "(0,'Metting')," +
                                     "(1,'Accounting')," +
                                     "(2,'Important')"};
            SQLDataBase DataBase = new SQLDataBase("Email_Archive", InitalTable);
            ConsoleInteraction.CheckConsoleExistance();
            AccountsMenu(ref DataBase);







        }

    }

}

