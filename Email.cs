using NEA_protoype;
using Org.BouncyCastle.Asn1.Misc;
// Copyright 2025 Daniel Ian White
namespace Computer_Science_A_Level_NEA
{
    public class Email
    {
        private bool IsArchived = false;
        private string Sender;
        private string Recipient;
        private string Subject;
        private string Body;
        private List<string> Keywords;
        private Dictionary<int, string> EmailTags = new Dictionary<int, string>();
        private int EmailID;
        private int DataBaseID;
        private string DateRecived;
        public Email(string Sender, string Recipient, string Subject, string Body, DateTimeOffset date)
        {
            this.Sender = Sender;
            this.Recipient = Recipient;
            this.Subject = Subject;
            this.Body = Body;

            DateRecived = date.DateTime.ToString();

            EmailID = Sender.Length + Recipient.Length + Subject.Length + Body.Length; 
            CheckArchived();
            if (!IsArchived) CreateKeywords();
        }
        public int GetRecipientLength()
        {
            return Recipient.Length;
        }
        public int GetSenderLength()
        {
            return Sender.Length;
        }
        public int GetSubjectLength()
        {
            return Subject.Length;
        }
        public bool CheckAgainstFilters(string SenderFilter, string SubjectFilter, string TagFilter)
        {

            if (SenderFilter != "" && SenderFilter != Sender) return false;
            if (SubjectFilter != "" && SubjectFilter != Subject) return false;
            if (TagFilter != "" && EmailTags != null)
            {
                List<string> Tags = new List<string>();
                foreach (var t in EmailTags)
                {
                    Tags.Add(t.Value);
                }
                return Tags.Contains(TagFilter);
            }
            return true;

        }
        private void CheckArchived()
        {
            IsArchived = false;
            List<string[]> AllEmailIDsInDataBase = SQLDataBase.ExecuteQuery("SELECT EmailID, CollisionAt FROM Emails,Collisions");
            List<string[]> EmailData = null;
            bool found;
            DataBaseID = EmailID;
            if (AllEmailIDsInDataBase != null)
            {
                foreach (string[] s in AllEmailIDsInDataBase)
                {
                    if (s[0] == DataBaseID.ToString())
                    {
                        EmailData = SQLDataBase.ExecuteQuery($"SELECT * FROM Emails WHERE EmailID == {DataBaseID}");
                        while (!CheckIfDataMatches(EmailData) && (CheckIDIsInEmailsTable(DataBaseID) || CheckIDIsInCollisions(DataBaseID)))
                        {
                            DataBaseID += 1;
                            if (CheckIDIsInEmailsTable(DataBaseID))
                            {
                                EmailData = SQLDataBase.ExecuteQuery($"SELECT * FROM Emails WHERE EmailID == {DataBaseID}");
                            }
                            else if (CheckIDIsInCollisions(DataBaseID))
                            {
                                DataBaseID += 1;
                            }
                        }
                    }
                    else if (s[1] == EmailID.ToString())
                    {
                        DataBaseID++;
                        while (EmailData == null && (CheckIDIsInEmailsTable(DataBaseID) || CheckIDIsInCollisions(DataBaseID)))
                        {
                            if (CheckIDIsInEmailsTable(DataBaseID)) EmailData = SQLDataBase.ExecuteQuery($"SELECT * FROM Emails WHERE EmailID == {DataBaseID}");
                            else DataBaseID++;
                        }
                        while ((CheckIDIsInEmailsTable(DataBaseID) || CheckIDIsInCollisions(DataBaseID)) &&!CheckIfDataMatches(EmailData))
                        {
                            DataBaseID += 1;
                            if (CheckIDIsInEmailsTable(DataBaseID))
                            {
                                EmailData = SQLDataBase.ExecuteQuery($"SELECT * FROM Emails WHERE EmailID == {DataBaseID}");
                            }
                            else if (CheckIDIsInCollisions(DataBaseID))
                            {
                                DataBaseID += 1;
                            }
                        }
                    }

                }

                if (!(EmailData == null) && CheckIfDataMatches(EmailData)) LoadArchiveData();
            }

        }
        private void LoadArchiveData()
        {
            IsArchived = true;
            List<string[]> AllTagIDS = SQLDataBase.ExecuteQuery($"SELECT TagID FROM AssignedTags WHERE EmailID == {DataBaseID}");
            if (!(AllTagIDS == null || AllTagIDS.Count == 0)) foreach (string[] s in AllTagIDS) EmailTags.Add(int.Parse(s[0]), SQLDataBase.ExecuteQuery($"SELECT TagName FROM Tags WHERE TagId == {int.Parse(s[0])}")[0][0]);
            List<string[]> Keywords = SQLDataBase.ExecuteQuery($"SELECT Keywords FROM Emails WHERE EmailID == {DataBaseID}");
            this.Keywords = new List<string>();
            foreach (string[] s in Keywords) this.Keywords.Add(s[0]);
            

        }
        private bool CheckIfDataMatches(List<string[]> DataBaseData)
        {
            if (DataBaseData == null) return false;
            foreach (string[] s in DataBaseData)
            {
                if (s[1] != Sender) return false;
                if (s[2] != Recipient) return false;
                if (s[3] != Subject) return false;
                if (s[4] != Body) return false;
            }
            return true;
        }
        private bool CheckIDIsInEmailsTable(int ID)
        {
            List<string[]> AllEmailIDsInDataBase = SQLDataBase.ExecuteQuery("SELECT EmailID FROM Emails");
            foreach (string[] s in AllEmailIDsInDataBase)
            {
                if (s[0] == ID.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckIDIsInCollisions(int ID)
        {
            List<string[]> AllEmailIDsInDataBase = SQLDataBase.ExecuteQuery("SELECT * FROM Collisions");
            foreach (string[] s in AllEmailIDsInDataBase)
            {
                if (s[0] == ID.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        public string GetEmailShort(int[] Buffers)
        {
            string output;
            if (IsArchived)
            {
                output = Sender + ConsoleInteraction.GetBuffer(Buffers[0] - Sender.Length) + "|" + Recipient + ConsoleInteraction.GetBuffer(Buffers[1] - Recipient.Length) + "|" + Subject + ConsoleInteraction.GetBuffer(Buffers[2] - Subject.Length) + "|" + DateRecived + "|" + "A";
                if (EmailTags != null) foreach (var tags in EmailTags) output += " " + tags.Value;


            }
            else
            {
                output = Sender + ConsoleInteraction.GetBuffer(Buffers[0] - Sender.Length) + "|" + Recipient + ConsoleInteraction.GetBuffer(Buffers[1] - Recipient.Length) + "|" + Subject + ConsoleInteraction.GetBuffer(Buffers[2] - Subject.Length) + "|" + DateRecived + "|";

            }
            return output;
        }
        public void DisplayEmail(LoadedEmails AllEmails)
        {
            Console.Clear();
            string Tags = "";
            if (this.EmailTags != null)
            {
                foreach (var tag in this.EmailTags)
                {
                    Tags += tag.Value + " ";
                }
            }
            string keywords = "";
            for (int i = 0; i < this.Keywords.Count(); i++) { keywords += this.Keywords[i] + " "; }
          
            bool Exit = false;
            int menuOption = 0;
            string input;
            while (!Exit)
            {
                if (menuOption == 0)
                {
                    Console.WriteLine("> Back |   Actions");
                }
                else
                {
                    Console.WriteLine("  Back | > Actions");
                }
                Console.WriteLine($"From: {Sender} To: {Recipient}\n" +
                                  $"Subject: {Subject} Tags: {Tags} KeyWords: {keywords}\n\n" +
                                  $"{Body}");
                input = ConsoleInteraction.GetConsoleInput();
                ConsoleInteraction.ResetCursor(); 
                if (input.ToLower() == "w" || input.ToLower() == "a")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = 1;
                    }
                }
                else if (input == "s" || input == "d")
                {
                    menuOption++;
                    if (menuOption > 1)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == 0) Exit = true;
                    else
                    {
                        Exit = EmailActions(AllEmails);
                    }
                }
            }
        }
        private bool EmailActions(LoadedEmails AllEmails)
        {
            CheckArchived();
            Console.Clear();
            string[] MenuOptions = { "Back", "Archive", "UnArchive", "Manage Tags" };
            bool exit = false;
            int menuOption = 0;
            string input;
            while (!exit)
            {
                for (int i = 0; i < MenuOptions.Length; i++)
                {
                    if (menuOption == i) Console.Write(" > ");
                    else Console.Write("   ");
                    Console.WriteLine(MenuOptions[i]);
                }
                input = ConsoleInteraction.GetConsoleInput();
                ConsoleInteraction.ResetCursor();
                if (input.ToLower() == "w")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = MenuOptions.Length - 1;
                    }
                }
                else if (input == "s")
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
                        case "Archive":
                            ArchiveEmail();
                            break;
                        case "UnArchive":
                            UnArchiveEmail();
                            break;
                        case "Manage Tags":
                            if (!IsArchived) Console.WriteLine("Email needs to be archived before tags can be managed");
                            else TagManagement(AllEmails);
                            Console.Clear();
                            break;
                    }
                }
            }


