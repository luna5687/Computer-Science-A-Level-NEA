using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using Org.BouncyCastle.Tls;
using static System.Runtime.InteropServices.JavaScript.JSType;
// Copyright 2025 Daniel Ian White
namespace Computer_Science_A_Level_NEA
{
    public class ImapServer
    {
        private ImapClient client;
        static private SQLiteConnection connection = new SQLiteConnection("Data Source = Email_Archive.db; Version=3;New=True;Compress=True;");
        public ImapServer(string emailAddress,string Password,string MailServer)
        {
            client = new ImapClient();
            client.Connect(MailServer, 993, true);
            client.Authenticate(emailAddress, Password);
        }



    }
}
