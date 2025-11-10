using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Copyright 2025 Daniel Ian White
namespace Computer_Science_A_Level_NEA
{
    public class UserMenu
    {
        public static void EmailAddressesMenu(string accountName)
        {
            List<string> EmaliAddreses = GetEmailAddresses( accountName);
            if (EmaliAddreses == null)
            {
                Console.WriteLine("No Email address found please add an email address:");
                CreateEmail( accountName);
            }
            EmaliAddreses = GetEmailAddresses( accountName);
            bool exit = false;
            List<string[]> temp;
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
                    Console.WriteLine(" > Manage Tags");
                    Console.WriteLine("   Manage Emails");
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
                        menuOption = EmaliAddreses.Count + 2;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > EmaliAddreses.Count + 2)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == EmaliAddreses.Count + 2)
                    {
                        exit = true;
                    }
                    else if (menuOption == EmaliAddreses.Count+1)
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
                        EmailMenu(temp[0][0], Encryption.Decrypt(temp[0][1]), temp[0][2]);
                        temp = null;
                        Console.Clear();
                    }

                }
                ConsoleInteraction.ResetCursor(); ;
            }


            // work on text rank on email and automate archive
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

            List<string> EmaliAddreses=GetEmailAddresses(accountName);
            while (EmaliAddreses!=null && EmaliAddreses.Contains(EmailAddress))
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
        static void EmailMenu(string EmailAddress, string EmailPassword, string Mailserver)
        {
            ImapServer imapServer = new ImapServer(EmailAddress, EmailPassword, Mailserver);
            LoadedEmails emalis = new LoadedEmails(imapServer);
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
                        DeleteEmails(EmaliAddreses,accountName);
                        
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
        static void DeleteEmails(List<string> EmailAddresses,string accountName)
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
                EmailAddresses = GetEmailAddresses( accountName);
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
}
