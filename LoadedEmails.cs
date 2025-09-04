using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void EmailMenu()
        {
            Console.Clear();
            bool exit = false;
            int menuOption = 0;
            while (!exit)
            {
                if (menuOption == emails.Count)
                {
                    for (int i = 0; i < emails.Count; i++)
                    {
                        Console.Write("   " + emails[i].GetEmailShort() + "\n");
                    }
                    Console.Write(" > Exit");
                }
                else
                {
                    for (int i = 0; i < emails.Count; i++)
                    {
                        if (i == menuOption)
                        {
                            Console.Write(" > " + emails[i].GetEmailShort() + "\n");
                        }
                        else
                        {
                            Console.Write("   " + emails[i].GetEmailShort() + "\n");
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
                else if (input == "\r"||input=="")
                {
                    if (menuOption == emails.Count)
                    {
                        exit = true;
                    }
                    else
                    {
                        // display emails
                        emails[menuOption].DisplayEmail();
                        Console.Clear();
                    }

                }
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
            }
        }
    }
}
