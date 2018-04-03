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
            Nodes = new List<Node>();
            Edges = new List<Edge>();
        }

        /// <summary>
        /// Ініціалізація графу будь-яким способом
        /// </summary>
        /// <param name="initializer"></param>
        public void CreateGraph(IGraphInitializer initializer)
        {
            if (initializer is null)
                return;
            initializer.Initialize();
        }



        /// <summary>
        /// Функція, що створює ребро між заданими вершинами
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public Edge CreateLink(Node nodeA, Node nodeB)
        {
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
            Node node = new Node(){Id = pId};
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
        public Node FindNode(int pId, bool add = false)
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
    }
}