// Copyright 2025 Daniel Ian White
using Org.BouncyCastle.Cms;

namespace Computer_Science_A_Level_NEA
{

    public class LoadedEmails
    {
        private List<Email> emails;
        public LoadedEmails(ImapServer server)
        {
            emails = server.GetAllEmails();
        }
        public Email GetEmail(int index)
        {
            return emails[index];
        }
        public void EmailMenu(SQLDataBase dataBase)
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
                        menuOption = emails.Count;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > emails.Count)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == emails.Count)
                    {
                        exit = true;
                    }
                    else
                    {
                        // display emails
                        emails[menuOption].DisplayEmail(dataBase);
                        Console.Clear();
                    }

                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }
        }
        private void DisplayEmails(int menuOption)
        {
            // when displaying emails add headings 
            int[] Buffers = { FindLongestSender(), FindLongestRecipient(),FindLongestSubject()};
            Console.WriteLine($"   Sender{ConsoleInteraction.GetBuffer(Buffers[0] - 6)}|Recipient{ConsoleInteraction.GetBuffer(Buffers[1] - 9)}|Subject{ConsoleInteraction.GetBuffer(Buffers[2] - 7)}");
            if (menuOption == emails.Count)
                {
                    for (int i = 0; i < emails.Count; i++)
                    {
                        Console.Write("   " + emails[i].GetEmailShort(Buffers) + "\n");
                    }
                    Console.Write(" > Exit");
                }
                else
                {
                    for (int i = 0; i < emails.Count; i++)
                    {
                        if (i == menuOption)
                        {
                            Console.Write(" > " + emails[i].GetEmailShort(Buffers) + "\n");
                        }
                        else
                        {
                            Console.Write("   " + emails[i].GetEmailShort(Buffers) + "\n");
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
