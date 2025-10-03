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
                        temp = DataBase.ExecuteQuery("SELECT EmailAddress FROM Users");
                        if (temp == null)
                        {
                            Console.WriteLine("No EmailAddress found \nEmailAddress input requred press enter to continue");
                            Console.ReadLine();
                            Console.Clear();
                            CreateEmail(ref DataBase, accountName);
                            temp = DataBase.ExecuteQuery("SELECT EmailAddress FROM Users");
                        }

                        EmaliAddreses = new List<string>();
                        foreach (string[] s in temp)
                        {
                            EmaliAddreses.Add(s[0].ToString());
                        }
                        temp = null;
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
            foreach (string EmailAddress in EmailAddresses)
            {
                DataBase.ExecuteNonQuery($"DELETE FROM Users WHERE EmailAddress == '{EmailAddress}'");
                DataBase.ExecuteNonQuery($"DELETE FROM Emails WHERE EmailAddress == '{EmailAddress}'");
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
            //SQLDataBase DataBase = new SQLDataBase("Email_Archive", InitalTable);
            ConsoleInteraction.CheckConsoleExistance();
            // AccountsMenu(ref DataBase);

            POSTagging.POStagging("Dear All\r\n\r\nTrip to Leonardo event\r\nTuesday 16th September\r\n\r\nWe (CANSAT teams) have been invited to attend a special event at Leonard Southampton to celebrate their new space technology. \r\n\r\nMore details below on the technology.\r\nDate: 16th September \r\nI will take you there and back in minibus.\r\n\r\nWill involve some talks and a tour and lunch\r\n\r\nWe have been given 12 tickets. So it is first come first serves.\r\n\r\nIf you would like to come on this trip then please email ME (not Leonardo!) letting me know that you want to come and any dietary requirements AS SOON AS POSSIBLE. \r\nThey want to know by 1st August ideally, but just let me know as soon as you can.\r\n\r\nALSO Leonardo have asked us to not tell anyone about this event till afterwards!!\r\nAny questions let me know.");
            ConsoleInteraction.GetConsoleInput();
            Console.Clear();
            POSTagging.POStagging("\r\nDear student,\r\n \r\nWelcome to the first edition of our College newsletter for the new academic year. This newsletter serves as a vital communication tool to keep you informed about important developments, events, and achievements throughout the year.\r\n\r\nAs you navigate the term, please remember that a wide range of resources is available to support your academic and personal well-being. Our dedicated Student Progress Advisers, Student Services, Careers Team, Learning Support, and Health and Wellbeing teams are always ready to assist you.\r\n\r\nPlease click the button below to read the newsletter. ");
            ConsoleInteraction.GetConsoleInput();
            // testing text rank delete after finishing 

            char[] Body = "Dear All\r\n\r\nTrip to Leonardo event\r\nTuesday 16th September\r\n\r\nWe (CANSAT teams) have been invited to attend a special event at Leonard Southampton to celebrate their new space technology. \r\n\r\nMore details below on the technology.\r\nDate: 16th September \r\nI will take you there and back in minibus.\r\n\r\nWill involve some talks and a tour and lunch\r\n\r\nWe have been given 12 tickets. So it is first come first serves.\r\n\r\nIf you would like to come on this trip then please email ME (not Leonardo!) letting me know that you want to come and any dietary requirements AS SOON AS POSSIBLE. \r\nThey want to know by 1st August ideally, but just let me know as soon as you can.\r\n\r\nALSO Leonardo have asked us to not tell anyone about this event till afterwards!!\r\nAny questions let me know.".ToCharArray();
            List<string> FilteredWords = new List<string>();
            string[] WordsToFilter = { " the ", "\n", "\r", " is ", " and ", " a ", " to " };
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
                            if (temp != "" && temp == TempArray[TempArray.Length - 1])
                            {
                                FilteredWords.Add(temp);
                            }
                            else if (temp != "")
                            {
                                FilteredWords.Add(temp + ".");
                            }


                        }
                    }
                    else
                    {
                        FilteredWords.Add(word);
                    }

                }
            }

            Graph graph = CreateGraph(FilteredWords);

            foreach (Node node in graph.nodes)
            {
                node.CaculateScore();
            }

            Node HighestScore1 = graph.nodes[0];
            Node HighestScore2 = graph.nodes[0];
            Node HighestScore3 = graph.nodes[0];

            for (int i = 0; i < graph.nodes.Count; i++)
            {
                if (HighestScore1.GetScore() < graph.nodes[i].GetScore())
                {
                    HighestScore3 = HighestScore2;
                    HighestScore2 = HighestScore1;
                    HighestScore1 = graph.nodes[i];
                }
                else if (HighestScore2.GetScore() < graph.nodes[i].GetScore())
                {
                    HighestScore3 = HighestScore2;
                    HighestScore2 = graph.nodes[i];
                }
                else if (HighestScore3.GetScore() < graph.nodes[i].GetScore())
                {

                    HighestScore3 = graph.nodes[i];
                }
            }
            Console.WriteLine($"1st: {HighestScore1.GetWord()} , {HighestScore1.GetScore()}");
            Console.WriteLine($"2nd: {HighestScore2.GetWord()} , {HighestScore2.GetScore()}");
            Console.WriteLine($"3rd: {HighestScore3.GetWord()} , {HighestScore3.GetScore()}");


            foreach (Node node in graph.nodes)
            {
                Console.Write($"{node.GetWord()}: ");
                for (int i = 0; i < node.GetEdgeAmount(); i++)
                {
                    Console.Write(node.GetEdge(i) + ",");
                 
                }
                
                Console.WriteLine("Score: " + node.GetScore());
            }
            Console.ReadLine();

        }

        
        static Graph CreateGraph(List<string> input)
        {
            Graph graph = new Graph();
            bool InGraph = false;
            for (int i = 0; i < input.Count; i++)
            {
                InGraph = false;
                foreach (Node n in graph.nodes)
                {
                    if (input[i].ToLower() == n.GetWord().ToLower())
                    {
                        InGraph = true;
                    }
                }
                if (!InGraph)
                {
                    graph.AddNode(input[i].ToLower());
                }
                if (i != 0)
                {
                    if (!input[i - 1].ToLower().Contains('.'))
                    {
                        if (graph.GetNodeIndex(input[i - 1].ToLower()) == -1)
                        {
                            graph.nodes[graph.GetNodeIndex(input[i].ToLower())].AddEdge(new Node(input[i - 1].ToLower()), 1);
                        }
                        else
                        {
                            if (graph.nodes[graph.GetNodeIndex(input[i].ToLower())].GetIndexOfEdge(input[i - 1].ToLower()) == -1)
                            {
                                graph.nodes[graph.GetNodeIndex(input[i].ToLower())].AddEdge(graph.nodes[graph.GetNodeIndex(input[i - 1].ToLower())], 1);
                            }
                            else
                            {
                                graph.nodes[graph.GetNodeIndex(input[i].ToLower())].IncreaseEdgeWeight(input[i - 1].ToLower(), 1);
                            }
                        }
                    }
                }
                if (i != input.Count - 1)
                {
                    
                        if (graph.GetNodeIndex(input[i + 1].ToLower()) == -1)
                        {
                            graph.nodes[graph.GetNodeIndex(input[i].ToLower())].AddEdge(new Node(input[i + 1].ToLower()), 1);
                        }
                        else
                        {
                            if (graph.nodes[graph.GetNodeIndex(input[i].ToLower())].GetIndexOfEdge(input[i + 1].ToLower()) == -1)
                            {
                                graph.nodes[graph.GetNodeIndex(input[i].ToLower())].AddEdge(graph.nodes[graph.GetNodeIndex(input[i + 1].ToLower())], 1);
                            }
                            else
                            {
                                graph.nodes[graph.GetNodeIndex(input[i].ToLower())].IncreaseEdgeWeight(input[i + 1].ToLower(), 1);
                            }
                        }
                    
                }
            }
            return graph;
        }

    }

}

