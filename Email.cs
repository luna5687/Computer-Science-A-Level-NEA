using System.Data;
using System.Runtime.CompilerServices;
// Copyright 2025 Daniel Ian White
namespace Computer_Science_A_Level_NEA
{
    public class Email
    {
        private bool IsArchived = false;
        private string Sender;
        private string Recipient;
        private string Subject;
        private string Body;
        private List<string> Keywords;
        private List<string> Tags;
        private int EmailID;
        public Email(string Sender, string Recipient, string Subject, string Body)
        {
            this.Sender = Sender;
            this.Recipient = Recipient;
            this.Subject = Subject;
            this.Body = Body;

            CreateKeywords();
        }
        public string GetEmailShort()
        {
            string output;
            if (IsArchived)
            {
                output = Sender + " " + Recipient + " " + Subject + " " + "A";
                for (int i = 0; i < Tags.Count; i++)
                {
                    output += " " + Tags[i];
                }
            }
            else
            {
                output = Sender + " " + Recipient + " " + Subject + " " + " ";
                
            }
            return output;
        }
        public void DisplayEmail()
        {
            Console.Clear();
            string Tags = "";
            if (this.Tags != null)
            { 
                for (int i = 0; i < this.Tags.Count(); i++)
                {
                    Tags += this.Tags[i] + " ";
                }
            }
            string keywords = "";
            //for (int i = 0; i < this.Keywords.Count(); i++) { keywords += this.Keywords[i] + " "; }
            Console.Clear();

            Console.WriteLine("> Back");
            Console.WriteLine($"From: {Sender} To: {Recipient}\n" +
                              $"Subject: {Subject} Tags: {Tags} KeyWords: {keywords}\n\n" +
                              $"{Body}");
            ConsoleInteraction.GetConsoleInput();
            Console.Clear();
        }
        private void CreateKeywords()
        {
            char[] Body = this.Body.ToCharArray();
            List<string> FilteredWords = new List<string>();
            string[] WordsToFilter = { "the", "\n", "\r", "is", "and", "a" };
            string Temp = "";
            string text = "";
            if (IsArchived)
            { /* retreve keywords from archive */ }
            else
            {
               foreach (string word in WordsToFilter)
                {
                    for (int i = 0;i < Body.Length-word.Length;i++)
                    {
                        Temp += "";
                        for (int j = 0;j < word.Length;j++)
                        {
                            Temp += Body[i + j];
                        }
                        if (Temp == word)
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

                
                
            }
        }
    }
}
