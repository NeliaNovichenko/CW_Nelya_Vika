using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Клас, що описує граф
    /// </summary>
    public class Graph
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Бажана кількість розбиттів
        /// </summary>
        public int CommunityCount { get; set; }

        /// <summary>
        /// Нижня границя кількості вершин у підграфі
        /// </summary>
        public int MinCountInSubgraph { get; set; }

        /// <summary>
        /// Верхня границя кількості вершин у графі
        /// </summary>
        public int MaxCountInSubgraph { get; set; }

        /// <summary>
        /// Список вершин графу
        /// </summary>
        public List<Vertex> Vertices { get; set; }

        /// <summary>
        /// Список ребер графу
        /// </summary>
        public List<Edge> Edges { get; set; }

        public List<Problem> Problems { get; set; }


        public Graph()
        {
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
        }

        /// <summary>
        /// Функція, що створює ребро між заданими вершинами
        /// </summary>
        /// <param name="vertexA"></param>
        /// <param name="vertexB"></param>
        /// <returns></returns>
        public Edge CreateLink(Vertex vertexA, Vertex vertexB)
        {
            if (vertexA is null || vertexB is null)
                return null;
            Edge edge = new Edge(vertexA, vertexB);

            if (vertexA.GetAdjacencyVertices().Contains(vertexB) || vertexB.GetAdjacencyVertices().Contains(vertexA))
                return null;

            vertexB.GetAdjacencyVertices().Add(vertexA);
            vertexA.GetAdjacencyVertices().Add(vertexB);

            vertexA.AdjacencyEdges.Add(edge);
            vertexB.AdjacencyEdges.Add(edge);

            this.Edges.Add(edge);
            return edge;
        }

        /// <summary>
        /// Створює та додає нову вершину до графу
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public Vertex CreateVertex(int pLable)
        {
            Vertex vertex = new Vertex() { Label = pLable };
            Vertices.Add(vertex);
            return vertex;
        }

        /// <summary>
        /// Шукає вузол по заданому ідентифікатору.
        /// Якщо вказаний ідентифікатор "додати", то створює та додає новий вузол.
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public Vertex FindNode(int pLable, bool add = true)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Label == pLable)
                    return Vertices[i];
            }

            if (add == false)
                return null;

            return CreateVertex(pLable);
        }

        /// <summary>
        /// Знайти ребро між заданими вершинами.
        /// Направлення не має значення.
        /// </summary>
        /// <param name="pVertexOut"></param>
        /// <param name="pVertexIn"></param>
        /// <returns></returns>
        public Edge FindEdge(Vertex pVertecesOut, Vertex pVertecesIn)
        {
            foreach (Edge e in pVertecesOut.AdjacencyEdges)
            {
                if ((e.VertexOut == pVertecesOut && e.VertexIn == pVertecesIn) || (e.VertexOut == pVertecesIn && e.VertexIn == pVertecesOut))
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
        public Edge FindEdgeExtract(Vertex pVertecesOut, Vertex pVertecesIn)
        {
            foreach (Edge e in pVertecesOut.AdjacencyEdges)
            {
                if ((e.VertexOut == pVertecesOut && e.VertexIn == pVertecesIn))
                    return e;
            }

            return null;
        }

        /// <summary>
        /// Shortest path using Dijkstra Algorithm
        /// </summary>
        /// <param name="sourceVertex"></param>
        /// <param name="destinationVertex"></param>
        /// <returns></returns>
        public List<Vertex> DijkstraAlgorithm(Vertex sourceVertex, Vertex destinationVertex)
        {
            var n = this.Vertices.Count;
            Dictionary<Vertex, int> distance = new Dictionary<Vertex, int>();
            Dictionary<Vertex, bool> used = new Dictionary<Vertex, bool>();
            Dictionary<Vertex, Vertex> previous = new Dictionary<Vertex, Vertex>();
            //var distance = new int[n];
            foreach (var node in this.Vertices)
            {
                distance.Add(node, int.MaxValue);
                used.Add(node, false);
                previous.Add(node, null);
            }

            distance[sourceVertex] = 0;

            while (true)
            {
                var minDistance = int.MaxValue;
                Vertex minVertex = null;
                foreach (var node in this.Vertices)
                    if (!used[node] && minDistance > distance[node])
                    {
                        minDistance = distance[node];
                        minVertex = node;
                    }

                if (minDistance == int.MaxValue)
                    break;

                used[minVertex] = true;

                foreach (var node in this.Vertices)
                {
                    Edge edge = this.Edges.Find(e => e.VertexOut == minVertex && e.VertexIn == node);
                    if (edge != null)
                    {
                        var shortestToMinNode = distance[minVertex];
                        var distanceToNextNode = edge.Weight;
                        var totalDistance = shortestToMinNode + distanceToNextNode;

                        if (totalDistance < distance[node])
                        {
                            distance[node] = totalDistance;
                            previous[node] = minVertex;
                        }
                    }
                }
            }
            if (distance[destinationVertex] == int.MaxValue)
                return null;
            var path = new LinkedList<Vertex>();
            Vertex currentVertex = destinationVertex;
            while (currentVertex != null)
            {
                path.AddFirst(currentVertex);
                currentVertex = previous[currentVertex];
            }
            return path.ToList();
        }

        /// <summary>
        /// Створити копію графа
        /// </summary>
        /// <returns></returns>
        public Graph Clone()
        {
            Graph graph = new Graph();

            foreach (Edge e in Edges)
            {
                Vertex VertexOut = graph.FindNode(e.VertexOut.Label);
                Vertex VertexIn = graph.FindNode(e.VertexIn.Label);

                graph.CreateLink(VertexOut, VertexIn);
            }
            return graph;
        }


        public override string ToString()
        {
            string result = string.Format("Id: {0};" +
                                          " Кількість розбиттів: {1};" +
                                          " Мінімальна-максимальна кількість вершин у підграфах: {2}-{3}; ",
                Id, CommunityCount, MinCountInSubgraph, MaxCountInSubgraph);
            foreach (var edge in Edges)
            {
                result += edge.ToString();
            }
            return result;
        }
    }
}