            return false;
        }
        private void UnArchiveEmail()
        {
            if (IsArchived)
            {
                IsArchived = false;
                SQLDataBase.ExecuteNonQuery($"DELETE FROM Emails WHERE EmailID == {EmailID}");
                SQLDataBase.ExecuteNonQuery($"DELETE FROM AssignedTags WHERE EmailID == {EmailID}");
            }
        }
        private void TagManagement(LoadedEmails AllEmails)
        {

            Console.Clear();
            bool exit = false;
            List<string[]> AllTagIDS;
            int menuOption = 0;
            string input;
            string[] MenuOptions = { "Add tag to email", "Remove tags from email", "Create tag", "Delete Tag", "Back" };
            while (!exit)
            {
                ConsoleInteraction.ResetCursor();
                Console.WriteLine("Current Tags:");
                if (EmailTags == null || EmailTags.Count == 0) Console.WriteLine("No tags to display");
                else
                {
                    foreach (var t in EmailTags)
                    {
                        Console.WriteLine($"{t.Value}");
                    }
                }



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
                        case "Add tag to email":
                            AddTag();
                            break;
                        case "Remove tags from email":
                            RemoveTag();
                            break;
                        case "Create tag":
                            Console.Write("Please Enter name for tag: ");
                            Tags.CreateNewTag(Console.ReadLine());
                            Console.Clear();
                            break;
                        case "Delete Tag":
                            DeleteTag(AllEmails);
                            break;
                    }
                }
            }


        }
        private void DeleteTag(LoadedEmails AllEmails)
        {
            Tags.DeleteTagMenu(AllEmails);
        }
        public void RemoveTagFromEmail(int tagId)
        {
            EmailTags.Remove(tagId);
        }
        private void AddTag()
        {
            Console.Clear();
            Dictionary<int, string> AllTags = Tags.GetAllTags();
            bool exit = false;
            int menuOption = 0;
            string input;
            int count = 0;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Select A tag");
                count = 0;
                foreach (var t in AllTags)
                {
                    if (!EmailTags.ContainsKey(t.Key))
                    {
                        if (count == menuOption) Console.Write(" > ");
                        else Console.Write("   ");
                        Console.WriteLine(t.Value);
                        count++;
                    }
                }
                if (menuOption == AllTags.Count - EmailTags.Count) Console.Write(" > ");
                else Console.Write("   ");
                Console.WriteLine("Back");
                input = ConsoleInteraction.GetConsoleInput();
                if (input.ToLower() == "w")
                {
                    menuOption--;
                    if (menuOption < 0)
                    {
                        menuOption = AllTags.Count - EmailTags.Count;
                    }
                }
                else if (input.ToLower() == "s")
                {
                    menuOption++;
                    if (menuOption == AllTags.Count - EmailTags.Count + 1)
                    {
                        menuOption = 0;
                    }
                }
                else if (input == "\r" || input == "")
                {
                    if (menuOption == AllTags.Count - EmailTags.Count) exit = true;
                    else
                    {
                        count = 0;
                        foreach (var i in AllTags)
                        {
                            if (!EmailTags.ContainsKey(i.Key))
                            {
                                if (count == menuOption) EmailTags.Add(i.Key, i.Value);
                                count++;
                            }
                        }
                    }
                }
            }
            UpdateTags();
            Console.Clear();
        }
        private void RemoveTag()
        {
            if (EmailTags == null || EmailTags.Count == 0) Console.WriteLine("No tags to remvoe");
            else
            {
                bool exit = false;
                int menuOption = 0;
                string input;
                int count = 0;
                while (!exit)
                {
                    Console.WriteLine("Current Tags:");

                    for (int i = 0; i < EmailTags.Count; i++)
                    {
                        if (i == menuOption) Console.Write(" > ");
                        else Console.Write("   ");
                        Console.WriteLine(EmailTags[i]);
                    }
                    if (menuOption == EmailTags.Count) Console.Write(" > ");
                    else Console.Write("   ");
                    Console.WriteLine("Back");
                    input = ConsoleInteraction.GetConsoleInput();
                    if (input.ToLower() == "w")
                    {
                        menuOption--;
                        if (menuOption < 0)
                        {
                            menuOption = EmailTags.Count;
                        }
                    }
                    else if (input.ToLower() == "s")
                    {
                        menuOption++;
                        if (menuOption == EmailTags.Count + 1)
                        {
                            menuOption = 0;
                        }
                    }
                    else if (input == "\r" || input == "")
                    {
                        if (menuOption == EmailTags.Count) exit = true;
                        else
                        {
                            count = 0;
                            foreach (var i in EmailTags)
                            {
                                if (count == menuOption) EmailTags.Remove(i.Key);
                                count++;
                            }
                        }
                    }
                }
                UpdateTags();
            }
            Console.Clear();
        }
        private void UpdateTags()
        {
            List<string[]> CurrentStoredTags = SQLDataBase.ExecuteQuery($"SELECT TagID FROM AssignedTags WHERE EmailID == {DataBaseID}");
            if (CurrentStoredTags != null)
            {
                foreach (string[] s in CurrentStoredTags)
                {
                    if (!EmailTags.ContainsKey(int.Parse(s[0]))) SQLDataBase.ExecuteNonQuery($"DELETE FROM AssignedTags WHERE TagID == {s[0]} AND EmailID == {DataBaseID}");
                }
            }
            foreach (var t in EmailTags)
            {
                if (!CheckIds(t.Key, CurrentStoredTags)) SQLDataBase.ExecuteNonQuery($"INSERT INTO AssignedTags(TagID,EmailID) VALUES ({t.Key},{DataBaseID})");
            }
        }
        private void CreateKeywords()
        {
            Keywords = new List<string>();
            if (Body != null)
            {
                List<List<POSTagging.word>> POSTagged
                = POSTagging.POStagging(Body);

                Graph graph = CreateGraph(POSTagged);

                foreach (Node node in graph.nodes)
                {
                    node.CaculateScore();
                }

                Node HighestScore1 = graph.nodes[0];
                Node HighestScore2 = graph.nodes[0];
                Node HighestScore3 = graph.nodes[0];

                for (int i = 0; i < graph.nodes.Count; i++)
                {
                    if (HighestScore1.GetScore() < graph.nodes[i].GetScore())
                    {
                        HighestScore3 = HighestScore2;
                        HighestScore2 = HighestScore1;
                        HighestScore1 = graph.nodes[i];
                    }
                    else if (HighestScore2.GetScore() < graph.nodes[i].GetScore())
                    {
                        HighestScore3 = HighestScore2;
                        HighestScore2 = graph.nodes[i];
                    }
                    else if (HighestScore3.GetScore() < graph.nodes[i].GetScore())
                    {

                        HighestScore3 = graph.nodes[i];
                    }
                }
                if (HighestScore1 != null) Keywords.Add(HighestScore1.GetWord());
                else Keywords.Add("");
                if (HighestScore2 != null)
                    Keywords.Add(HighestScore2.GetWord());
                else Keywords.Add("");
                if (HighestScore3 != null)
                    Keywords.Add(HighestScore3.GetWord());
                else Keywords.Add("");


                // automatic arciving
                

            }
        }
        private void AutomaticArcive(bool BypassAlgorithm)
        {
            if (BypassAlgorithm) ArchiveEmail();
            else
            {
                List<string[]> Tags = SQLDataBase.ExecuteQuery("SELECT * FROM Tags");
                List<int> TagIdsToadd = new List<int>();
                foreach (string[] tag in Tags)
                {
                    foreach (string s in Keywords)
                    {
                        if (s.ToLower() == tag[1].ToLower()) TagIdsToadd.Add(int.Parse(tag[0]));
                    }
                }
                if (TagIdsToadd.Count > 0)
                {
                    ArchiveEmail();
                    foreach (int i in TagIdsToadd)
                    {
                        if (!EmailTags.ContainsKey(i)) EmailTags.Add(i, SQLDataBase.ExecuteQuery($"SELECT TagName FROM Tags WHERE TagID == {i}")[0][0]);

                    }
                    UpdateTags();
                }
            }
        }
        private Graph CreateGraph(List<List<POSTagging.word>> input) 
        {
            List<POSTagging.word> words = new List<POSTagging.word>();
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[i].Count; j++)
                {
                    words.Add(input[i][j]);
                }
            }


            Graph graph = new Graph();
            bool InGraph = false;
            for (int i = 0; i < words.Count; i++)
            {
                InGraph = false;
                foreach (Node n in graph.nodes)
                {
                    if (words[i].value.ToLower() == n.GetWord().ToLower())
                    {
                        InGraph = true;
                    }
                }
                if (!InGraph)
                {
                    graph.AddNode(words[i]);
                }
                if (i != 0)
                {
                    if (!words[i - 1].value.ToLower().Contains('.'))
                    {
                        if (graph.GetNodeIndex(words[i - 1].value.ToLower()) == -1)
                        {
                            graph.nodes[graph.GetNodeIndex(words[i].value.ToLower())].AddEdge(new Node(words[i - 1]), 1);
                        }
                        else
                        {
                            if (graph.nodes[graph.GetNodeIndex(words[i].value.ToLower())].GetIndexOfEdge(words[i - 1].value.ToLower()) == -1)
                            {
                                graph.nodes[graph.GetNodeIndex(words[i].value.ToLower())].AddEdge(graph.nodes[graph.GetNodeIndex(words[i - 1].value.ToLower())], 1);
                            }
                            else
                            {
                                graph.nodes[graph.GetNodeIndex(words[i].value.ToLower())].IncreaseEdgeWeight(words[i - 1].value.ToLower(), 1);
                            }
                        }
                    }
                }
                if (i != words.Count - 1)
                {

                    if (graph.GetNodeIndex(words[i + 1].value.ToLower()) == -1)
                    {
                        graph.nodes[graph.GetNodeIndex(words[i].value.ToLower())].AddEdge(new Node(words[i + 1]), 1);
                    }
                    else
                    {
                        if (graph.nodes[graph.GetNodeIndex(words[i].value.ToLower())].GetIndexOfEdge(words[i + 1].value.ToLower()) == -1)
                        {
                            graph.nodes[graph.GetNodeIndex(words[i].value.ToLower())].AddEdge(graph.nodes[graph.GetNodeIndex(words[i + 1].value.ToLower())], 1);
                        }
                        else
                        {
                            graph.nodes[graph.GetNodeIndex(words[i].value.ToLower())].IncreaseEdgeWeight(words[i + 1].value.ToLower(), 1);
                        }
                    }

                }
            }
            return graph;
        }
        private void ArchiveEmail()
        {
            List<string[]> AllEmailIDsInDataBase = SQLDataBase.ExecuteQuery("SELECT EmailID FROM Emails");
            int IDToBeStored = EmailID;
            if (!IsArchived)
            {
                if (AllEmailIDsInDataBase == null) AddToDatabase(EmailID);
                else
                {
                    while (CheckIds(IDToBeStored, AllEmailIDsInDataBase))
                    {
                        SQLDataBase.ExecuteNonQuery("INSERT INTO Collisions(CollisionAt) " +
                                                $"VALUES " +
                                                $"({IDToBeStored.ToString()})");
                        IDToBeStored += 1;
                    }

                    AddToDatabase(IDToBeStored);
                }
                IsArchived = true;
            }
        }
        private bool CheckIds(int ID, List<string[]> AllIds)
        {
            if (AllIds == null) return false;
            for (int i = 0; i < AllIds.Count; i++)
            {
                if (int.Parse(AllIds[i][0]) == ID)
                {
                    return true;
                }
            }
            return false;
        }
        private void AddToDatabase(int ID)
        {
            SQLDataBase.ExecuteNonQuery($"INSERT INTO Emails(EmailId,Sender,Recipient,Subject,TextBody,KeyWords,DateRecived)" +
                                     $"VALUES " +
                                     $"({ID},'{Sender}','{Recipient}','{Subject}','{Body}','{CombineKeywords()}','{DateRecived}')");
            SQLDataBase.ExecuteNonQuery($"DELETE FROM Collisions " +
                                     $"WHERE CollisionAt == {ID.ToString()}");
            DataBaseID = ID;
        }
        private string CombineTags()
        {
            string output = "";
            foreach (var s in EmailTags)
            {
                output += s.Value;
            }
            return output;
        }
        private string CombineKeywords()
        {
            string output = "";
            if (Keywords != null)
            {
                foreach (string s in Keywords)
                {
                    output += s + " ";
                }
            }

            return output;
        }
    }
}
