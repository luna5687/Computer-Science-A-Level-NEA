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
                                UserMenu.EmailAddressesMenu(Accounts[menuOption], ref DataBase);
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
                                     "(2,'Important')",

                                     "CREATE TABLE Collisions " +
                                     "(CollisionAt int)"};
            SQLDataBase DataBase = new SQLDataBase("Email_Archive", InitalTable);
            
            ConsoleInteraction.CheckConsoleExistance();
            AccountsMenu(ref DataBase);
        }

    }

}

