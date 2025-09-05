using Computer_Science_A_Level_NEA;
using MimeKit;
using System.Data.SQLite;
// Copyright 2025 Daniel Ian White

// Bobs email: bob@ampretia.co.uk password: passw0rdWibble Mailserver: mail.ampretia.co.uk

namespace NEA_protoype
{
    public class Graph
    {
        private string word;
        private List<Graph> edges;
        private List<int> edgeWeights;

        public Graph(string word)
        {
            this.word = word;
        }
        public void AddEdge(Graph ConnectedNode, int edgeweight)
        {
            edges.Add(ConnectedNode);
            edgeWeights.Add(edgeweight);
        }

    }
}