using MailKit;
using MailKit.Net.Imap;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
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
             //Emails();
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
            string teststring = "Dear Students,\r\nThis week marks Men’s Health Week  and the EDI Team is proud to shine a spotlight on this important topic.\r\n\r\n\r\n\r\n💙 What is Men’s Health Week?\r\nMen’s Health Week is an annual campaign to raise awareness of the physical and mental health challenges faced by men and boys. This year’s focus is on early action and prevention, encouraging men to check in with their health, talk openly, and seek support when needed.\r\n\r\nFrom heart disease to mental health, men are statistically less likely to seek help. By opening up these conversations, we can break down stigma, promote healthy habits, and support each other to live longer, healthier lives.\r\n\r\n\r\n\r\n💬 Why It Matters\r\n1 in 8 men has a common mental health condition such as anxiety or depression\r\n\r\nMen are more likely to die from preventable conditions due to late diagnosis\r\n\r\nSuicide remains the biggest killer of men under 50 in the UK\r\n\r\nMen’s Health Week is not just for men—it’s for everyone. We all have a role to play in creating a culture where it’s okay to not be okay.\r\n\r\n\r\n\r\n\U0001f91d How You Can Get Involved\r\nCheck in with a mate – A simple “how are you?” can go a long way\r\n\r\nJoin the conversation – Look out for posters, info stands and discussion spaces this week\r\n\r\nEncourage healthy habits – Whether it’s walking together, eating well, or booking a check-up\r\n\r\nBe kind, be open – Mental and physical health affect us all differently. Let’s support without judgement.\r\n\r\n\r\nIf you’re struggling, you are not alone. Support is available:\r\n\r\nCollege Wellbeing Services\r\n\r\nSPA\r\n\r\nTeachers/Tutors\r\n\r\nSamaritans – 116 123 (24/7, free)\r\n\r\nShout - If you'd like a free, confidential and anonymous conversation about how you're feeling, you can also text SHOUT to 85258 to speak to a trained volunteer \r\n\r\n\r\nTogether, we can build a more open, caring and healthy community. Let’s support our friends, challenge stigma, and look after ourselves too.";
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
            for (int i =0; i<WordCount.Count;i++)
            {
                Console.Write(WordCount[i].word+" " + WordCount[i].score);
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
                    words[i].AdjacentWords.RemoveAt(IndexesToRemove[j]-j);
                }
                IndexesToRemove = new List<int>();
                CountedWords = new List<string>();
            }
        }
    }
}