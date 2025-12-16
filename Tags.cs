// Copyright 2025 Daniel Ian White
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
        static void DeleteTag(int TagID, LoadedEmails AllEmails)
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

            while (!exit)
            {
                ConsoleInteraction.ResetCursor();
                string[] StringTempArray = new string[AllTags.Count + 1];
                StringTempArray[0] = "Back";
                count = 0;
                foreach (var t in AllTags)
                {
                    StringTempArray[count + 1] = t.Value;
                    count++;
                }


                menuOption = ConsoleInteraction.Menu("Please select tags to be deleted", StringTempArray, menuOption);

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
            Console.Clear();
        }
        static public void DeleteTagMenu(LoadedEmails AllEmails)
        {
            Console.Clear();
            bool exit = false;
            int menuOption = 0;
            int count = 0;

            while (!exit)
            {
                ConsoleInteraction.ResetCursor();
                string[] StringTempArray = new string[AllTags.Count + 1];
                StringTempArray[0] = "Back";
                count = 0;
                foreach (var t in AllTags)
                {
                    StringTempArray[count + 1] = t.Value;
                    count++;
                }


                menuOption = ConsoleInteraction.Menu("Please select tags to be deleted", StringTempArray, menuOption);

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
                ConsoleInteraction.ResetCursor();
                string TempString = "All Tags: \n";
                
                foreach (var t in AllTags) TempString+=" " + t.Value+"\n";
                TempString += "\n";

                
                menuOption = ConsoleInteraction.Menu(TempString, MenuOptions, menuOption);
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
