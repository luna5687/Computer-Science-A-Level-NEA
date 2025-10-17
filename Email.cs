using NEA_protoype;
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
        private List<string> Tags;
        private int EmailID;
        public Email(string Sender, string Recipient, string Subject, string Body)
        {
            this.Sender = Sender;
            this.Recipient = Recipient;
            this.Subject = Subject;
            this.Body = Body;

            CreateKeywords();
        }
        public string GetEmailShort()
        {
            string output;
            if (IsArchived)
            {
                output = Sender + " " + Recipient + " " + Subject + " " + "A";
                for (int i = 0; i < Tags.Count; i++)
                {
                    output += " " + Tags[i];
                }
            }
            else
            {
                output = Sender + " " + Recipient + " " + Subject + " " + " ";

            }
            return output;
        }
        public void DisplayEmail()
        {
            Console.Clear();
            string Tags = "";
            if (this.Tags != null)
            {
                for (int i = 0; i < this.Tags.Count(); i++)
                {
                    Tags += this.Tags[i] + " ";
                }
            }
            string keywords = "";
            //for (int i = 0; i < this.Keywords.Count(); i++) { keywords += this.Keywords[i] + " "; }
            Console.Clear();

            Console.WriteLine("> Back");
            Console.WriteLine($"From: {Sender} To: {Recipient}\n" +
                              $"Subject: {Subject} Tags: {Tags} KeyWords: {keywords}\n\n" +
                              $"{Body}");
            ConsoleInteraction.GetConsoleInput();
            Console.Clear();
        }
        private void CreateKeywords()
        {
            List<List<POSTagging.word>> POSTagged
            = POSTagging.POStagging(Body);
            //ConsoleInteraction.GetConsoleInput();
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
        static Graph CreateGraph(List<List<POSTagging.word>> input) // needs implementing with POStagging word stucture
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
    }
}
