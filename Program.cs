using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
namespace NEA_protoype
{
    internal class Program
    {
        public class wordCount
        {
            public string word;
            public int count;
            public List<string> AdjacentWords;
            public List<int> AdjacentWordsCount;

            public void GetAdjacentWordCount()
            {
                int length = AdjacentWords.Count;
                string currentWord;
                for (int i = 0; i < length; i++)
                {
                    currentWord = AdjacentWords[i]; 
                    AdjacentWordsCount.Add(1);
                    for (int j = 0; j < length; j++)
                    {
                        if (j != i)
                        {
                            if (AdjacentWords[j] == currentWord)
                            {

                                AdjacentWordsCount[i]++;
                                AdjacentWords.RemoveAt(j);
                            }
                        }
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            // Emails();
            textRank();



            Console.ReadKey();
        }
        static void Emails()
        {

            using (var client = new ImapClient())
            {
                client.Connect("mail.ampretia.co.uk", 993, true);
                client.Authenticate("bob@ampretia.co.uk", "passw0rdWibble");
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine("From: " + message.From);
                    Console.WriteLine("Body: " + message.Body);
                }
            }

        }
        static void textRank()
        {
            string teststring = "Hello\r\n\r\nThank you all for handing in your forms for Japan.\r\n\r\nI just wanted to let you know that I am letting people have till 19th June to hand in their forms.\r\nThen, if it is oversubscribed, then I will pick names randomly.\r\n\r\nI will let you know as soon as I can - probably on the 20th June.\r\n\r\nThank you \r\nIsabel";
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
            int[,] Matrix = new int[WordCount.Count, WordCount.Count];


        }
    }
}