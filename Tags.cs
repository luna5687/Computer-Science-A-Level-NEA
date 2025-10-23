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
            int key = AllTags.Keys.ToArray()[AllTags.Count - 1] + 1; // may want to change to use ID that are no longer used but not at the end 
            AllTags.Add(key, Value);
            SQLDataBase.ExecuteNonQuery($"INSERT INTO Tags(TagID,TagName)" +
                                        $" VALUES ({key},'{Value}')");
        }
        static public Dictionary<int,string> GetAllTags()
        {
            return AllTags;
        }
    }
}
