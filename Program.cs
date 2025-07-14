using MailKit;
using MailKit.Net.Imap;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Net;
// Copyright 2025 Daniel Ian White

// Bobs email: bob@ampretia.co.uk password: passw0rdWibble Mailserver: mail.ampretia.co.uk

namespace NEA_protoype
{
    internal class Program
    {
        public class wordCount
        {
            public string word;
            public int count;
            public List<string> AdjacentWords = new List<string>();
            public List<int> AdjacentWordsCount = new List<int>();
            public int score;
            public void CaculateScore()
            {
                for (int i = 0; i < AdjacentWordsCount.Count; i++)
                {
                    score += AdjacentWordsCount[i];
                }
            }

        }
        static void Main(string[] args)
        {
            string EmailAddress = "bob@ampretia.co.uk";
            string EmailAddressPassWord = "passw0rdWibble";
            string EmailAddressMailServer = "mail.ampretia.co.uk";
            bool Exit = false;
            string[] Options = { "View emails", "Settings", "Exit" };
            int menuOption = 0;
            string input;

            while (!Exit)
            {

                for (int i = 0; i < Options.Length; i++)
                {
                    if (menuOption == i)
                    {
                        Console.WriteLine($" > {Options[i]}");
                    }
                    else
                    {
                        Console.WriteLine($"   {Options[i]}");
                    }
                }
                input = Console.ReadLine().ToUpper();
                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = Options.Length - 1;
                    }
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption >= Options.Length)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "")
                {
                    if (Options[menuOption] == "View emails")
                    {
                        Emails(ref EmailAddress, ref EmailAddressPassWord, ref EmailAddressMailServer);
                    }
                    if (Options[menuOption] == "Setting")
                    {
                        Console.WriteLine("No settings avilable");
                    }
                    if (Options[menuOption] == "Exit")
                    {
                        Exit = true;
                    }
                }

            }


            /*
            IPHostEntry host = Dns.GetHostEntry("ampretia.co.uk"); // found on microsoft website

                foreach (IPAddress address in host.AddressList)
                {
                    Console.WriteLine( address.);
                }
    */








        }
        static void Emails(ref string EmailAddress, ref string EmailPassword, ref string EmailAddressMailServer)
        {
            if (EmailAddress == null)
            {
                Console.Write("Please enter your email: ");
                EmailAddress = Console.ReadLine();
            }
            if (EmailPassword == null)
            {
                Console.Write("Please enter your password: ");
                EmailPassword = Console.ReadLine();
            }





            //ReadAllEmails("bob@ampretia.co.uk", "passw0rdWibble", "mail.ampretia.co.uk");
            ListAllEmails("bob@ampretia.co.uk", "passw0rdWibble", "mail.ampretia.co.uk");

        }
        static void ReadAllEmails(string email, string password, string MailServer)
        {
            using (var client = new ImapClient())
            {
                client.Connect(MailServer, 993, true);
                client.Authenticate(email, password);
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine("From: " + message.From);
                    Console.WriteLine("Body: " + message.TextBody);
                    Console.WriteLine("Keywords: ");
                    textRank(message.TextBody);
                    Console.Write("\n-------------------------------------------------------------------------------------------------------\n");
                }
            }
        }
        static void ListAllEmails(string email, string password, string MailServer)
        {
            bool Exit = false;
            int menuOption = 0;
            string input;
            using (ImapClient client = new ImapClient())
            {
                client.Connect(MailServer, 993, true);
                client.Authenticate(email, password);
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                while (!Exit)
                {
                    Console.Clear();
                    if (menuOption == 0)
                    {
                        Console.WriteLine(" > Back");
                    }
                    else
                    {
                        Console.WriteLine("   Back");
                    }
                    for (int i = 0; i < inbox.Count; i++)
                    {
                        var message = inbox.GetMessage(inbox.Count - i - 1);
                        if (menuOption - 1 == i)
                        {
                            Console.Write($" > From: {message.From}, Subject: {message.Subject}, Date recived: {message.Date}\n");
                        }
                        else
                        {
                            Console.Write($"   From: {message.From}, Subject: {message.Subject}, Date recived: {message.Date}\n");
                        }

                    }
                    input = Console.ReadLine().ToUpper();
                    if (input == "W")
                    {
                        menuOption--;
                        if (menuOption < 0)
                        {
                            menuOption = inbox.Count;
                        }
                    }
                    else if (input == "S")
                    {
                        menuOption++;
                        if (menuOption > inbox.Count)
                        {
                            menuOption = 0;
                        }
                    }
                    else if (input == "")
                    {
                        if (menuOption == 0)
                        {
                            Exit = true;
                        }
                        else
                        {  // Add feature to close open message
                            Console.Clear() ;
                            var message = inbox.GetMessage((inbox.Count) - menuOption);
                            Console.Write($"From: {message.From} \nSubject: {message.Subject}\nBody:\n{message.TextBody}");
                            textRank(message.TextBody);
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }

            }
        }
        static void textRank(string input)
        {

            string teststring = input;
            teststring = teststring.Replace("\r\n", " ");
            teststring = teststring.Replace(".", " .");
            string[] words = teststring.Split(' ');

            List<wordCount> WordCount = new List<wordCount>();
            wordCount ToAdd;

            for (int j = 0; j < words.Length; j++)

            {
                string word = words[j];
                bool InList = false;
                for (int i = 0; i < WordCount.Count; i++)
                {
                    if (WordCount[i].word == word)
                    {
                        InList = true;
                        WordCount[i].count++;
                        if (j != 0)
                        {
                            WordCount[i].AdjacentWords.Add(words[j - 1]);
                        }
                        if (j != words.Length - 1)
                        {
                            WordCount[i].AdjacentWords.Add(words[j + 1]);
                        }
                    }
                }
                if (!InList)
                {
                    ToAdd = new wordCount();
                    ToAdd.word = word;
                    ToAdd.count = 1;
                    if (j != 0)
                    {
                        ToAdd.AdjacentWords.Add(words[j - 1]);
                    }
                    if (j != words.Length - 1)
                    {
                        ToAdd.AdjacentWords.Add(words[j + 1]);
                    }
                    WordCount.Add(ToAdd);
                    ToAdd = null;
                }
            }
            CountAdjacents(ref WordCount);
            int[,] Matrix = new int[WordCount.Count, WordCount.Count];
            for (int i = 0; i < WordCount.Count; i++)
            {
                WordCount[i].CaculateScore();
            }
            for (int i = 0; i < WordCount.Count; i++)
            {
                Console.Write(WordCount[i].word + " " + WordCount[i].score);
                /*
                for (int j =0; j<WordCount[i].AdjacentWords.Count;j++)
                {
                    Console.Write(" "+ WordCount[i].AdjacentWords[j]+" " + WordCount[i].AdjacentWordsCount[j]+",");
                }
                */
                Console.Write("\n");
            }

        }
        static void CountAdjacents(ref List<wordCount> words)
        {
            List<string> CountedWords = new List<string>();
            List<int> IndexesToRemove = new List<int>();
            for (int i = 0; i < words.Count; i++)
            {
                for (int j = 0; j < words[i].AdjacentWords.Count; j++)
                {
                    if (CountedWords.Contains(words[i].AdjacentWords[j]))
                    {
                        words[i].AdjacentWordsCount[CountedWords.IndexOf(words[i].AdjacentWords[j])]++;
                        IndexesToRemove.Add(j);
                    }
                    else
                    {
                        words[i].AdjacentWordsCount.Add(1);
                        CountedWords.Add(words[i].AdjacentWords[j]);
                    }
                }
                for (int j = 0; j < IndexesToRemove.Count; j++)
                {
                    words[i].AdjacentWords.RemoveAt(IndexesToRemove[j] - j);
                }
                IndexesToRemove = new List<int>();
                CountedWords = new List<string>();
            }
        }
    }
}