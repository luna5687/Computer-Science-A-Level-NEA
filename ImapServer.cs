// Copyright 2025 Daniel Ian White

using MailKit.Net.Imap;
using MailKit;
using MimeKit;
namespace Computer_Science_A_Level_NEA
{
    public class ImapServer
    {
        private ImapClient client;


        public ImapServer(string emailAddress, string Password, string MailServer)
        {
            client = new ImapClient();
            client.Connect(MailServer, 993, true);
            client.Authenticate(emailAddress, Password);
        }
        public List<Email> GetAllEmails(string accountName)
        {
            List<Email> list = new List<Email>();
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            for (int i = 0; i < inbox.Count; i++)
            {
                var message = inbox.GetMessage(i);

                list.Add(new Email(message.From.ToString(), message.To.ToString(), message.Subject, message.TextBody,message.Date));
            }
            foreach (var email in list) email.RunAutomaticArcive(accountName);
            return list;
        }


    }
}
