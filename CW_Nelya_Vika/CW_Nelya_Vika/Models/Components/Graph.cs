using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Клас, що описує граф
    /// </summary>
    public class Graph
    {
        private static int count = 0;

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Список вершин графу
        /// </summary>
        public List<Node> Nodes { get; set; }

        /// <summary>
        /// Список ребер графу
        /// </summary>
        public List<Edge> Edges { get; set; }

        public Graph()
        {
            Id = count++;
            Nodes = new List<Node>();
            Edges = new List<Edge>();
        }

        //TODO: think how to realize method 
        /// <summary>
        /// Ініціалізація графу будь-яким способом
        /// </summary>
        /// <param name="initializer"></param>
        //public void CreateGraph(IGraphInitializer initializer)
        //{
        //    if (initializer is null)
        //        return;
        //    if (!initializer.Initialize(this))
        //        //TODO: Create custom exception cannot create graph
        //        throw new Exception("cannot create graph");
        //}

        /// <summary>
        /// Функція, що створює ребро між заданими вершинами
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public Edge CreateLink(Node nodeA, Node nodeB)
        {
            if (nodeA is null || nodeB is null)
                return null;
            Edge edge = new Edge(nodeA, nodeB);

            if (nodeA.AdjacencyNodes.Contains(nodeB) || nodeB.AdjacencyNodes.Contains(nodeA))
                return null;

            nodeB.AdjacencyNodes.Add(nodeA);
            nodeA.AdjacencyNodes.Add(nodeB);

            nodeA.AdjacencyEdges.Add(edge);
            nodeB.AdjacencyEdges.Add(edge);

            this.Edges.Add(edge);
            return edge;
        }

        /// <summary>
        /// Створює та додає нову вершину до графу
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public Node CreateNode(int pId)
        {
            Node node = new Node() {Id = pId};
            Nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Шукає вузол по заданому ідентифікатору.
        /// Якщо вказаний ідентифікатор "додати", то створює та додає новий вузол.
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public Node FindNode(int pId, bool add = true)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Id == pId)
                    return Nodes[i];
            }

            if (add == false)
                return null;

            return CreateNode(pId);
        }

        /// <summary>
        /// Знайти ребро між заданими вершинами.
        /// Направлення не має значення.
        /// </summary>
        /// <param name="pNodeOut"></param>
        /// <param name="pNodeIn"></param>
        /// <returns></returns>
        public Edge FindEdge(Node pNodeOut, Node pNodeIn)
        {
            foreach (Edge e in pNodeOut.AdjacencyEdges)
            {
                if ((e.NodeOut == pNodeOut && e.NodeIn == pNodeIn) || (e.NodeOut == pNodeIn && e.NodeIn == pNodeOut))
                    return e;
            }
            return null;
        }

        /// <summary>
        /// Знайти ребро між заданими вершинами.
        /// Направлення має значення.
        /// </summary>
        /// <param name="pNodeA"></param>
        /// <param name="pNodeB"></param>
        /// <returns></returns>
        public Edge FindEdgeExtract(Node pNodeOut, Node pNodeIn)
        {
            foreach (Edge e in pNodeOut.AdjacencyEdges)
            {
                if ((e.NodeOut == pNodeOut && e.NodeIn == pNodeIn))
                    return e;
            }

            return null;
        }




        //TODO: move function to another class :IGraphInitializer
        public Graph Clone()
        {
            Graph graph = new Graph();

            foreach (Edge e in Edges)
            {
                Node nodeOut = graph.FindNode(e.NodeOut.Id);
                Node nodeIn = graph.FindNode(e.NodeIn.Id);

                graph.CreateLink(nodeOut, nodeIn);
            }
            return graph;
        }

        public int[,] GraphToMatrix()
        {
            int[,] matrix = new int[this.Nodes.Count, this.Nodes.Count];
            foreach (var edge in this.Edges)
            {
                matrix[edge.NodeOut.Id - 1, edge.NodeIn.Id - 1] = edge.Weight;
            }
            return matrix;
        }


        /// <summary>
        /// Shortest path using Dijkstra Algorithm
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="destinationNode"></param>
        /// <returns></returns>
        public List<Node> DijkstraAlgorithm(Node sourceNode, Node destinationNode)
        {
            var n = this.Nodes.Count;
            Dictionary<Node, int> distance = new Dictionary<Node, int>();
            Dictionary<Node, bool> used = new Dictionary<Node, bool>();
            Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
            //var distance = new int[n];
            foreach (var node in this.Nodes)
            {
                distance.Add(node, int.MaxValue);
                used.Add(node, false);
                previous.Add(node, null);
            }

            distance[sourceNode] = 0;

            while (true)
            {
                var minDistance = int.MaxValue;
                Node minNode = null;
                foreach (var node in this.Nodes)
                    if (!used[node] && minDistance > distance[node])
                    {
                        minDistance = distance[node];
                        minNode = node;
                    }

                if (minDistance == int.MaxValue)
                    break;

                used[minNode] = true;

                foreach (var node in this.Nodes)
                {
                    Edge edge = this.Edges.Find(e => e.NodeOut == minNode && e.NodeIn == node);
                    if (edge != null)
                    {
                        var shortestToMinNode = distance[minNode];
                        var distanceToNextNode = edge.Weight;
                        var totalDistance = shortestToMinNode + distanceToNextNode;

                        if (totalDistance < distance[node])
                        {
                            distance[node] = totalDistance;
                            previous[node] = minNode;
                        }
                    }
                }
            }
            if (distance[destinationNode] == int.MaxValue)
                return null;
            var path = new LinkedList<Node>();
            Node currentNode = destinationNode;
            while (currentNode != null)
            {
                path.AddFirst(currentNode);
                currentNode = previous[currentNode];
            }
            return path.ToList();
        }
        /// <summary>
        /// Shortest path using Dijkstra Algorithm 
        /// graph represented in matrix
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="destinationNode"></param>
        /// <returns></returns>
        public List<int> DijkstraAlgorithm(int sourceNode, int destinationNode)
        {
            int[,] graph = GraphToMatrix();
            var n = graph.GetLength(0);
            var distance = new int[n];
            for (int i = 0; i < n; i++)
                distance[i] = int.MaxValue;

            distance[sourceNode] = 0;
            var used = new bool[n];
            var previous = new int?[n];

            while (true)
            {
                var minDistance = int.MaxValue;
                var minNode = 0;
                for (int i = 0; i < n; i++)
                    if (!used[i] && minDistance > distance[i])
                    {
                        minDistance = distance[i];
                        minNode = i;
                    }

                if (minDistance == int.MaxValue)
                    break;
                used[minNode] = true;

                for (int i = 0; i < n; i++)
                    if (graph[minNode, i] > 0)
                    {
                        var shortestToMinNode = distance[minNode];
                        var distanceToNextNode = graph[minNode, i];

                        var totalDistance = shortestToMinNode + distanceToNextNode;

                        if (totalDistance < distance[i])
                        {
                            distance[i] = totalDistance;
                            previous[i] = minNode;
                        }
                    }
            }
            if (distance[destinationNode] == int.MaxValue)
                return null;
            var path = new LinkedList<int>();
            int? currentNode = destinationNode;
            while (currentNode != null)
            {
                path.AddFirst(currentNode.Value);
                currentNode = previous[currentNode.Value];
            }
            return path.ToList();
        }
    }
}