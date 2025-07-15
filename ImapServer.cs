using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using Org.BouncyCastle.Tls;
// Copyright 2025 Daniel Ian White
namespace Computer_Science_A_Level_NEA
{
    public class ImapServer
    {
        private ImapClient client;
       
        public ImapServer(string emailAddress,string Password,string MailServer)
        {
            client = new ImapClient();
            client.Connect(MailServer, 993, true);
            client.Authenticate(emailAddress, Password);
        }



    }
}
