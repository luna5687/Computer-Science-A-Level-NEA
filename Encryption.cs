using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Copyright 2025 Daniel Ian White
namespace Computer_Science_A_Level_NEA
{
    static public class Encryption
    {
        static private int offset = 31;
        static public string Encrypt(string input)
        {
            string output = "";
            foreach(char c in input)
            {
                if ((int)c + offset > 126)
                {
                    output += (char)((((int)c + offset) - 126)+31);
                } 
                else
                {
                    output += (char)((int)c + offset);
                }
                
            }
            return output;
        }
        static public string Decrypt(string input)
        {
            string output = "";
            foreach (char c in input)
            {
                if ((int)c - offset < 32)
                {
                    output += (char)((((int)c - 31 ) + 126) - offset);
                }
                else
                {
                    output += (char)((int)c - offset);
                }

            }
            return output;
        }
    }
}
