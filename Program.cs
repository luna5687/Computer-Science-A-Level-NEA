using Computer_Science_A_Level_NEA;
// Copyright 2025 Daniel Ian White


// Bobs email: bob@ampretia.co.uk password: passw0rdWibble Mailserver: mail.ampretia.co.uk

namespace NEA_protoype
{

    internal class Program
    {
       
        
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                try
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
                                     "EmailAddress varchar," +
                                     "DateRecived varchar)",

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
                                     "(0,'Meeting')," +
                                     "(1,'Accounting')," +
                                     "(2,'Important')",

                                     "CREATE TABLE Collisions " +
                                     "(CollisionAt int)",

                                     "INSERT INTO Collisions (CollisionAt)" + 
                                     " VALUES (-1)" // it is -1 as an email can never have this tag
                                     };

                    SQLDataBase.CreateDataBase("Email_Archive", InitalTable);
                    Tags.LoadTags();
                    CheckForGlobalSettings();
                    ConsoleInteraction.CheckConsoleExistance();

                    Accounts.AccountsMenu();
                    exit = true;
                    SQLDataBase.CloseConnection();
                }
                
                catch (MailKit.Security.SslHandshakeException) { Console.WriteLine("Cannot cannot connect to mail server due to security issues "); Console.ReadKey(); }
                catch (Exception e)
                {
                    Console.Write("An error occured with exeption ");
                    Console.WriteLine(e.Message);
                    exit = ChrashOptions();
                    SQLDataBase.CloseConnection();
                }
                
            }

        }
        static bool ChrashOptions()
        {
            string[] MenuOptions = { "Restart Program","Exit Program" };
            string input;
            int menuOption = 0;
            while (true)
            {
                ConsoleInteraction.ResetCursor();
                Console.CursorTop += 5;
                for (int i = 0; i < MenuOptions.Length; i++)
                {
                    if (i == menuOption) Console.Write(" > ");
                    else Console.Write("   ");
                    Console.WriteLine(MenuOptions[i]);
                }
                input = ConsoleInteraction.GetConsoleInput(true);
                if (input.ToLower() == "w")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = MenuOptions.Length - 1;
                    }
                }
                else if (input.ToLower() == "s")
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
                        case "Restart Program":
                            return false;
                        case "Exit Program":
                            return true;
                    }
                }
            }
            return true;
        }
        static void CheckForGlobalSettings()
        {
            if (!File.Exists("GlobalSettings.txt"))
            {
                File.Create("GlobalSettings.txt").Close();
                StreamWriter SW = new StreamWriter("GlobalSettings.txt");
                SW.Write("-1,Error");
                SW.Close();
            }
            else
            {
                StreamReader SR = new StreamReader("GlobalSettings.txt");
                string body = SR.ReadToEnd();
                SQLDataBase.SetMaxSize(int.Parse(body.Split(',')[0]));
                SQLDataBase.SetOverFlowType(body.Split(',')[1]);
                SR.Close();
            }
        }
    }

}

