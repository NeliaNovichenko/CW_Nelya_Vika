using System;
using System.Collections.Generic;
using System.Linq;
using CW_Nelya_Vika.Models;

namespace CW_Nelya_Vika_Algorithms
{
    class GirvanNewman : IAlgorithm
    {
        /// <summary>
        /// Кількість найкоротших шляхів між усіма вершинами, що проходять через дане ребро
        /// </summary>
        Dictionary<Edge, double> edgeBetweenness;
        /// <summary>
        /// List<Graph> result - результат розбиття
        /// </summary>
        Result result;
        /// <summary>
        /// Граф, для якого проводиться розбитта
        /// </summary>
        Graph graph;
        //TODO:
        private double Q;

        public double BestQ { get; set; }

        public string Log { get; private set; }

        /// <summary>
        /// Вхідна функція виконання алгоритму
        /// </summary>
        /// <param name="pGraph"></param>
        /// <returns></returns>
        public Result FindCommunityStructure(Graph pGraph)
        {
            // Clone graph
            if (pGraph is null)
                return null;
            graph = pGraph.Clone();

            //Тимчасове розбиття
            Result tempCs = GetCommunityStructure();

            int initCount = tempCs.Count;
            int countCommunity = initCount;



            //обчислюємо Betweenness
            InitializeEdgeBetweenness(graph);

            int j = 0;
            while (true)
            {
                while (countCommunity <= initCount)
                {
                    Graph community = RemoveMaxEdgeBetweenness(tempCs);
                    InitializeEdgeBetweenness(community);

                    tempCs = GetCommunityStructure();
                    countCommunity = tempCs.Count;
                }

                initCount = countCommunity;

                Q = 0;
                if (graph.CommunityCount != 0)
                {
                    BestQ = graph.CommunityCount;
                }
                else
                {

                    BestQ = 0;
                    Q = CalculateModularity(tempCs, pGraph);

                }
                if (Q > BestQ)
                {
                    BestQ = Q;
                    result = tempCs;
                }


                if (graph.Edges.Count == 0)
                    break;
            }

            return this.result;
        }

        /// <summary>
        /// Додати запис у лог виконання алгоритму
        /// </summary>
        /// <param name="log"></param>
        private void WriteLog(string log = "")
        {
            Log += log + "\r\n";
        }

        /// <summary>
        /// Modularity - оцінка якості розбиття графа на підграфи,
        /// наскільки дане розбиття якісно
        /// </summary>
        /// <param name="result"></param>
        /// <param name="pOriginalGraph"></param>
        /// <returns></returns>
        public static double CalculateModularity(Result result, Graph pOriginalGraph)
        {
            double modularity = 0;
            int numEdge = pOriginalGraph.Edges.Count;
            foreach (Graph subGraph in result)
            {
                int l = 0;
                int d = 0;
                foreach (Vertex node in subGraph.Vertexes)
                {
                    l += node.AdjacencyVertexes.Count;
                    d += pOriginalGraph.FindNode(node.Id, false).AdjacencyVertexes.Count;
                }

                l /= 2;

                modularity += ((double)l / numEdge) - Math.Pow(((double)d / (2 * numEdge)), 2);
            }
            return modularity;
        }

        /// <summary>
        /// Видалити ребро з найбільшим показником Betweenness
        /// </summary>
        /// <param name="pTempCS"></param>
        /// <returns></returns>
        private Graph RemoveMaxEdgeBetweenness(Result pTempCS)
        {
            double max = edgeBetweenness.Max(n => n.Value);
            Edge e = edgeBetweenness.Where(n => Math.Abs(n.Value - max) < 1e-6).ToArray()[0].Key;

            graph.Edges.Remove(e);

            e.VertexOut.AdjacencyEdges.Remove(e);
            e.VertexIn.AdjacencyEdges.Remove(e);

            e.VertexOut.AdjacencyVertexes.Remove(e.VertexIn);
            e.VertexIn.AdjacencyVertexes.Remove(e.VertexOut);

            WriteLog(" - Remove: (" + e.VertexOut.Id + ", " + e.VertexIn.Id + ")");

            edgeBetweenness.Remove(e);

            foreach (Graph subgraph in pTempCS)
            {
                if (subgraph.Vertexes.Contains(e.VertexOut))
                    return subgraph;
            }
            return null;
        }

        /// <summary>
        /// Ініціалізуємо edgeBetweenness та 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="community"></param>
        private void InitializeEdgeBetweenness(Graph community = null)
        {
            if (edgeBetweenness == null)
            {
                edgeBetweenness = new Dictionary<Edge, double>();
                //Ініціалізуємо edgeBetweenness: для всіх ребер спочатку значення = 0
                foreach (Edge e in graph.Edges)
                {
                    edgeBetweenness.Add(e, 0);
                }
            }

            CalculateEdgeBetweenness(community);
        }

