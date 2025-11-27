// Copyright 2025 Daniel Ian White
using System.Text.RegularExpressions;

namespace Computer_Science_A_Level_NEA
{
    public class UserMenu
    {

        static int EmailAddressesMennOptionSelect(List<string> EmaliAddreses)
        {
            bool exit = false;
            int menuOption = 0;
            while (!exit)
            {
                ConsoleInteraction.ResetCursor();
                if (menuOption == EmaliAddreses.Count)
                {
                    for (int i = 0; i < EmaliAddreses.Count; i++)
                    {
                        Console.Write("   " + EmaliAddreses[i] + "\n");
                    }
                    Console.WriteLine(" > Manage Tags");
                    Console.WriteLine("   Manage Emails");
                    Console.WriteLine("   Manage Account Settings");
                    Console.Write("   Exit");
                }
                else if (menuOption == EmaliAddreses.Count + 1)
                {
                    for (int i = 0; i < EmaliAddreses.Count; i++)
                    {
                        Console.Write("   " + EmaliAddreses[i] + "\n");
                    }
                    Console.WriteLine("   Manage Tags");
                    Console.WriteLine(" > Manage Emails");
                    Console.WriteLine("   Manage Account Settings");
                    Console.Write("   Exit");
                }
                else if (menuOption == EmaliAddreses.Count + 2)
                {
                    for (int i = 0; i < EmaliAddreses.Count; i++)
                    {
                        Console.Write("   " + EmaliAddreses[i] + "\n");
                    }
                    Console.WriteLine("   Manage Tags");
                    Console.WriteLine("   Manage Emails");
                    Console.WriteLine(" > Manage Account Settings");
                    Console.Write("   Exit");
                }
                else if (menuOption == EmaliAddreses.Count + 3)
                {
                    for (int i = 0; i < EmaliAddreses.Count; i++)
                    {
                        Console.Write("   " + EmaliAddreses[i] + "\n");
                    }
                    Console.WriteLine("   Manage Tags");
                    Console.WriteLine("   Manage Emails");
                    Console.WriteLine("   Manage Account Settings");
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
                    Console.WriteLine("   Manage Tags");
                    Console.WriteLine("   Manage Emails");
                    Console.WriteLine("   Manage Account Settings");
                    Console.Write("   Exit");
                }
                string input = ConsoleInteraction.GetConsoleInput(true).ToUpper();

                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = EmaliAddreses.Count + 3;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > EmaliAddreses.Count + 3)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    exit = true;
                }
            }
            return menuOption;
        }
        public static void EmailAddressesMenu(string accountName)
        {
            try
            {
                List<string> EmaliAddreses = GetEmailAddresses(accountName);
                if (EmaliAddreses == null)
                {
                    Console.WriteLine("No Email address found please add an email address:");
                    CreateEmail(accountName);
                }
                EmaliAddreses = GetEmailAddresses(accountName);
                bool exit = false;
                List<string[]> temp;
                int menuOption = 0;
                Console.Clear();
                while (!exit)
                {
                    menuOption = EmailAddressesMennOptionSelect(EmaliAddreses);
                    if (menuOption == EmaliAddreses.Count + 3)
                    {
                        exit = true;
                    }
                    else if (menuOption == EmaliAddreses.Count + 2)
                    {
                        ManageAccountSettings(accountName);
                        Console.Clear();
                    }
                    else if (menuOption == EmaliAddreses.Count + 1)
                    {
                        EmailManagement(EmaliAddreses, accountName);
                        Console.Clear();
                    }
                    else if (menuOption == EmaliAddreses.Count)
                    {
                        Tags.TagManagement();
                        Console.Clear();
                    }
                    else
                    {

                        temp = SQLDataBase.ExecuteQuery($"SELECT * FROM users WHERE EmailAddress = '{EmaliAddreses[menuOption]}'");
                        EmailMenu(temp[0][0], Encryption.Decrypt(temp[0][1]), temp[0][2], accountName);
                        temp = null;
                        Console.Clear();
                    }


                    ConsoleInteraction.ResetCursor(); ;
                }


            }
            catch (DataBaseFullExeption ex)
            {

                string[] MenuOptions = { "Set all accounts automatic archive settings to off", "Manualy Delete emails form archive", "Automaticaly Delete oldest" };
                int menuOption = 0;
                bool exit = false;

                while (!exit)
                {

                    menuOption = ConsoleInteraction.Menu("The database is full Please select an option: ", MenuOptions);
                    switch (MenuOptions[menuOption])
                    {
                        case "Set automatic archive settings to off":
                            AccountSettings accountSettings = new AccountSettings(accountName);
                            accountSettings.AutomaticArcive = "Off";
                            accountSettings.UpdateSettingsFile();
                            break;
                        case "Manualy Delete emails form archive":
                            DeleteEmailMenu(accountName);
                            break;
                        case "Automaticaly Delete oldest":
                            SQLDataBase.ResolveOverFlow();
                            break;
                    }
                    if (SQLDataBase.IsFull()) exit = false;
                    else exit = true;
                }
            }
        }
        static void DeleteEmailMenu(string accountName)
        {
            bool exit = false;
            int MenuOption = 0;
            List<string[]> AllEmails = SQLDataBase.ExecuteQuery("SELECT * FROM Emails");
            string[] MenuOptions = new string[AllEmails.Count + 1];

            MenuOptions[MenuOptions.Length - 1] = "Back";
            while (!exit)
            {
                AllEmails = SQLDataBase.ExecuteQuery("SELECT * FROM Emails");
                for (int i = 0; i < AllEmails.Count; i++)
                {
                    string temp = AllEmails[i][0] + "|" + AllEmails[i][1] + "|" + AllEmails[i][2] + "|" + AllEmails[i][3] + "|" + AllEmails[i][5] + "|" + AllEmails[i][7];
                    MenuOptions[i] = temp;
                }
                MenuOption = ConsoleInteraction.Menu($"ID{ConsoleInteraction.GetBuffer(GetLongestPart(AllEmails, 0) - 2)}|Sender{ConsoleInteraction.GetBuffer(GetLongestPart(AllEmails, 1) - 6)}|Recipient{ConsoleInteraction.GetBuffer(GetLongestPart(AllEmails, 2) - 9)}|Subject{ConsoleInteraction.GetBuffer(GetLongestPart(AllEmails, 3) - 7)}|Keywords{ConsoleInteraction.GetBuffer(GetLongestPart(AllEmails, 5) - 8)}|Date Recived",
                                                       MenuOptions);
                if (MenuOption == MenuOptions.Length - 1)
                {
                    exit = true;
                }
                else
                {
                    SQLDataBase.ExecuteNonQuery($"DELETE FROM Emails WHERE EmailID == {AllEmails[MenuOption][0]}");
                }
            }
        }
        static int GetLongestPart(List<string[]> Emails, int indexOfPart)
        {
            int Longest = 0;
            foreach (string[] Email in Emails)
            {
                if (Email[indexOfPart].Length > indexOfPart) Longest = Email[indexOfPart].Length;
            }
            return Longest;
        }
        static void ManageAccountSettings(string accountName)
        {
            bool exit = false;
            int MenuOption = 0;
            string[] MenOptions = { "View settings", "Update Automatic archive settings", "Reset To defualt", "Back" };
            while (!exit)
            {
                MenuOption = ConsoleInteraction.Menu("", MenOptions);
                switch (MenOptions[MenuOption])
                {
                    case "Back":
                        exit = true;
                        break;
                    case "View settings":
                        DisplayAccoutSettings(accountName);
                        break;
                    case "Update Automatic archive settings":
                        UpdateAccoutArciveSettings(accountName);
                        break;
                    case "Reset To defualt":
                        Accounts.SetDefulatAccountSettings(accountName);
                        break;
                }
                Console.Clear();

            }

        }
        static void UpdateAccoutArciveSettings(string input)
        {

            AccountSettings accountSettings = new AccountSettings(input);


            string[] MenuOptions = { "Set to 'Algorithm'", "Set to 'All Emails'", "Set to 'Off'", "Back" };
            int MenuOption = 0;
            bool exit = false;
            while (!exit)
            {
                MenuOption = ConsoleInteraction.Menu("", MenuOptions);
                switch (MenuOptions[MenuOption])
                {
                    case "Back":
                        exit = true;
                        break;
                    case "Set to 'Algorithm'":
                        accountSettings.AutomaticArcive = "Algorithm";
                        break;
                    case "Set to 'All Emails'":
                        accountSettings.AutomaticArcive = "All Emails";
                        break;
                    case "Set to 'Off'":
                        accountSettings.AutomaticArcive = "Off";
                        break;
                }
                Console.Clear();

                accountSettings.UpdateSettingsFile();
            }
        }


        static void DisplayAccoutSettings(string accountName)
        {
            StreamReader SR = new StreamReader($"{accountName}Settings.txt");
            string settings = SR.ReadToEnd();
            SR.Close();
            Console.WriteLine(settings);
            ConsoleInteraction.GetConsoleInput();
        }
        static bool CheckEmailAddressIsValid(string input)
        {
            if (!Regex.IsMatch(input, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$")) return false; // the regex epression was taken from https://emailregex.com/ on 27/11/2025
            List<string[]> AllEmailAddresses = SQLDataBase.ExecuteQuery("SELECT EmailAddress FROM Users");
            if (AllEmailAddresses == null) return true;
            foreach (string[] EmailAddress in AllEmailAddresses) if (EmailAddress[0] == input) return false;
            return true;
        }
        static List<string> GetEmailAddresses(string accountName)
        {
            List<string[]> temp;
            temp = SQLDataBase.ExecuteQuery($"SELECT EmailAddress FROM Users WHERE Account == '{accountName}'");
            if (temp == null)
            {
                return null;
            }

            List<string> EmaliAddreses = new List<string>();
            foreach (string[] s in temp)
            {
                EmaliAddreses.Add(s[0].ToString());
            }
            return EmaliAddreses;
        }
        static bool CheckMailSever(string input)
        {
            return Regex.IsMatch(input, "^(\\w+\\.)?\\w+\\.\\w+\\.\\w+$");
        }
        static void CreateEmail(string accountName)
        {
            Console.Write("Enter Emailaddress: ");
            string EmailAddress = Console.ReadLine();
            List<string> EmaliAddreses = GetEmailAddresses(accountName);
            while (!CheckEmailAddressIsValid(EmailAddress) && EmaliAddreses != null && EmaliAddreses.Contains(EmailAddress))
            {
                while (!CheckEmailAddressIsValid(EmailAddress))
                {
                    Console.Write("Email Address is not valid please ReEnter Usersame: ");
                    EmailAddress = Console.ReadLine();
                }

                while (EmaliAddreses != null && EmaliAddreses.Contains(EmailAddress))
                {
                    Console.Write("Email entered already exists\nPlease enter a diffrent email: ");
                    EmailAddress = Console.ReadLine();
                }
            }
            Console.Write("Enter the email's mail server: ");
            string MailServer = Console.ReadLine();
            while (!CheckMailSever(MailServer))
            {
                Console.Write("Mail Sever is invalid\nPlease ReEnter Mail sever: ");
                MailServer = Console.ReadLine();
            }


            Console.Write("Enter EmailAddress Password (Please note it is not hidden): ");
            // TODO - validate password 
            string EmailPassword = Console.ReadLine();

            SQLDataBase.ExecuteNonQuery("INSERT INTO Users(EmailAddress,Password,Mailserver,Account)" +
                              "VALUES " +
                              $"('{EmailAddress}','{Encryption.Encrypt(EmailPassword)}','{MailServer}','{accountName}');");
            Console.Clear();
        }
        static void EmailMenu(string EmailAddress, string EmailPassword, string Mailserver, string accountName)
        {
            ImapServer imapServer = new ImapServer(EmailAddress, EmailPassword, Mailserver);
            LoadedEmails emalis = new LoadedEmails(imapServer, accountName);
            emalis.EmailMenu();
        }
        static void EmailManagement(List<string> EmaliAddreses, string accountName)
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
                        DeleteEmails(EmaliAddreses, accountName);

                    }
                    else if (menuOption == 1)
                    {
                        AddEmail(EmaliAddreses, accountName);
                    }
                    else
                    {
                        exit = true;
                    }
                }
                ConsoleInteraction.ResetCursor();
            }
        }
        static void DeleteEmails(List<string> EmailAddresses, string accountName)
        {
            bool exit = false;
            int menuOption = 0;
            Console.Clear();
            while (!exit && EmailAddresses != null)
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

                        SQLDataBase.ExecuteNonQuery($"DELETE FROM Users WHERE EmailAddress == '{EmailAddresses[menuOption]}'");
                        Console.Clear();
                    }

                }
                ConsoleInteraction.ResetCursor();
                EmailAddresses = GetEmailAddresses(accountName);
            }
        }
        static void AddEmail(List<string> EmailAddresses, string accountName)
        {
            Console.Clear();
            Console.Clear();

            CreateEmail(accountName);


            Console.Clear();
        }
    }
    public class AccountSettings
    {
        public string AutomaticArcive;
        public string AccountName;
        public AccountSettings(string accountName)
        {
            StreamReader sr = new StreamReader($"{accountName}Settings.txt");
            string[] AllSettings = sr.ReadToEnd().Split(',');
            AccountName = accountName;
            sr.Close();
            AutomaticArcive = AllSettings[0];
        }
        public void UpdateSettingsFile()
        {
            string output = "";
            output += AutomaticArcive;
            StreamWriter SW = new StreamWriter($"{AccountName}Settings.txt", false);
            SW.Write(output);
            SW.Close();
        }

    }

}
