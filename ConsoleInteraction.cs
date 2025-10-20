// Copyright 2025 Daniel Ian White
namespace Computer_Science_A_Level_NEA
{
    internal class ConsoleInteraction
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
