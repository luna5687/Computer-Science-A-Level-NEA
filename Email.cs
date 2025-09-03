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
            if (IsArchived)
            { }
            else
            {
                string[] WordsInBody = Body.Split(' ');
                List<string> FilteredWords = new List<string>();
                string[] WordsToFilter = { "the", " ","\n","\r","\n\r","is","and","a"};
                foreach (string word in WordsInBody)
                {
                    if (!(WordsToFilter.Contains(word.ToLower())))
                    { FilteredWords.Add(word.Replace('\n',' ').Replace('\r', ' ')); }
                }
                List<int> WordIndex = new List<int>();
                for (int i = 0;i < FilteredWords.Count;i++)
                {
                    WordIndex.Add(i);
                }

            }
        }
    }
}
