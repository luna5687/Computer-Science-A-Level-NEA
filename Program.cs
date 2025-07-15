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
    static class ConsoleInteraction
    {
        static private bool HasConsole;
        static private void CheckConsoleExistance() 
        {
            try
            {
                
                Console.ReadKey();
                Console.WriteLine("Has Console");
                HasConsole = true;
            }
            catch
            {
                Console.ReadLine();
                Console.WriteLine("Doesn't have console");
                HasConsole = false;
            }
        }
    internal class Program
    {

        static void Main(string[] args)
        {
            ConsoleInteraction.CheckConsoleExistance();




        }


    }

}

