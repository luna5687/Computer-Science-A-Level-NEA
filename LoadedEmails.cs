// Copyright 2025 Daniel Ian White
using System.Linq;

namespace Computer_Science_A_Level_NEA
{

    public class LoadedEmails
    {
        private List<Email> emails;
        private List<Email> CurrentDispalyEmails = new List<Email>();
        private string SenderFilter = "";
        private string SubjectFilter = "";
        private string TagFilter = "";
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
            Console.WindowWidth = 3 + FindLongestSender() + 1 + FindLongestRecipient() + 1 + FindLongestSubject() + 20;
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
                        CurrentDispalyEmails[CurrentDispalyEmails.Count -menuOption].DisplayEmail(this);
                        Console.Clear();
                    }

                }
                ConsoleInteraction.ResetCursor();
            }
        }
        private void DisplaySearchParamters()
        {
            
            Console.CursorTop = 1;
            Console.CursorLeft = 0;
            Console.Write("    " + SenderFilter);
            Console.CursorLeft = 3 + FindLongestSender() + 1 + FindLongestRecipient();
            Console.Write("    " + SubjectFilter);
            Console.CursorTop = 1;
            Console.CursorLeft = 3 + FindLongestSender() + 1 + FindLongestRecipient() + 1 + FindLongestSubject();
            Console.Write("    "+ TagFilter);
        }
        private void SearchForEamils()
        {
            bool Exit = false;
            
            
            string input;
            int menuOption = 0;
            int MaxMenuOption = 3;
            while (!Exit)
            {
                ConsoleInteraction.ResetCursor();
                Console.Write("Please Select Field to filter");
                Console.CursorTop = 0;
                Console.CursorLeft = 29;
                Console.Write("   Exit Search");
                DisplaySearchParamters();
                if (menuOption == 0)
                {
                    Console.CursorTop = 0;
                    Console.CursorLeft = 29;
                    Console.Write(" > ");

                }
                if (menuOption == 1)
                {
                    Console.CursorTop = 1;
                    Console.CursorLeft = 0;
                    Console.Write("   >");

                }
                if (menuOption == 2)
                {
                    Console.CursorTop = 1;
                    Console.CursorLeft = 3 + FindLongestSender() + 1 + FindLongestRecipient();
                    Console.Write("   >");
                }
                if (menuOption == 3)
                {
                    Console.CursorTop = 1;
                    Console.CursorLeft = 3+FindLongestSender()+1+FindLongestRecipient()+1+FindLongestSubject();
                    Console.Write("   >");
                }
                input = ConsoleInteraction.GetConsoleInput(true).ToUpper();
                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = MaxMenuOption;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > MaxMenuOption)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == 0) Exit=true;
                    else if (menuOption == 1)
                    {
                        Console.CursorTop = 1;
                        Console.CursorLeft = 3;
                        SenderFilter = Console.ReadLine();
                    }
                    else if (menuOption == 2) 
                    {
                        Console.CursorTop = 1;
                        Console.CursorLeft = 3 + FindLongestSender() + 1 + FindLongestRecipient()+3;
                        SubjectFilter = Console.ReadLine();
                    }
                    else if (menuOption == 3)
                    {
                        Console.CursorTop = 1;
                        Console.CursorLeft = 3 + FindLongestSender() + 1 + FindLongestRecipient() + 1 + FindLongestSubject()+3+19;
                        TagFilter = Console.ReadLine();
                    }
                }
                Console.Clear();
                UpdateDisplayedEmails();
                DisplayEmails(-1);
            }
        }
        private void UpdateDisplayedEmails()
        {
            CurrentDispalyEmails = new List<Email>();
            foreach (Email email in emails)
            {
                if (email.CheckAgainstFilters(SenderFilter,SubjectFilter,TagFilter))
                {
                    CurrentDispalyEmails.Add(email);
                }
            }
        }
        
        private void DisplayEmails(int menuOption)
        {
            Console.Clear();
            // when displaying emails add headings 
            DisplaySearchParamters();
            ConsoleInteraction.ResetCursor();
            int[] Buffers = { FindLongestSender(), FindLongestRecipient(), FindLongestSubject() };
            if (menuOption == 0)
            {
                Console.WriteLine($" > Search Emails{ConsoleInteraction.GetBuffer(100)}\n");
                Console.WriteLine($"   Sender{ConsoleInteraction.GetBuffer(Buffers[0] - 6)}|Recipient{ConsoleInteraction.GetBuffer(Buffers[1] - 9)}|Subject{ConsoleInteraction.GetBuffer(Buffers[2] - 7)}|Date/Tine Recived  |");
                for (int i = 0; i < CurrentDispalyEmails.Count; i++)
                {
                    Console.Write("   " + CurrentDispalyEmails[CurrentDispalyEmails.Count-1-i].GetEmailShort(Buffers) + "\n");
                }
                Console.Write("   Exit");
            }
            else if (menuOption - 1 == CurrentDispalyEmails.Count)
            {
                Console.WriteLine($"   Search Emails{ConsoleInteraction.GetBuffer(100)}\n");
                Console.WriteLine($"   Sender{ConsoleInteraction.GetBuffer(Buffers[0] - 6)}|Recipient{ConsoleInteraction.GetBuffer(Buffers[1] - 9)}|Subject{ConsoleInteraction.GetBuffer(Buffers[2] - 7)}|Date/Tine Recived  |");
                for (int i = 0; i < CurrentDispalyEmails.Count; i++)
                {
                    Console.Write("   " + CurrentDispalyEmails[CurrentDispalyEmails.Count - 1 - i].GetEmailShort(Buffers) + "\n");
                }
                Console.Write(" > Exit");
            }
            else
            {
                Console.WriteLine($"   Search Emails{ConsoleInteraction.GetBuffer(100)}\n");
                Console.WriteLine($"   Sender{ConsoleInteraction.GetBuffer(Buffers[0] - 6)}|Recipient{ConsoleInteraction.GetBuffer(Buffers[1] - 9)}|Subject{ConsoleInteraction.GetBuffer(Buffers[2] - 7)}|Date/Tine Recived  |");
                for (int i = 0; i < CurrentDispalyEmails.Count; i++)
                {
                    if (i == menuOption - 1)
                    {
                        Console.Write(" > " + CurrentDispalyEmails[CurrentDispalyEmails.Count - 1 - i].GetEmailShort(Buffers) + "\n");
                    }
                    else
                    {
                        Console.Write("   " + CurrentDispalyEmails[CurrentDispalyEmails.Count - 1 - i].GetEmailShort(Buffers) + "\n");
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
        public void RemoveTagFromEmails(int tagID)
        {
            foreach (Email email in emails)
            {
                email.RemoveTagFromEmail(tagID);
            }
        }
    }
}
