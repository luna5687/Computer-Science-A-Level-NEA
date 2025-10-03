// Copyright 2025 Daniel Ian White

// Bobs email: bob@ampretia.co.uk password: passw0rdWibble Mailserver: mail.ampretia.co.uk

using Computer_Science_A_Level_NEA;

namespace NEA_protoype
{
    public class Node
    {
        
        private List<Node> edges;
        private List<int> edgeWeights;
        private double Score = 0;
        
        private POSTagging.word word;
        public Node(POSTagging.word word)
        {
            this.word = word;
            edges = new List<Node>();
            edgeWeights = new List<int>();
        }
        public void AddEdge(Node ConnectedNode, int edgeweight)
        {
            edges.Add(ConnectedNode);
            edgeWeights.Add(edgeweight);
        }
        public void IncreaseEdgeWeight(string edge, int increaseBY)
        {
            edgeWeights[GetIndexOfEdge(edge)] += increaseBY;
        }
        public string GetWord()
        {
            return word.value;
        }
        public int GetIndexOfEdge(string edge)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].GetWord() == edge)
                {
                    return i;
                }
            }
            return -1;
        }
        public int GetEdgeAmount()
        {
            return edges.Count;
        }
        public string GetEdge(int index)
        {
            return edges[index].GetWord();
        }
        public int GetEdgeWeight(int index)
        {
            return edgeWeights[index];
        }
        public void CaculateScore()
        {
            double total = 0;
            for (int i = 0; i < this.GetEdgeAmount(); i++)
            {

                total += this.GetEdgeWeight(i);
            }
            total = total * ((double)this.GetEdgeAmount());
            if (word.wordType != "N")
            {
                total = 0;
            }
            Score = total;
        }
        public int GetScore()
        {
            return (int)Score;
        }
        public string DecideWordType(string word)
        {
            return "";
        }
    }
    public class Graph
    {
        public List<Node> nodes;
        public Graph()
        {
            nodes = new List<Node>();
        }
        public void AddNode(POSTagging.word word)
        {
            nodes.Add(new Node(word));
        }
        public int GetNodeIndex(string ToFind)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (ToFind == nodes[i].GetWord())
                {
                    return i;
                }
            }
            return -1;
        }
    }
}