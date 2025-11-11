// Copyright 2025 Daniel Ian White
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
    }
  
}
