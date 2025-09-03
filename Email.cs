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
                for (int i = 0; i < Tags.Count; i++)
                {
                    output += " " + Tags[i];
                }
            }
            return output;
        }
        public void DisplayEmail()
        {
            string Tags = "";
            for (int i = 0; i < this.Tags.Count(); i++)
            {
                Tags += this.Tags[i] + " ";
            }
            string keywords = "";
            for (int i = 0; i < this.Keywords.Count(); i++) { keywords += this.Keywords[i] + " "; }
            Console.Clear();

            Console.WriteLine("Back");
            Console.WriteLine($"From: {Sender} To: {Recipient}\n" +
                              $"Subject: {Subject} Tags: {Tags} KeyWords: {keywords}\n\n" +
                              $"{Body}");
            ConsoleInteraction.GetConsoleInput();
        }
    }
}
