using System;
using System.Collections.Generic;
using System.Linq;
using CW_Nelya_Vika.Models;

namespace CW_Nelya_Vika_Algorithms
{
    public class GirvanNewman : IAlgorithm
    {
        /// <summary>
        /// Кількість найкоротших шляхів між усіма вершинами, що проходять через дане ребро
        /// </summary>
        Dictionary<Edge, double> edgeBetweenness;
        /// <summary>
        /// List<Graph> result - результат розбиття
        /// </summary>
        GraphList graphList;
        /// <summary>
        /// Граф, для якого проводиться розбитта
        /// </summary>
        Graph graph;
        //TODO:
        private double Q;
        public double BestQ { get; set; }

        public string Log { get; private set; }

        public GirvanNewman()
        {

        }
        /// <summary>
        /// Вхідна функція виконання алгоритму
        /// </summary>
        /// <param name="pGraph"></param>
        /// <returns></returns>
        public GraphList FindCommunityStructure(Graph pGraph)
        {
            //// Clone graph
            //if (pGraph is null)
            //    return null;
            //graph = pGraph.Clone();
            ////Тимчасове розбиття
            //GraphList tempGraphList = GetCommunityStructure();
            //int initCount = tempGraphList.Count;
            //int countCommunity = tempGraphList.Count;
            ////обчислюємо Betweenness
            //InitializeEdgeBetweenness(graph);

            //// Q
            //BestQ = 0;
            //Q = 0;

            //while (true)
            //{
            //    while (countCommunity <= initCount)
            //    {
            //        //видалити ребро с макс Betweenness
            //        Graph community = RemoveMaxEdgeBetweenness(tempGraphList);
            //        //перерахувати  Betweenness для нового community
            //        InitializeEdgeBetweenness(community);

            //        //розбити граф
            //        tempGraphList = GetCommunityStructure();
            //        //кількість розбиттів
            //        countCommunity = tempGraphList.Count;
            //    }

            //    initCount = countCommunity;


            //    Q = CalculateModularity(tempGraphList, pGraph);
            //    if (Q > BestQ)
            //    {
            //        BestQ = Q;
            //        graphList = tempGraphList;
            //    }

            //    if (graph.Edges.Count == 0)
            //        break;
            //}


            //return graphList;

            graph = pGraph.Clone();

            var tempCS = GetCommunityStructure();
            int initCount = tempCS.Count;
            int countCommunity = initCount;

            // Q
            BestQ = 0;
            Q = 0;

            InitializeEdgeBetweenness(graph);

            // tính thuật toán
            int j = 0;
            while (countCommunity < pGraph.CommunityCount)
            {
                while (countCommunity <= initCount)
                {
                    Graph community = RemoveMaxEdgeBetweenness(tempCS); // Xóa cạnh lớn nhất và cho biết community nào có cạnh được xóa

                    CalculateEdgeBetweenness(community);

                    tempCS = GetCommunityStructure();
                    countCommunity = tempCS.Count;
                }

                initCount = countCommunity;
                graphList = tempCS;
                // Tính Q
                //Q = CalculateModularity(tempCS, pGraph);
                //if (Q > BestQ)
                //{
                //    BestQ = Q;
                //    graphList = tempCS;

                //}

                if (graph.Edges.Count == 0) break;
            }

            foreach (var g in graphList)
            {
                foreach (var v1 in g.Vertices)
                {
                    foreach (var v2 in g.Vertices)
                    {
                        Edge e = pGraph.FindEdge(v1, v2);
                        if (e == null)
                            continue;
                        g.CreateLink(e.VertexOut, e.VertexIn, e.Weight);
                        g.Edges.Add(e);
                    }
                }
            }

            return graphList;
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
        /// <param name="graphList"></param>
        /// <param name="pOriginalGraph"></param>
        /// <returns></returns>
        public static double CalculateModularity(GraphList graphList, Graph pOriginalGraph)
        {
            double modularity = 0;
            int numEdge = pOriginalGraph.Edges.Count;
            foreach (Graph subGraph in graphList)
            {
                int l = 0;
                int d = 0;
                foreach (Vertex node in subGraph.Vertices)
                {
                    l += node.GetAdjacencyVertices().Count;
                    d += pOriginalGraph.FindNode(node.Label, false).GetAdjacencyVertices().Count;
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
        private Graph RemoveMaxEdgeBetweenness(GraphList pTempCS)
        {
            if (edgeBetweenness == null || edgeBetweenness.Count == 0)
                return null;
            double max = edgeBetweenness.Max(n => n.Value);
            Edge e = edgeBetweenness.Where(n => Math.Abs(n.Value - max) < 1e-6).ToArray()[0].Key;

            graph.Edges.Remove(e);

            e.VertexOut.AdjacencyEdges.Remove(e);
            e.VertexIn.AdjacencyEdges.Remove(e);

            e.VertexOut.GetAdjacencyVertices().Remove(e.VertexIn);
            e.VertexIn.GetAdjacencyVertices().Remove(e.VertexOut);

            WriteLog(" - Remove: (" + e.VertexOut.Label + ", " + e.VertexIn.Label + ")");

            edgeBetweenness.Remove(e);

            foreach (Graph subgraph in pTempCS)
            {
                if (subgraph.Vertices.Contains(e.VertexOut))
                    return subgraph;
            }
            return null;
        }

        /// <summary>
        /// Ініціалізуємо edgeBetweenness та 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="community"></param>
        public void InitializeEdgeBetweenness(Graph community = null, Graph g = null)
        {
            if (g is null)
                g = graph;
            if (edgeBetweenness == null)
            {
                edgeBetweenness = new Dictionary<Edge, double>();
                //Ініціалізуємо edgeBetweenness: для всіх ребер спочатку значення = 0
                foreach (Edge e in g.Edges)
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
        public Dictionary<Edge, double> CalculateEdgeBetweenness(Graph subgraph)
        {
            #region Old
            //if (subgraph == null)
            //    return;
            //int nodesCount = subgraph.Vertices.Count;
            //int MAX = Int32.MaxValue;

            //foreach (Vertex vertex in subgraph.Vertices)
            //{
            //    foreach (Edge e in vertex.AdjacencyEdges)
            //    {
            //        edgeBetweenness[e] = 0;
            //    }
            //}

            //foreach (Vertex vertex in subgraph.Vertices)
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
            //    foreach (Vertex _node in subgraph.Vertices)
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

            //        foreach (Vertex adjacencyNode in currentVertex.GetAdjacencyVertices())
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
            //foreach (var node1 in subgraph.Vertices)
            //foreach (var node2 in subgraph.Vertices)
            //{
            //    Edge currEdge = subgraph.FindEdge(node1, node2);
            //    List<int> path = DijkstraAlgorithm(matrix, node1.Id - 1, node2.Id - 1);
            //    if (path is null)
            //        continue;
            //    for (int i = 0; i < path.Count - 1; i++)
            //    {
            //        Vertex nOut = subgraph.Vertices.Find(n => n.Id == path[i] + 1),
            //            nIn = subgraph.Vertices.Find(n => n.Id == path[i + 1] + 1);
            //        Edge tmpEdge = subgraph.FindEdge(nOut, nIn);
            //        edgeBetweenness[tmpEdge] += 1;
            //    }
            //}
            #endregion

            foreach (var node1 in subgraph.Vertices)
                foreach (var node2 in subgraph.Vertices)
                {
                    Edge currEdge = subgraph.FindEdge(node1, node2);
                    List<Vertex> path = subgraph.DijkstraAlgorithm(node1, node2);
                    if (path is null)
                        continue;
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Vertex nOut = subgraph.Vertices.Find(n => n.Label == path[i].Label),
                            nIn = subgraph.Vertices.Find(n => n.Label == path[i + 1].Label);
                        Edge tmpEdge = subgraph.FindEdge(nOut, nIn);
                        edgeBetweenness[tmpEdge] += 1;
                    }
                }
            return edgeBetweenness;
        }

        private GraphList GetCommunityStructure()
        {
            GraphList cs = new GraphList();

            int count = 0;
            int n = graph.Vertices.Count;
            Dictionary<Vertex, bool> visited = new Dictionary<Vertex, bool>();
            for (int i = 0; i < n; i++)
            {
                visited.Add(graph.Vertices[i], false);
            }
            // Якщо є "висячі" підграфи або вершини, додаємо їх до розбиття
            foreach (Vertex i in graph.Vertices)
            {
                if (visited[i] == false)
                {
                    count++;
                    Graph subgraph = new Graph();

                    Queue<Vertex> Q = new Queue<Vertex>();
                    visited[i] = true;
                    Q.Enqueue(i);

                    //subgraph.Vertices.Add(i);
                    subgraph.FindNode(i.Label, true);

                    while (Q.Count != 0)
                    {
                        Vertex v = Q.Dequeue();
                        foreach (Vertex j in v.GetAdjacencyVertices())
                        {
                            if (visited[j] == false)
                            {
                                //subgraph.Vertices.Add(j);
                                subgraph.FindNode(j.Label, true);
                                Edge e = graph.FindEdge(v, j);
                                subgraph.CreateLink(v, j, e.Weight);
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
