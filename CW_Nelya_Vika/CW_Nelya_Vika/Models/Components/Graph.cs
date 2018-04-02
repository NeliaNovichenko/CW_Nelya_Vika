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
            initializer.Initialize(Nodes, Edges);
        }




        //TODO: За мірою еобхідності реалізувати такі функції:
        //public Edge CreateLink(Node nodeA, Node nodeB)
        //{
        //    Edge edge = new Edge(nodeA, nodeB);

        //    if (nodeA.AdjacencyNodes.Contains(nodeB) || nodeB.AdjacencyNodes.Contains(nodeA))
        //        return null;

        //    nodeB.AdjacencyNodes.Add(nodeA);
        //    nodeA.AdjacencyNodes.Add(nodeB);

        //    nodeA.AdjacencyEdges.Add(edge);
        //    nodeB.AdjacencyEdges.Add(edge);

        //    this.Edges.Add(edge);
        //    return edge;
        //}

        //public Node FindNode(string pLabel, bool add = true)
        //{
        //    for (int i = 0; i < Nodes.Count; i++)
        //    {
        //        if (Nodes[i].Id == pLabel) return Nodes[i];
        //    }

        //    if (add == false) return null;

        //    return CreateNode(pLabel);
        //}

        //public Node CreateNode(string pLabel)
        //{
        //    Node node = new Node();
        //    node.Id = pLabel;
        //    Nodes.Add(node);
        //    return node;
        //}

        //public DGraph Clone()
        //{
        //    DGraph graph = new DGraph();

        //    foreach (Edge e in Edges)
        //    {
        //        Node nodeA = graph.FindNode(e.NodeA.Label);
        //        nodeA.Location = new Point(e.NodeA.Location.X, e.NodeA.Location.Y);
        //        Node nodeB = graph.FindNode(e.NodeB.Id);
        //        nodeB.Location = new Point(e.NodeB.Location.X, e.NodeB.Location.Y);

        //        graph.CreateLink(nodeA, nodeB);
        //    }

        //    return graph;
        //}

        //public Edge FindEdge(Node pNodeA, Node pNodeB)
        //{
        //    foreach (Edge e in pNodeA.AdjacencyEdges)
        //    {
        //        if ((e.NodeA == pNodeA && e.NodeB == pNodeB) || (e.NodeA == pNodeB && e.NodeB == pNodeA))
        //            return e;
        //    }

        //    return null;
        //}

        //public Edge FindEdgeExtract(Node pNodeA, Node pNodeB)
        //{
        //    foreach (Edge e in pNodeA.AdjacencyEdges)
        //    {
        //        if ((e.NodeA == pNodeA && e.NodeB == pNodeB))
        //            return e;
        //    }

        //    return null;
        //}
    }
}