        /// <summary>
        /// Розраховує Betweenness для кожного ребра в графі
        /// </summary>
        /// <param name="subgraph"></param>
        private void CalculateEdgeBetweenness(Graph subgraph)
        {
            #region Old
            //if (subgraph == null)
            //    return;
            //int nodesCount = subgraph.Vertexes.Count;
            //int MAX = Int32.MaxValue;

            //foreach (Vertex vertex in subgraph.Vertexes)
            //{
            //    foreach (Edge e in vertex.AdjacencyEdges)
            //    {
            //        edgeBetweenness[e] = 0;
            //    }
            //}

            //foreach (Vertex vertex in subgraph.Vertexes)
            //{
            //    Queue<Vertex> nodeQueue = new Queue<Vertex>();
            //    Stack<Vertex> nodeStack = new Stack<Vertex>();
            //    //TODO: до поточної вершини можна дійти через список вершин
            //    Dictionary<Vertex, List<Vertex>> pred = new Dictionary<Vertex, List<Vertex>>();
            //    //Відстань від поточної до кожної
            //    Dictionary<Vertex, int> dist = new Dictionary<Vertex, int>();
            //    //
            //    Dictionary<Vertex, int> sigma = new Dictionary<Vertex, int>();
            //    //
            //    //Dictionary<Vertex, double> delta = new Dictionary<Vertex, double>();

            //    // initialization
            //    foreach (Vertex _node in subgraph.Vertexes)
            //    {
            //        dist.Add(_node, MAX); 
            //        sigma.Add(_node, 0);
            //        //delta.Add(_node, 0);
            //        pred.Add(_node, new List<Vertex>());
            //    }

            //    dist[vertex] = 0; //відстань самої до себе = 0
            //    sigma[vertex] = 1;
            //    nodeQueue.Enqueue(vertex);

            //   while (nodeQueue.Count != 0)
            //    {
            //        Vertex currentVertex = nodeQueue.Dequeue();
            //        nodeStack.Push(currentVertex);

            //        foreach (Vertex adjacencyNode in currentVertex.AdjacencyVertexes)
            //        {
            //            if (dist[adjacencyNode] == MAX)
            //            {
            //                dist[adjacencyNode] = dist[currentVertex] + 1; //відстань = як до поточної + 1
            //                nodeQueue.Enqueue(adjacencyNode);
            //            }
            //            if (dist[adjacencyNode] == dist[currentVertex] + 1)
            //            {
            //                sigma[adjacencyNode] += sigma[currentVertex];
            //                pred[adjacencyNode].Add(currentVertex); //до прилеглої вершини можна дійти через поточну 
            //            }
            //        }
            //    }

            //    // накопичення
            //    while (nodeStack.Count != 0)
            //    {
            //        Vertex nodeFromStack = nodeStack.Pop();
            //        foreach (Vertex n in pred[nodeFromStack]) //для всіх вершин, через які можно дійти до поточної
            //        {
            //            double c = ((double)(sigma[n]) / sigma[nodeFromStack]) /** (1.0 + delta[nodeFromStack])*/;

            //            Edge e = graph.FindEdge(n, nodeFromStack);
            //            edgeBetweenness[e] += c;
            //            //delta[n] += c;
            //        }
            //    }
            //}

            ////OLD2
            //int[,] matrix = subgraph.GraphToMatrix();
            //foreach (var node1 in subgraph.Vertexes)
            //foreach (var node2 in subgraph.Vertexes)
            //{
            //    Edge currEdge = subgraph.FindEdge(node1, node2);
            //    List<int> path = DijkstraAlgorithm(matrix, node1.Id - 1, node2.Id - 1);
            //    if (path is null)
            //        continue;
            //    for (int i = 0; i < path.Count - 1; i++)
            //    {
            //        Vertex nOut = subgraph.Vertexes.Find(n => n.Id == path[i] + 1),
            //            nIn = subgraph.Vertexes.Find(n => n.Id == path[i + 1] + 1);
            //        Edge tmpEdge = subgraph.FindEdge(nOut, nIn);
            //        edgeBetweenness[tmpEdge] += 1;
            //    }
            //}
            #endregion

            foreach (var node1 in subgraph.Vertexes)
                foreach (var node2 in subgraph.Vertexes)
                {
                    Edge currEdge = subgraph.FindEdge(node1, node2);
                    List<Vertex> path = subgraph.DijkstraAlgorithm(node1, node2);
                    if (path is null)
                        continue;
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Vertex nOut = subgraph.Vertexes.Find(n => n.Id == path[i].Id),
                            nIn = subgraph.Vertexes.Find(n => n.Id == path[i + 1].Id);
                        Edge tmpEdge = subgraph.FindEdge(nOut, nIn);
                        edgeBetweenness[tmpEdge] += 1;
                    }
                }
        }

        private Result GetCommunityStructure()
        {
            Result cs = new Result();

            int count = 0;
            int n = graph.Vertexes.Count;
            Dictionary<Vertex, bool> visited = new Dictionary<Vertex, bool>();
            for (int i = 0; i < n; i++)
            {
                visited.Add(graph.Vertexes[i], false);
            }
            // Якщо є "висячі" підграфи або вершини, додаємо їх до розбиття
            foreach (Vertex i in graph.Vertexes)
            {
                if (visited[i] == false)
                {
                    count++;
                    Graph subgraph = new Graph();

                    Queue<Vertex> Q = new Queue<Vertex>();
                    visited[i] = true;
                    Q.Enqueue(i);

                    subgraph.Vertexes.Add(i);

                    while (Q.Count != 0)
                    {
                        Vertex v = Q.Dequeue();
                        foreach (Vertex j in v.AdjacencyVertexes)
                        {
                            if (visited[j] == false)
                            {
                                subgraph.Vertexes.Add(j);
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
