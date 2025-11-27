// Copyright 2025 Daniel Ian White

using MailKit.Net.Imap;
using MailKit;
using MimeKit;
namespace Computer_Science_A_Level_NEA
{
    public class ImapServer
    {
        private ImapClient client;

        private Dictionary<int,List<Email>> AllEmailsInServer;
        public ImapServer(string emailAddress, string Password, string MailServer)
        {
            client = new ImapClient();
            client.Connect(MailServer, 993, true);
            client.Authenticate(emailAddress, Password);
        }
        public List<Email> GetAllEmails(string accountName)
        {
            List<Email> list = new List<Email>();
            List<Task> tasks = new List<Task>();
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            for (int i = 0; i < inbox.Count; i++)
            {
                var message = inbox.GetMessage(i);

                list.Add(new Email());
                tasks.Add(new Task(() => list[list.Count - 1].SetUp(message.From.ToString(), message.To.ToString(), message.Subject, message.TextBody, message.Date)));
                tasks[tasks.Count - 1].Start();
            }
            
            Task.WaitAll(tasks.ToArray());
            foreach (var email in list) email.RunAutomaticArcive(accountName);
            return list;
        }
        

    }
}
