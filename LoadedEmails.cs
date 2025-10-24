// Copyright 2025 Daniel Ian White
using Org.BouncyCastle.Cms;

namespace Computer_Science_A_Level_NEA
{

    public class LoadedEmails
    {
        private List<Email> emails;
        private List<Email> CurrentDispalyEmails = new List<Email>();
        
        public LoadedEmails(ImapServer server)
        {
            emails = server.GetAllEmails();
            foreach (Email e in emails) CurrentDispalyEmails.Add(e);
        }
        public Email GetEmail(int index)
        {
            return emails[index];
        }
        public void EmailMenu()
        {

            Console.Clear();
            bool exit = false;
            int menuOption = 0;
            while (!exit)
            {
                DisplayEmails(menuOption);
                string input = ConsoleInteraction.GetConsoleInput(true).ToUpper();

                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = CurrentDispalyEmails.Count;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > CurrentDispalyEmails.Count + 1)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == CurrentDispalyEmails.Count + 1)
                    {
                        exit = true;
                    }
                    else if (menuOption == 0)
                    {
                        SearchForEamils();
                    }
                    else
                    {
                        // display emails
                        CurrentDispalyEmails[menuOption - 1].DisplayEmail();
                        Console.Clear();
                    }

                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }
        }
        private void SearchForEamils()
        {
            bool Exit = false;
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.Write("Please Select Field to filter");
            string SenderFilter, SubjectFilter;
            string input;
            int menuOption = 1;
            while (!Exit)
            {
                if (menuOption == 0)
                {
                    Console.CursorLeft = 0;
                    Console.Write("   >");
                }
                if (menuOption == 1)
                {
                    Console.CursorLeft = 3 + FindLongestSender() + 1 + FindLongestRecipient();
                    Console.Write("   >");
                }
                input = ConsoleInteraction.GetConsoleInput();
                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = 1;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > 1)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    
                }
            }
        }
        private void DisplayEmails(int menuOption)
        {
            // when displaying emails add headings 
            int[] Buffers = { FindLongestSender(), FindLongestRecipient(),FindLongestSubject()};
            if (menuOption == 0)
            {
                Console.WriteLine(" > Search Emails\n");
                  Console.WriteLine($"   Sender{ConsoleInteraction.GetBuffer(Buffers[0] - 6)}|Recipient{ConsoleInteraction.GetBuffer(Buffers[1] - 9)}|Subject{ConsoleInteraction.GetBuffer(Buffers[2] - 7)}");
                    for (int i = 0; i < CurrentDispalyEmails.Count; i++)
                    {
                        Console.Write("   " + CurrentDispalyEmails[i].GetEmailShort(Buffers) + "\n");
                    }
                    Console.Write(" > Exit");
            }
            if (menuOption-1 == CurrentDispalyEmails.Count)
            {
                Console.WriteLine("   Search Emails");
                     Console.WriteLine($"   Sender{ConsoleInteraction.GetBuffer(Buffers[0] - 6)}|Recipient{ConsoleInteraction.GetBuffer(Buffers[1] - 9)}|Subject{ConsoleInteraction.GetBuffer(Buffers[2] - 7)}");
                    for (int i = 0; i < CurrentDispalyEmails.Count; i++)
                    {
                        Console.Write("   " + CurrentDispalyEmails[i].GetEmailShort(Buffers) + "\n");
                    }
                    Console.Write(" > Exit");
            }
                else
            {
                     Console.WriteLine("   Search Emails");
                     Console.WriteLine($"   Sender{ConsoleInteraction.GetBuffer(Buffers[0] - 6)}|Recipient{ConsoleInteraction.GetBuffer(Buffers[1] - 9)}|Subject{ConsoleInteraction.GetBuffer(Buffers[2] - 7)}");
                    for (int i = 0; i < CurrentDispalyEmails.Count; i++)
                    {
                        if (i == menuOption)
                        {
                            Console.Write(" > " + CurrentDispalyEmails[i].GetEmailShort(Buffers) + "\n");
                        }
                        else
                        {
                            Console.Write("   " + CurrentDispalyEmails[i].GetEmailShort(Buffers) + "\n");
                        }
                    }
                    Console.Write("   Exit");
                }
        }
        private int FindLongestRecipient()
        {
            int Longest = 0;

            foreach (var email in emails)
            {
                if (Longest < email.GetRecipientLength())
                {
                    Longest = email.GetRecipientLength();
                }
            }
            return Longest;
        }
        private int FindLongestSender()
        {
            int Longest = 0;

            foreach (var email in emails)
            {
                if (Longest < email.GetSenderLength())
                {
                    Longest = email.GetSenderLength();
                }
            }
            return Longest;
        }
        private int FindLongestSubject()
        {
            int Longest = 0;

            foreach (var email in emails)
            {
                if (Longest < email.GetSubjectLength())
                {
                    Longest = email.GetSubjectLength();
                }
            }
            return Longest;
        }
    }
}
