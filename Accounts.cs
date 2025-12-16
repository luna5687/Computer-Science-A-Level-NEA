using System.Data.Common;

namespace Computer_Science_A_Level_NEA
{
    static class Accounts
    {
        static void AddAccounts()
        {
            Console.Clear();
            Console.Write("Enter Account Username: ");
            string accountname = Console.ReadLine();
            Console.Write("Enter Account Password (Please note it is not hidden): ");
            while (!IsVaildAccountName(accountname))
            {
                Console.Write("Account Username is not valid please ReEnter Usersame: ");
                accountname = Console.ReadLine();
            }
            string accountPassword = Console.ReadLine();
            while (accountPassword == "")
            {
                Console.WriteLine("invalid password");
                Console.Write("Enter Account Password (Please note it is not hidden): ");

                accountPassword = Console.ReadLine();
            }
            File.Create($"{accountname}Settings.txt").Close();
            SetDefulatAccountSettings(accountname);
            SQLDataBase.ExecuteNonQuery("INSERT INTO Accounts(AccountName,Password)" +
                              "VALUES " +
                              $"('{accountname}','{Encryption.Encrypt(accountPassword)}');");
        }
        public static void SetDefulatAccountSettings(string accountName)
        {
            StreamWriter SW = new StreamWriter($"{accountName}Settings.txt", false);
            SW.WriteLine("Algorithm");
            SW.Close();
        }
        static bool IsVaildAccountName(string name)
        {
            List<string[]> AllUsernames = SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts");
            if (AllUsernames == null) return true;
            foreach (string[] usernames in AllUsernames)
            {
                if (usernames[0] == name) return false;
            }
            if (name == "") return false;
            return true;
        }
        public static void AccountsMenu()
        {
            try
            {
                List<string[]> temp;


                List<string> Accounts = new List<string>();
                temp = SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts");
                if (temp == null)
                {
                    Console.WriteLine("No accounts found \nAccount creation requred press enter to continue");
                    Console.ReadLine();
                    AddAccounts();
                    temp = SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts");
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
                    string[] StringTempArray = new string[Accounts.Count+3];
                    StringTempArray[StringTempArray.Length - 1] = "Exit";
                    StringTempArray[StringTempArray.Length - 2] = "Manage Global settings";
                    StringTempArray[StringTempArray.Length - 3] = "Manage Accounts";
                    for (int i =0; i < Accounts.Count; i++)
                    {
                        StringTempArray[i] = Accounts[i];
                    }

                    menuOption = ConsoleInteraction.Menu("", StringTempArray, menuOption);
                    
                        if (menuOption == Accounts.Count + 2)
                        {
                            exit = true;
                        }
                        else if (menuOption == Accounts.Count + 1)
                        {
                            EditGlobalSettings();
                        }
                        else if (menuOption == Accounts.Count)
                        {
                            ManageAccounts();
                            temp = SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts");
                            Accounts = new List<string>();
                            foreach (string[] s in temp)
                            {
                                Accounts.Add(s[0].ToString());
                            }
                            temp = null;
                        }
                        else
                        {
                            Console.Clear();
                            Console.Write($"Please enter password for Account: {Accounts[menuOption]}.\nPress ENTER when input is empty to cancel\n" +
                                $"");
                            string EnteredPassword = Console.ReadLine();
                            bool passwordConfined = false;
                            temp = SQLDataBase.ExecuteQuery($"SELECT Password FROM Accounts WHERE AccountName = '{Accounts[menuOption]}'");
                            while (EnteredPassword != "" && !passwordConfined)
                            {
                                if (EnteredPassword == Encryption.Decrypt(temp[0][0]))
                                {

                                    passwordConfined = true;
                                    UserMenu.EmailAddressesMenu(Accounts[menuOption]);
                                }
                                else
                                {
                                    ConsoleInteraction.ResetCursor();
                                    Console.WriteLine("Password invalid                              ");

                                    Console.Write($"Please enter password for Account: {Accounts[menuOption]}.\nPress ENTER when input is empty to cancel                                                         \n                                                  ");
                                    Console.CursorLeft = 0;
                                    EnteredPassword = Console.ReadLine();
                                }
                            }
                            Console.Clear();
                        }

                    
                    ConsoleInteraction.ResetCursor();
                }
            }
            catch (MailKit.ServiceNotConnectedException ex)
            {
                Console.WriteLine("Could not connect to mail server");
                ConsoleInteraction.GetConsoleInput();
            }
            catch (System.Net.Sockets.SocketException)
            {
                Console.WriteLine("A mail server for an email was invalid");
                ConsoleInteraction.GetConsoleInput();   
            }
            catch (MailKit.Security.AuthenticationException)
            {
                Console.WriteLine("A email address or password for email address was invalid");
            }

        }
        static void ManageAccounts()
        {
            Console.Clear();
            string[] MenuOptions = { "Back", "Add account", "Delete account" };
            bool Exit = false;
            int menuOption = 0;
            
            while (!Exit)
            {
                
                menuOption = ConsoleInteraction.Menu("", MenuOptions, menuOption);
                
                    switch (MenuOptions[menuOption])
                    {
                        case "Back":
                            Exit = true;
                            break;
                        case "Add account":
                            AddAccounts();
                            break;
                        case "Delete account":
                            DeleteAccountsMenu();
                            break;
                    }

                
            }
            Console.Clear();
        }
        static void DeleteAccountsMenu()
        {
            Console.Clear();
            if (SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts").Count == 1)
            {
                Console.WriteLine("Deleting accounts is not possible");
            }
            else
            {
                bool exit = false;
                string input;
                int menuOption = 0;
                string[] menuOptions = GetAllAccountNames();
                menuOptions[0] = "Back";
                while (SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts").Count != 1 && !exit)
                {
                    menuOptions = GetAllAccountNames();
                    menuOptions[0] = "Back";
                    menuOption = ConsoleInteraction.Menu("", menuOptions, menuOption);
                    
                        if (menuOptions[menuOption] == "Back") exit = true;
                        else
                        {
                            DeleteAccount((SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts"))[menuOption-1][0]);
                        }
                    
                    menuOptions = GetAllAccountNames();
                }
            }
        }
        static void DeleteAccount(string accountName)
        {
            List<string[]> AllConnectedEmailAddresses = SQLDataBase.ExecuteQuery($"SELECT EmailAddress FROM Users WHERE Account == '{accountName}'");
            for (int i = 0; AllConnectedEmailAddresses!=null && i < AllConnectedEmailAddresses.Count; i++)
            {
                List<string[]> AllEmailsIDs = SQLDataBase.ExecuteQuery($"SELECT EmailID FROM Emails WHERE Recipient == '{AllConnectedEmailAddresses[i][0]}'");
                for (int j = 0; j < AllEmailsIDs.Count; j++)
                {
                    SQLDataBase.ExecuteNonQuery($"DELETE FROM AssignedTags WHERE EmailID == {AllEmailsIDs[i][0]}");
                    SQLDataBase.ExecuteNonQuery($"DELETE FROM Emails WHERE EmailID == {AllEmailsIDs[i][0]}");
                }
            }
            File.Delete($"{accountName}Settings.txt");
            SQLDataBase.ExecuteNonQuery($"DELETE FROM Users WHERE Account == '{accountName}'");
            SQLDataBase.ExecuteNonQuery($"DELETE FROM Accounts WHERE AccountName == '{accountName}'");
        }
        static string[] GetAllAccountNames()
        {
            List<string[]> AllAccounts = SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts");
            int maxAccounts = SQLDataBase.ExecuteQuery("SELECT AccountName FROM Accounts").Count;
            string[] output = new string[maxAccounts + 1];

            for (int i = 0; i < maxAccounts; i++)
            {
                output[i + 1] = AllAccounts[i][0];
            }
            return output;
        }
        static void EditGlobalSettings()
        {
            Console.Clear();
            string[] MenuOptions = { "Back", "Resest To Default", "Edit Max database size", "Edit Overflow handaling", "View current settings" };
            bool exit = false;
            string input;
            int menuOption = 0;
            while (!exit)
            {
                ConsoleInteraction.ResetCursor();
                
                menuOption = ConsoleInteraction.Menu("", MenuOptions, menuOption);
                
                    switch (MenuOptions[menuOption])
                    {
                        case "Back":
                            exit = true;
                            break;
                        case "Resest To Default":
                            SQLDataBase.SetMaxSize(-1);
                            SQLDataBase.SetOverFlowType("Error");
                            break;
                        case "Edit Max database size":
                            Console.WriteLine("Please enter the max database size in GB (Must be greater than or equal to 1 or -1 for unlimited)");
                            double Size = double.Parse(Console.ReadLine());
                            Console.Clear();
                            while (Size < 0 && Size != -1)
                            {
                                Console.WriteLine("Invalid Size");
                                Console.WriteLine("Please enter the max database size in GB (Must be greater than to 0)");
                                Size = double.Parse(Console.ReadLine());
                            }
                            StreamWriter SW = new StreamWriter("GlobalSettings.txt", false);
                            if (Size == -1)
                            {
                                SW.Write($"-1,{SQLDataBase.GetOverFlowType()}");
                            }
                            else
                            {
                                SW.Write($"{Size},{SQLDataBase.GetOverFlowType()}");

                            }
                            SW.Close();
                            SQLDataBase.SetMaxSize(Size);
                            break;
                        case "Edit Overflow handaling":
                            EditOverFlowSettings();
                            break;
                        case "View current settings":
                            OutPutSettings();
                            Console.CursorTop -= 2;
                            Console.Write("                                        \n");
                            Console.Write("                                        \n");
                            break;
                    }
                
            }
            Console.Clear();
        }
        static void OutPutSettings()
        {
            StreamReader SR = new StreamReader("GlobalSettings.txt");
            string body = SR.ReadToEnd();
            Console.Write("Max database Size: ");
            if (double.Parse(body.Split(',')[0]) == -1) Console.Write("Unlimited\n");
            else Console.Write($"{body.Split(',')[0]}GB\n");
            Console.WriteLine($"Overfolw handling: {body.Split(',')[1]}");
            Console.ReadKey();


            SR.Close();
        }
        static void EditOverFlowSettings()
        {
            Console.Clear();
            string[] MenuOptions = { "Back", "Error", "Delete Oldest" };
            bool exit = false;
            string input;
            int menuOption = 0;
            while (!exit)
            {
                ConsoleInteraction.ResetCursor();
                
                menuOption = ConsoleInteraction.Menu("", MenuOptions, menuOption);
                
                    switch (MenuOptions[menuOption])
                    {
                        case "Back":
                            exit = true;
                            break;
                        case "Error":
                            SQLDataBase.SetOverFlowType("Error");
                            break;
                        case "Delete Oldest":
                            SQLDataBase.SetOverFlowType("Delete Oldest");
                            break;
                    }
               
            }
        }
    }
}
