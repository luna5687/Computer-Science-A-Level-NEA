// Copyright 2025 Daniel Ian White
using Computer_Science_A_Level_NEA;
using System.Data.SqlTypes;
using System.Data.SQLite;
namespace Computer_Science_A_Level_NEA
{
    public class SQLDataBase
    {
        private SQLiteConnection connection;
        public SQLDataBase(string name, string[] IntalTables)
        {

            if (!File.Exists("Email_Archive.db"))
            {
                SQLiteConnection.CreateFile("Email_Archive.db");
                connection = new SQLiteConnection($"Data Source={name}.db;Version=3;New=True;Compress=True;");
                connection.Open();

                foreach (string s in IntalTables)
                {
                    this.ExecuteNonQuery(s);
                }
            }
            else
            {
                connection = new SQLiteConnection($"Data Source={name}.db;Version=3;New=True;Compress=True;");  
                connection.Open();
            }
        }
        public void ExecuteNonQuery(string Query)
        {
            new SQLiteCommand(Query, connection).ExecuteNonQuery();
        }
        public List<string[]> ExecuteQuery(string Query)
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

                for (int i = 0; i < DR.VisibleFieldCount; i++)
                {
                    Record[i] = DR.GetValue(i).ToString();
                }
                Data.Add(Record);

            }
            DR.Close();
            return Data;
        }
        public void CloseConnection()
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