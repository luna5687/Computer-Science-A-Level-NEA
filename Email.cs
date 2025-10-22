using NEA_protoype;
using Org.BouncyCastle.Crypto;
using System.Data.Entity;
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
        private Dictionary<string,int> Tags;
        private int EmailID;
        public Email(string Sender, string Recipient, string Subject, string Body)
        {
            this.Sender = Sender;
            this.Recipient = Recipient;
            this.Subject = Subject;
            this.Body = Body;
            EmailID = Sender.Length + Recipient.Length + Subject.Length + Body.Length; // need to check if it is in database 
            CreateKeywords();
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
        public string GetEmailShort(int[] Buffers)
        {
            string output;
            if (IsArchived)
            {
                output = Sender + ConsoleInteraction.GetBuffer(Buffers[0] - Sender.Length) + "|" + Recipient + ConsoleInteraction.GetBuffer(Buffers[1] - Recipient.Length) + "|" + Subject + ConsoleInteraction.GetBuffer(Buffers[2] - Subject.Length) + "|" + "A";
                foreach (var tags in Tags)
                {
                    output += " " + Tags.Keys;
                }
                
            }
            else
            {
                output = Sender + ConsoleInteraction.GetBuffer(Buffers[0] - Sender.Length) + "|" + Recipient + ConsoleInteraction.GetBuffer(Buffers[1] - Recipient.Length) + "|" + Subject + ConsoleInteraction.GetBuffer(Buffers[2] - Subject.Length) + " ";

            }
            return output;
        }
        public void DisplayEmail(SQLDataBase dataBase)
        {
            Console.Clear();
            string Tags = "";
            if (this.Tags != null)
            {
                foreach (var tag in this.Tags)
                {
                    Tags += this.Tags.Keys + " ";
                }
            }
            string keywords = "";
            //for (int i = 0; i < this.Keywords.Count(); i++) { keywords += this.Keywords[i] + " "; }
            Console.Clear();
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
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
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
                        Exit = EmailActions(dataBase);
                    }
                }
             }
         }
        private bool EmailActions(SQLDataBase dataBase)
        {
            string[] MenuOptions = { "Archive" };
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
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
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
                switch(MenuOptions[menuOption])
                {
                    case "Archive":
                        ArchiveEmail(dataBase);
                        break;
                }
            }


            return false;
        }
        
        
        private void CreateKeywords()
        {
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
            }
        }
        private Graph CreateGraph(List<List<POSTagging.word>> input) // needs implementing with POStagging word stucture
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
        private void ArchiveEmail(SQLDataBase dataBase)
        {
            List<string[]> AllEmailIDsInDataBase = dataBase.ExecuteQuery("SELECT EmailID FROM Emails");
            int IDToBeStored = EmailID;
            if (!IsArchived)
            {
                if (AllEmailIDsInDataBase.Count == 0)
                {
                    AddToDatabase(EmailID, dataBase);
                }
                else
                {
                    while (CheckIds(IDToBeStored, AllEmailIDsInDataBase))
                    {
                        dataBase.ExecuteNonQuery("INSERT INTO Collisions(CollisionAt)" +
                                                $"VALUES " +
                                                $"({IDToBeStored})");
                        IDToBeStored += 1;
                    }
                    IsArchived = true;
                    AddToDatabase(IDToBeStored, dataBase);
                }
            }
        }
        private bool CheckIds(int ID, List<string[]> AllIds)
        {
            for (int i = 0; i < AllIds.Count; i++)
            {
                if (int.Parse(AllIds[i][0]) == ID)
                {
                    return true;
                }
            }
            return false;
        }
        private void AddToDatabase(int ID,SQLDataBase dataBase)
        {
            dataBase.ExecuteNonQuery($"INSERT INTO Emails(EmailId,Sender,Recipient,Subject,TextBody,KeyWords)" +
                                     $"VALUES " +
                                     $"({ID},'{Sender}','{Recipient}','{Subject}','{Body}','{CombineKeywords}')");
            dataBase.ExecuteNonQuery($"DELETE FROM Collisions" +
                                     $"WHERE CollisionAt == {ID}");
            if (Tags.Count>0)
            {
                // add tags
            }
        }
        private string CombineTags()
        {
            string output = "";
            foreach(var s in Tags)
            {
                output += s.Key;
            }
            return output;
        }
        private string CombineKeywords()
        {
            string output = "";
            foreach (string s in Keywords)
            {
                output += s+" ";
            }
            return output;
        }
    }
}
