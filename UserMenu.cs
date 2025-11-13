// Copyright 2025 Daniel Ian White
using System.Collections.Generic;

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
                    EmailAddressesMennOptionSelect(EmaliAddreses);
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
                Console.WriteLine("The database is full Please select an option: ");
                string[] MenuOptions = { "Set all accounts automatic archive settings to off", "Manualy Delete emails form archive", "Automaticaly Delete oldest" };
                int menuOption = 0;
                bool exit = false;
                string input;
                while (!exit)
                {

                    for (int i = 0; i < MenuOptions.Length; i++)
                    {
                        if (menuOption == i) Console.Write(" > ");
                        else Console.Write("   ");
                        Console.WriteLine(MenuOptions[i]);
                    }
                    input = ConsoleInteraction.GetConsoleInput();
                    ConsoleInteraction.ResetCursor();
                    if (input.ToLower() == "w")
                    {
                        menuOption--;
                        if (menuOption < 0)
                        {
                            menuOption = MenuOptions.Length - 1;
                        }
                    }
                    else if (input == "s")
                    {
                        menuOption++;
                        if (menuOption == MenuOptions.Length)
                        {
                            menuOption = 0;
                        }
                    }
                    else if (input == "\r" || input == "")
                    {
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

                    }
                }
            }
        }
        static void DeleteEmailMenu(string accountName)
        {
            bool exit = false;
            int MenuOption = 0;
            List<string[]> AllEmailsAddresses = SQLDataBase.ExecuteQuery("SELECT EmailAddress FROM Users WHERE ");
        }
        static void ManageAccountSettings(string accountName)
        {
            bool exit = false;
            int MenuOption = 0;
            string[] MenOptions = { "View settings", "Update Automatic archive settings", "Reset To defualt", "Back" };
            while (!exit)
            {
                for (int i = 0; i < MenOptions.Length; i++)
                {
                    if (MenuOption == i) Console.Write(" > ");
                    else Console.Write("   ");
                    Console.WriteLine(MenOptions[i]);
                }
                string input = ConsoleInteraction.GetConsoleInput(true);

                if (input == "W")
                {
                    MenuOption--;
                    if (MenuOption < 0)
                    {
                        MenuOption = MenOptions.Length - 1;
                    }
                }
                else if (input == "S")
                {
                    MenuOption++;
                    if (MenuOption > MenOptions.Length - 1)
                    {
                        MenuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    switch (MenOptions[MenuOption])
                    {
                        case "Back":
                            exit = true;
                            break;
                        case "View settings":
                            DisplayAccoutSettings(input);
                            break;
                        case "Update Automatic archive settings":
                            UpdateAccoutArciveSettings(input);
                            break;
                        case "Reset To defualt":
                            Accounts.SetDefulatAccountSettings(input);
                            break;
                    }

                }
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
                for (int i = 0; i < MenuOptions.Length; i++)
                {
                    if (i == MenuOption) Console.Write(" > ");
                    else Console.Write("   ");
                    Console.WriteLine(MenuOptions[i]);
                }

                if (input == "W")
                {
                    MenuOption--;
                    if (MenuOption < 0)
                    {
                        MenuOption = MenuOptions.Length - 1;
                    }
                }
                else if (input == "S")
                {
                    MenuOption++;
                    if (MenuOption > MenuOptions.Length - 1)
                    {
                        MenuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
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

                }
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
            List<string[]> AllEmailAddresses = SQLDataBase.ExecuteQuery("SELECT EmailAddresses FROM Users");
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
        static void CreateEmail(string accountName)
        {
            Console.Write("Enter Emailaddress: ");
            string EmailAddress = Console.ReadLine();
            while (!CheckEmailAddressIsValid(EmailAddress))
            {
                Console.Write("Email Address is not valid please ReEnter Usersame: ");
                EmailAddress = Console.ReadLine();
            }
            List<string> EmaliAddreses = GetEmailAddresses(accountName);
            while (EmaliAddreses != null && EmaliAddreses.Contains(EmailAddress))
            {
                Console.Write("Email entered already exists\nPlease enter a diffrent email: ");
                EmailAddress = Console.ReadLine();
            }
            Console.Write("Enter the email's mail server: ");
            string MailServer = Console.ReadLine();
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
