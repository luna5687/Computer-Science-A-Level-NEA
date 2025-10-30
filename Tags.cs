using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Science_A_Level_NEA
{
    static class Tags
    {
        static private Dictionary<int, string> AllTags = new Dictionary<int, string>();
        static public void LoadTags()
        {
            List<string[]> TagsInDataBase = SQLDataBase.ExecuteQuery("SELECT * FROM Tags");
            foreach (string[] s in TagsInDataBase)
            {
                AllTags.Add(int.Parse(s[0]), s[1]);
            }
        }
        static public void CreateNewTag(string Value)
        {
            int key = 0;
            while (AllTags.ContainsKey(key)) { key++; }
            AllTags.Add(key, Value);
            SQLDataBase.ExecuteNonQuery($"INSERT INTO Tags(TagID,TagName)" +
                                        $" VALUES ({key},'{Value}')");
        }
        static public Dictionary<int, string> GetAllTags()
        {
            return AllTags;
        }
        static void DeleteTag(int TagID)
        {
            Console.Clear();
            SQLDataBase.ExecuteNonQuery($"DELETE FROM Tags WHERE TagID == {TagID}");
            SQLDataBase.ExecuteNonQuery($"DELETE FROM AssignedTags WHERE TagID == {TagID}");
            
            AllTags.Remove(TagID);
        }
        static void DeleteTag(int TagID,LoadedEmails AllEmails)
        {
            Console.Clear();
            SQLDataBase.ExecuteNonQuery($"DELETE FROM Tags WHERE TagID == {TagID}");
            SQLDataBase.ExecuteNonQuery($"DELETE FROM AssignedTags WHERE TagID == {TagID}");
            AllEmails.RemoveTagFromEmails(TagID);
            AllTags.Remove(TagID);
        }
        static public void DeleteTagMenu()
        {
            Console.Clear();
            bool exit = false;
            int menuOption = 0;
            int count = 0;
            string input;
            while (!exit)
            {
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
                Console.WriteLine("Please select emails to be deleted");
                if (menuOption == 0)
                {
                    Console.WriteLine(" > Back");
                    foreach(var t  in AllTags) Console.WriteLine("   "+t.Value);
                }
                else
                {
                    Console.WriteLine("   Back");
                    count = 0;  
                    foreach(var t in AllTags)
                    {
                        if (count == menuOption-1) Console.WriteLine(" > " + t.Value);
                        else Console.WriteLine("   " + t.Value);
                        count ++;
                    }
                }
                input = ConsoleInteraction.GetConsoleInput(true).ToUpper();
                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0) menuOption = AllTags.Count;
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > AllTags.Count) menuOption = 0;
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == 0)
                    {

                        exit = true;
                    }
                    else
                    {
                        count = 0;
                        foreach (var t in AllTags)
                        {
                            if (count == menuOption - 1) DeleteTag(t.Key);
                            count++;
                        }
                    }
                }
            }
            Console.Clear();
        }
        static public void DeleteTagMenu(LoadedEmails AllEmails)
        {
            Console.Clear();
            bool exit = false;
            int menuOption = 0;
            int count = 0;
            string input;
            while (!exit)
            {
                Console.CursorTop = 0;
                Console.CursorLeft = 0;
                Console.WriteLine("Please select emails to be deleted");
                if (menuOption == 0)
                {
                    Console.WriteLine(" > Back");
                    foreach (var t in AllTags) Console.WriteLine("   " + t.Value);
                }
                else
                {
                    Console.WriteLine("   Back");
                    count = 0;
                    foreach (var t in AllTags)
                    {
                        if (count == menuOption - 1) Console.WriteLine(" > " + t.Value);
                        else Console.WriteLine("   " + t.Value);
                        count++;
                    }
                }
                input = ConsoleInteraction.GetConsoleInput(true).ToUpper();
                if (input == "W")
                {
                    menuOption--;
                    if (menuOption < 0) menuOption = AllTags.Count;
                }
                else if (input == "S")
                {
                    menuOption++;
                    if (menuOption > AllTags.Count) menuOption = 0;
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == 0)
                    {

                        exit = true;
                    }
                    else
                    {
                        count = 0;
                        foreach (var t in AllTags)
                        {
                            if (count == menuOption - 1) DeleteTag(t.Key, AllEmails);
                            count++;
                        }
                    }
                }
            }
            Console.Clear();
        }
        static public void TagManagement()
        {
            Console.Clear();
            bool exit = false;
          
            int menuOption = 0;
            string input;
            string[] MenuOptions = { "Create tag", "Delete Tag", "Back" };
            while (!exit)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                Console.WriteLine("All Tags: ");
                foreach (var t in AllTags) Console.WriteLine(" "+t.Value);
                Console.WriteLine("");
                for (int i = 0; i < MenuOptions.Length; i++)
                {
                    if (menuOption == i) Console.Write(" > ");
                    else Console.Write("   ");
                    Console.WriteLine(MenuOptions[i]);
                }
                input = ConsoleInteraction.GetConsoleInput(true);
                if (input.ToLower() == "w")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = MenuOptions.Length - 1;
                    }
                }
                else if (input.ToLower() == "s")
                {
                    menuOption++;
                    if (menuOption == MenuOptions.Length)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    switch (MenuOptions[menuOption])
                    {
                        case "Back":
                            exit = true;
                            break;
                        
                        case "Create tag":
                            Console.Write("Please Enter name for tag: ");
                            Tags.CreateNewTag(Console.ReadLine());
                            Console.Clear();
                            break;
                        case "Delete Tag":
                            DeleteTagMenu();
                            break;
                    }
                }
            }
        }
    }
}
