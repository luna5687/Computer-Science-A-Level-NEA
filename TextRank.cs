using MailKit;
using System.Text.RegularExpressions;

namespace Computer_Science_A_Level_NEA
{
    public class Edge
    {
        private int IndexNode1, IndexNode2, weight, index;
        public Edge(int indexNode1, int indexNode2, int weight)
        {
            IndexNode1 = indexNode1;
            IndexNode2 = indexNode2;
            this.weight = weight;
        }
        public void UpdateWeight(int weight) => this.weight = weight;
        public void SetIndex(int index) => this.index = index;
        public int GetIndexOfNode1() { return IndexNode1; }
        public int GetIndexOfNode2() { return IndexNode2; }

    }

    public class Node
    {
        private string Data;
        private double Score;
        private List<int> IndexOfEdges = new List<int>();
        public Node(string input) { Data = input; Score = 0; }
        public string GetData() => Data;
        public void AddEdgeIndex(int index) => IndexOfEdges.Add(index);
        public void SetScore(double input) => Score = input;
        public double GetScore() => Score;

    }
    public class Graph
    {
        List<Node> Nodes = new List<Node>();
        List<Edge> Edges = new List<Edge>();
        List<double> PreviousWeights = new List<double>();
        public int GetNodeIndex(string input)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].GetData() == input) return i;
            }
            return -1;
        }
        public void AddNode(string input)
        {
            if (!ContainsNode(input))
                Nodes.Add(new Node(input));

        }
        public bool ContainsNode(params string[] input) // can take in multiple nodes to check more than one at once, returns true if all exist, returns false of one or more does not exist
        {
            bool[] Founds = new bool[input.Length];
            for (int i = 0; i < Founds.Length; i++)
            { Founds[i] = false; }


            for (int i = 0; i < Founds.Length; i++)
            {
                foreach (Node node in Nodes)
                {
                    if (node.GetData() == input[i]) Founds[i] = true;
                }

            }

            foreach (bool b in Founds)
                if (!b) return false;
            return true;
        }
        public bool CheckEdgeExistance(string Node1, string Node2)
        {
            foreach (Edge e in Edges)
            {
                if (e.GetIndexOfNode1() == GetNodeIndex(Node1) && e.GetIndexOfNode2() == GetNodeIndex(Node2)) return true;
                if (e.GetIndexOfNode1() == GetNodeIndex(Node2) && e.GetIndexOfNode2() == GetNodeIndex(Node1)) return true;
            }
            return false;
        }
        public void Addedge(string NodeToConnectFrom, string NodeToConnectTo)
        {
            if (!ContainsNode(NodeToConnectFrom, NodeToConnectTo)) throw new NodeDoesNotExitstExeption();
            if (!CheckEdgeExistance(NodeToConnectTo, NodeToConnectFrom))
            {
                Edge NewEdge = new Edge(GetNodeIndex(NodeToConnectTo), GetNodeIndex(NodeToConnectFrom), 1);
                Edges.Add(NewEdge);
                Edges[Edges.Count - 1].SetIndex(Edges.Count - 1);
                Nodes[GetNodeIndex(NodeToConnectTo)].AddEdgeIndex(Edges.Count - 1);
                Nodes[GetNodeIndex(NodeToConnectFrom)].AddEdgeIndex(Edges.Count - 1);
                //RecaculateAllNodes();
            }

        }
        public void RecaculateAllNodes()
        {
            PreviousWeights = new List<double>();
            foreach (Node node in Nodes)
            {
                PreviousWeights.Add(node.GetScore());
                CaculateScoreForNode(node.GetData());
            }
        }
        public void CaculateScoreForNode(string node)
        {
            double DAMPINGCOEFFICIENT = 0.1;
            double OutPutScore = (1 - DAMPINGCOEFFICIENT) / Nodes.Count;
            OutPutScore += DAMPINGCOEFFICIENT * SumEdgeWeightsTimeScores(node);

            Nodes[GetNodeIndex(node)].SetScore(OutPutScore);
        }
        public string[] GetThreeBest()
        {
            int[] bestIndex = new int[3];
            string[] threeBest = new string[3];
            bestIndex[0] = 0;
            bestIndex[1] = 1;
            bestIndex[2] = 2;

            while (bestIndex[0] < Nodes.Count && IsStopWord(Nodes[bestIndex[0]].GetData()))
            {
                bestIndex[0] += 1;
                bestIndex[1] += 1;
                bestIndex[2] += 1;
            }
            while (bestIndex[1] < Nodes.Count && IsStopWord(Nodes[bestIndex[1]].GetData()))
            {
                bestIndex[1] += 1;
                bestIndex[2] += 1;
            }
            while (bestIndex[2] < Nodes.Count && IsStopWord(Nodes[bestIndex[2]].GetData()))
            {
                bestIndex[2] += 1;
            }

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[bestIndex[0]].GetScore() < Nodes[i].GetScore() && !IsStopWord(Nodes[i].GetData())) bestIndex[0] = i;
            }
            if (Nodes.Count >= 2)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (Nodes[bestIndex[1]].GetScore() < Nodes[i].GetScore() && !IsStopWord(Nodes[i].GetData()) && i != bestIndex[0]) bestIndex[1] = i;
                }
            }
            if (Nodes.Count >= 3)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (Nodes[bestIndex[2]].GetScore() < Nodes[i].GetScore() && !IsStopWord(Nodes[i].GetData()) && i != bestIndex[0] && i != bestIndex[1]) bestIndex[2] = i;
                }
            }
            threeBest[0] = Nodes[bestIndex[0]].GetData();
            if (Nodes.Count >= 2) threeBest[1] = Nodes[bestIndex[1]].GetData();
            if (Nodes.Count >= 3) threeBest[2] = Nodes[bestIndex[2]].GetData();

            return threeBest;
        }
        private bool IsStopWord(string word)
        {
            string[] StopWords = { "the", "and", "a", "an" };
            foreach (string s in StopWords)
            {
                if (s == word) return true;
            }
            return false;
        }
        private double SumEdgeWeightsTimeScores(string StartNode)
        {
            double output = 0;
            List<int> IndexsOfConnectedNode = new List<int>();
            foreach (Edge Edge in Edges)
            {
                if (Edge.GetIndexOfNode1() == GetNodeIndex(StartNode))
                {
                    IndexsOfConnectedNode.Add(Edge.GetIndexOfNode2());
                }
                if (Edge.GetIndexOfNode2() == GetNodeIndex(StartNode))
                {
                    IndexsOfConnectedNode.Add(Edge.GetIndexOfNode1());
                }
            }
            foreach (int i in IndexsOfConnectedNode)
            {
                output += Nodes[i].GetScore();
            }

            return output;
        }
        public bool CheckConvergance()
        {
            if (PreviousWeights.Count == 0) return false;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (PreviousWeights[i] != Nodes[i].GetScore()) return false;
            }

            return true;
        }
    }

    static public class TextRank
    {

        static public string KeywordExtraction(string input)
        {
            if (input == null) return "";
            
                
            
            string[] Sentenses = input.Split('.', '\n', '!', '?');
            List<string> AllWords = new List<string>();
            foreach (string s in Sentenses)
            {
                foreach (string word in Tokenization(s))
                {
                    AllWords.Add(word);
                }
            }
            Graph graph = new Graph();
            for (int i = 0; i < AllWords.Count; i++)
            {
                if (AllWords[i] != ".")
                {
                    graph.AddNode(AllWords[i]);
                    if (i != 0 && AllWords[i - 1] != ".")
                    {
                        graph.Addedge(AllWords[i], AllWords[i - 1]);
                    }
                }
            }

            while (!graph.CheckConvergance())
            {
                graph.RecaculateAllNodes();
            }

            string[] best = graph.GetThreeBest();
            string output = "";
            foreach (string s in best)
            {
                output += s + ",";
            }
            return output.Substring(0, output.Length - 1);
        }
        static List<string> Tokenization(string input)
        {
            string FormatedString = input.Replace("\n", " ");
            FormatedString = input.Replace("\r", " ");
            FormatedString = input.Replace(",", " ");
            FormatedString = input.Replace("(", " ");
            FormatedString = input.Replace(")", " ");
            List<string> output = new List<string>();
            string ToAdd;
            foreach (string s in FormatedString.Split(' '))
            {

                if (s != "" || !Regex.IsMatch(s, "^\\s*$"))
                {
                    ToAdd = s;
                    ToAdd = ToAdd.ToLower();
                    if (Regex.IsMatch(s, "$(ed)|(er)")) ToAdd = ToAdd.Substring(0, ToAdd.Length - 2);
                    output.Add(ToAdd);
                }
            }
            output.Add(".");
            return output;
        }
        static bool CheckWeightChangeSignificance()
        {
            return false;
        }
    }
}
