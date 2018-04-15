using System;
using System.Collections.Generic;
using CW_Nelya_Vika.Models;

namespace CW_Nelya_Vika_Algorithms
{
    class GirvanNewman: IAlgorithm
    {
        Dictionary<Edge, double> edgeBetweenness;
        Result result;
        Graph graph;
        private double Q;

        public double BestQ { get; set; }
        
        public string Log { get; set; }

        private void WriteLog(string log = "")
        {
            Log += log + "\r\n";
        }

        public Result FindCommunityStructure(Graph pGraph)
        {
            // Clone graph
            graph = pGraph.Clone();

            
            Result tempCS = GetCommunityStructure();

            int initCount = tempCS.Count;
            int countCommunity = initCount;

            // Q
            BestQ = 0;
            Q = 0;

            CalculateEdgeBetweenness(tempCS, graph);

            int j = 0;
            while (true)
            {
                while (countCommunity <= initCount)
                {
                    WriteLog("Xóa lần " + j.ToString()); j++;
                    Graph community = RemoveMaxEdgeBetweenness(tempCS); 
                    CalculateEdgeBetweenness(tempCS, community);

                    tempCS = GetCommunityStructure();
                    countCommunity = tempCS.Count;
                }

                initCount = countCommunity;

                Q = CalculateModularity(tempCS, pGraph);
                if (Q > BestQ)
                {
                    BestQ = Q;
                    result = tempCS;
                }

                if (graph.Edges.Count == 0) break;
            }

            return this.result;
        }

        public static double CalculateModularity(Result pCs, Graph pOriginalGraph)
        {
            double modularity = 0;
            int numEdge = pOriginalGraph.Edges.Count;
            foreach (Graph csItem in pCs)
            {
                int l = 0; 
                int d = 0; 
                foreach (Node node in csItem.Nodes)
                {
                    l += node.AdjacencyNodes.Count;
                    d += pOriginalGraph.FindNode(node.Id, false).AdjacencyNodes.Count;
                }

                l /= 2;

                modularity += ((double)l / numEdge) - Math.Pow(((double)d / (2 * numEdge)), 2);
            }
            return modularity;
        }

        private Graph RemoveMaxEdgeBetweenness(Result pTempCS)
        {
            double max = double.MinValue;
            Edge e = null;
            foreach (KeyValuePair<Edge, double> keypair in edgeBetweenness)
            {
                if (keypair.Value > max)
                {
                    max = keypair.Value;
                    e = keypair.Key;
                }
            }

            graph.Edges.Remove(e);

            e.NodeOut.AdjacencyEdges.Remove(e);
            e.NodeIn.AdjacencyEdges.Remove(e);

            e.NodeOut.AdjacencyNodes.Remove(e.NodeIn);
            e.NodeIn.AdjacencyNodes.Remove(e.NodeOut);

            WriteLog(" - Remove: (" + e.NodeOut.Id + ", " + e.NodeIn.Id + ")\t" + edgeBetweenness[e].ToString("0.00"));

            edgeBetweenness.Remove(e);

            foreach (Graph subgraph in pTempCS)
            {
                if (subgraph.Nodes.Contains(e.NodeOut))
                    return subgraph;
            }
            return null;
        }

        private void CalculateEdgeBetweenness(Result pCS, Graph community = null)
        {
            if (edgeBetweenness == null)
            {
                edgeBetweenness = new Dictionary<Edge, double>();
                foreach (Edge e in graph.Edges)
                {
                    edgeBetweenness.Add(e, 0);
                }
            }

            _CalculateEdgeBetweenness(community);
        }

        private void _CalculateEdgeBetweenness(Graph subgraph)
        {
            if (subgraph == null) return;
            int n = subgraph.Nodes.Count;
            int MAX = 99999;

            foreach (Node s in subgraph.Nodes)
            {
                foreach (Edge e in s.AdjacencyEdges)
                {
                    edgeBetweenness[e] = 0;
                }
            }

            foreach (Node s in subgraph.Nodes)
            {
                Queue<Node> Q = new Queue<Node>();
                Stack<Node> S = new Stack<Node>();
                Dictionary<Node, List<Node>> pred = new Dictionary<Node, List<Node>>();

                Dictionary<Node, int> dist = new Dictionary<Node, int>();
                Dictionary<Node, int> sigma = new Dictionary<Node, int>();
                Dictionary<Node, double> delta = new Dictionary<Node, double>();

                // initialization
                foreach (Node d in subgraph.Nodes)
                {
                    dist.Add(d, MAX);
                    sigma.Add(d, 0);
                    delta.Add(d, 0);
                    pred.Add(d, new List<Node>());
                }

                dist[s] = 0;
                sigma[s] = 1;
                Q.Enqueue(s);

                // while
                while (Q.Count != 0)
                {
                    Node v = Q.Dequeue();
                    S.Push(v);

                    foreach (Node w in v.AdjacencyNodes)
                    {
                        if (dist[w] == MAX)
                        {
                            dist[w] = dist[v] + 1;
                            Q.Enqueue(w);
                        }
                        if (dist[w] == dist[v] + 1)
                        {
                            sigma[w] = sigma[w] + sigma[v];
                            pred[w].Add(v);
                        }
                    }
                }

                // accumuation
                while (S.Count != 0)
                {
                    Node w = S.Pop();
                    foreach (Node v in pred[w])
                    {
                        double c = ((double)(sigma[v]) / sigma[w]) * (1.0 + delta[w]);

                        Edge e = graph.FindEdge(v, w);
                        edgeBetweenness[e] += c;

                        delta[v] += c;
                    }
                }
            }
        }

        private Result GetCommunityStructure()
        {
            Result cs = new Result();

            int count = 0;
            int n = graph.Nodes.Count;
            Dictionary<Node, bool> visited = new Dictionary<Node, bool>();
            for (int i = 0; i < n; i++)
            {
                visited.Add(graph.Nodes[i], false);
            }

            foreach (Node i in graph.Nodes)
            {
                if (visited[i] == false)
                {
                    count++;
                    Graph subgraph = new Graph();

                    Queue<Node> Q = new Queue<Node>();
                    visited[i] = true;
                    Q.Enqueue(i);

                    subgraph.Nodes.Add(i);

                    while (Q.Count != 0)
                    {
                        Node v = Q.Dequeue();
                        foreach (Node j in v.AdjacencyNodes)
                        {
                            if (visited[j] == false)
                            {
                                subgraph.Nodes.Add(j);
                                visited[j] = true;
                                Q.Enqueue(j);
                            }
                        }
                    }
                    cs.Add(subgraph);
                }
            }
            return cs;
        }
    }
}
