// Copyright 2025 Daniel Ian White
using Microsoft.VisualBasic;
using System.Data.SQLite;
namespace Computer_Science_A_Level_NEA
{
    public static class SQLDataBase
    {
        static private SQLiteConnection connection;
        static private int MaxMemory = -1;
        static private string OverFlowType = "Error";
        private struct Date
        {
            public int year;
            public int month;
            public int day;
            public int hour;
            public int minute;
            public int second;
        }

        static public void CreateDataBase(string name, string[] IntalTables)
        {

            if (!File.Exists("Email_Archive.db"))
            {
                SQLiteConnection.CreateFile("Email_Archive.db");
                connection = new SQLiteConnection($"Data Source={name}.db;Version=3;New=True;Compress=True;");
                connection.Open();

                foreach (string s in IntalTables)
                {
                    ExecuteNonQuery(s);
                }
            }
            else
            {
                connection = new SQLiteConnection($"Data Source={name}.db;Version=3;New=True;Compress=True;");
                connection.Open();
            }

        }
        static public void SetMaxSize(int max)
        {
            MaxMemory = max;
        }
        static public void SetOverFlowType(string overflow)
        {
            OverFlowType = overflow;
        }
        static public bool IsFull()
        {
            if (MaxMemory == -1) return false;
            FileSystem.FileCopy("Email_Archive.db", "TempFile");
            int FileNumber = FileSystem.FreeFile();
            FileSystem.FileOpen(FileNumber, "TempFile", OpenMode.Input);
            bool full = FileSystem.LOF(FileNumber) > MaxMemory;
            File.Delete("TempFile");
            return full;
        }

        static public void ExecuteNonQuery(string Query)
        {
            if (Query.StartsWith("INSERT") || Query.StartsWith("CREATE"))
            {
                if (!IsFull()) new SQLiteCommand(Query, connection).ExecuteNonQuery();// need to sanitize 
            }
            else
            {
                new SQLiteCommand(Query, connection).ExecuteNonQuery();
            }
            if (IsFull() && OverFlowType == "Error") throw new DataBaseFullExeption();
            else if (IsFull() && OverFlowType == "Delete Oldest") ResolveOverFlow();
        }
        public static void ResolveOverFlow()
        {
            List<string[]> AllDates = ExecuteQuery("SELECT EmailId,DateReviced FROM Emails");
            List<Date> Dates = new List<Date>();
            string SingleDate;
            Date TempDate;
            foreach (string[] s in AllDates)
            {
                TempDate = new Date();
                SingleDate = s[1];
                TempDate.day = int.Parse(SingleDate.Split(' ')[0].Split('/')[0]);
                TempDate.month = int.Parse(SingleDate.Split(' ')[0].Split('/')[1]);
                TempDate.month = int.Parse(SingleDate.Split(' ')[0].Split('/')[2]);
                TempDate.hour = int.Parse(SingleDate.Split(' ')[1].Split(':')[0]);
                TempDate.minute = int.Parse(SingleDate.Split(' ')[1].Split(':')[1]);
                TempDate.second = int.Parse(SingleDate.Split(' ')[1].Split(':')[2]);
                Dates.Add(TempDate);
            }
            int IndexOfOldest = 0;
            while (IsFull() && AllDates.Count > 0)
            {
                IndexOfOldest = 0;
                for (int i = 0; i < AllDates.Count; i++)
                {
                    if (Dates[i].year < Dates[IndexOfOldest].year) IndexOfOldest = i;
                    else if (Dates[i].year <= Dates[IndexOfOldest].year && Dates[i].month < Dates[IndexOfOldest].month) IndexOfOldest = i;
                    else if (Dates[i].year <= Dates[IndexOfOldest].year && Dates[i].month <= Dates[IndexOfOldest].month && Dates[i].day < Dates[IndexOfOldest].day) IndexOfOldest = i;
                    else if (Dates[i].year <= Dates[IndexOfOldest].year && Dates[i].month <= Dates[IndexOfOldest].month && Dates[i].day <= Dates[IndexOfOldest].day 
                        && Dates[i].hour < Dates[IndexOfOldest].hour) IndexOfOldest = i;
                    else if (Dates[i].year <= Dates[IndexOfOldest].year && Dates[i].month <= Dates[IndexOfOldest].month && Dates[i].day <= Dates[IndexOfOldest].day 
                        && Dates[i].hour <= Dates[IndexOfOldest].hour && Dates[i].minute < Dates[IndexOfOldest].minute) IndexOfOldest = i;
                    else if (Dates[i].year <= Dates[IndexOfOldest].year && Dates[i].month <= Dates[IndexOfOldest].month && Dates[i].day <= Dates[IndexOfOldest].day
                        && Dates[i].hour <= Dates[IndexOfOldest].hour && Dates[i].minute <= Dates[IndexOfOldest].minute && Dates[i].second < Dates[IndexOfOldest].second) IndexOfOldest = i;
                }
                ExecuteNonQuery($"DELETE FROM Emails WHERE EmailID == {AllDates[IndexOfOldest][0]}");
                ExecuteNonQuery($"DELETE FROM AssignedTags WHERE EmailID == {AllDates[IndexOfOldest][0]}");
                AllDates.RemoveAt(IndexOfOldest);
                Dates.RemoveAt(IndexOfOldest);
            }
        }
        static public List<string[]> ExecuteQuery(string Query)
        {
            List<string[]> Data = new List<string[]>();
            SQLiteDataReader DR = new SQLiteCommand(Query, connection).ExecuteReader();
            string[] Record = new string[DR.VisibleFieldCount];

            if (DR.StepCount == 0)
            {
                DR.Close();
                return null;
            }
            while (DR.Read())
            {
                Record = new string[DR.VisibleFieldCount];
                for (int i = 0; i < DR.VisibleFieldCount; i++)
                {
                    Record[i] = DR.GetValue(i).ToString();
                }
                Data.Add(Record);

            }
            DR.Close();
            return Data;
        }
        static public void CloseConnection()
        {
            connection.Close();
        }
    }
}
/*  new SQLiteCommand("CREATE TABLE Users (" +
                                  "EmailAddress varchar PRIMARY KEY," +
                                  "Password varchar," +
                                  "Mailserver varchar," +
                                  "Account varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Emails(" +
                                  "EmailID int PRIMARY KEY," +
                                  "Sender varchar," +
                                  "Recipient varchar," +
                                  "Subject varchar," +
                                  "TextBody varchar," +
                                  "Keywords varchar," +
                                  "EmailAddress varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Tags(" +
                                  "TagID int PRIMARY KEY," +
                                  "TagName varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE AssignedTags(" +
                                  "TagID int," +
                                  "EmailID int)", connection).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE Accounts(" +
                                  "AccountName varchar PRIMARY KEY," +
                                  "Password varchar)", connection).ExecuteNonQuery();
                new SQLiteCommand("INSERT INTO Tags(tagID,TagName)" +
                                  "VALUES " +
                                  "(0,'Metting')," +
                                  "(1,'Accounting')," +
                                  "(2,'Important')", connection).ExecuteNonQuery();
*/