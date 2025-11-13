// Copyright 2025 Daniel Ian White
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection.Metadata;

namespace Computer_Science_A_Level_NEA
{
    static class ConsoleInteraction
    {
        static private bool HasConsole;
        static public void CheckConsoleExistance()
        {
            Console.WriteLine("Press enter to start program");
            try
            {
                Console.ReadKey();
                HasConsole = true;
            }
            catch
            {
                Console.ReadLine();
                HasConsole = false;
            }
            if (HasConsole) Console.Write("To navigate use W/S keys and then ENTER to select");
            else Console.Write("To navigate input W/S and press ENTER and to select press ENTER when the input is empty");
            GetConsoleInput();
            Console.Clear();
        }
        public static void ResetCursor()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
        }
        static public string GetConsoleInput()
        {
            if (HasConsole)
            {
                return Console.ReadKey().KeyChar.ToString();
            }
            else
            {
                return Console.ReadLine();
            }
        }
        static public void SetConsoleWidth(int num)
        {
           if (HasConsole) Console.WindowWidth =num;
        }
        static public string GetConsoleInput(bool hiddenCoursor)
        {
            if (HasConsole)
            {
                return Console.ReadKey(hiddenCoursor).KeyChar.ToString();
            }
            else
            {
                return Console.ReadLine();
            }
        }
        static public string GetBuffer(int value)
        {
            string output = "";
            for (int i = 0; i < value; i++) { output+=" "; }
            return output;
        }
        public static int Menu(string message, string[] MenuOptions)
        {
            bool exit = false;
            int menuOption = 0;
            string input;
            while (!exit)
            {
                ResetCursor();
                if(message != "") Console.WriteLine(message);
                for (int i = 0; i < MenuOptions.Length; i++)
                {
                    if (i == menuOption) Console.Write(" > ");
                    else Console.Write("   ");
                }
                input = GetConsoleInput(true);
                if (input.ToLower() == "w")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption =MenuOptions.Length-1;
                    }
                }
                else if (input.ToLower() == "s")
                {
                    menuOption++;
                    if (menuOption > MenuOptions.Length-1 )
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    exit = true;
                }
            }
            return menuOption;
        }

    }
  
}
