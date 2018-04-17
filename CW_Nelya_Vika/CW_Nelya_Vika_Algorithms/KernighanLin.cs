using CW_Nelya_Vika.Models;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CW_Nelya_Vika_Algorithms
{
    public class KernighanLin : IAlgorithm
    {
        private Graph graph;
        public Result StartPartition(Graph pGraph)
        {
            Result r = new Result();
            int countVerticies = pGraph.Nodes.Count;
            int columnLength = (int)Math.Ceiling((decimal)(pGraph.Nodes.Count) / pGraph.CommunityCount);
            var list = pGraph.Nodes.Select((item, index) => new { index, item })
                       .GroupBy(x => x.index / columnLength)
                       .Select(x => x.Select(y => y.item).ToList()).ToList();
            foreach (var sublist in list)
            {
                Graph d = new Graph();
                d.Nodes = sublist;
                //foreach (var node1 in sublist)
                //{
                //    foreach(var node2 in sublist)
                //    {
                //        var edge = pGraph.Edges.Find(e => e.NodeOut == node1 && e.NodeIn == node2);
                //        d.Edges.Add(edge);
                //    }
                //}
                r.Add(d);
            }
            foreach (var sublist in r)
            {
                foreach (var node in sublist.Nodes)
                {
                    Console.Write(node.Id);
                    Console.Write(' ');
                }
                Console.WriteLine();
                Console.WriteLine(sublist.GetType());
            }
            return r;

        }
        public Dictionary<Node, int> D(Result result)
        {
            Dictionary<Node, int> Dv = new Dictionary<Node, int>();
            for (int i = 0; i < result.Count; i++)
            {
                Graph subgraph = result[i];
                for (int j = 0; j < subgraph.Nodes.Count; j++)
                {
                    int innerEdge = 0, crossEdge = 0;

                    var adjNode = graph.FindNode(subgraph.Nodes[j].Id, false).AdjacencyNodes;
                    foreach (var node in adjNode)
                    {
                        if (node.NodeFixed == false)
                        {
                            //var w = graph.FindEdge(subgraph.Nodes[j], node).Weight;
                            if (subgraph.FindNode(node.Id, false) != null)
                                innerEdge++;
                            //innerEdge += w;
                            else crossEdge++;//crossEdge += w;
                        }
                    }
                    Dv.Add(subgraph.Nodes[j], crossEdge - innerEdge);
                }
            }
            return Dv;
        }
        public void CountGrowth(Dictionary<Node, int> Dv, Result result)
        {
            Dictionary<List<Node>, int> delta_g = new Dictionary<List<Node>, int>();
            for (int i = 0; i < result.Count; i++)
            {
                Graph subgraph = result[i];
                for (int j = 0; j < subgraph.Nodes.Count; j++)
                {
                    //var adjNode = graph.FindNode(subgraph.Nodes[j].Id, false).AdjacencyNodes;
                    //foreach (var node in graph.Nodes)
                    for (int k = 0; k < graph.Nodes.Count; k++)
                    {
                        int w = 0;
                        //var w = graph.FindEdge(subgraph.Nodes[j], node).Weight;
                        if (subgraph.FindNode(graph.Nodes[k].Id, false) == null)
                        {
                            var e = graph.FindEdge(subgraph.Nodes[j], graph.Nodes[k]);
                            if (e != null) w = 1;

                            List<Node> list = new List<Node>();
                            list.Add(graph.Nodes[k]);
                            list.Add(subgraph.Nodes[j]);
                            int a = Dv[graph.Nodes[k]];
                            int b = Dv[subgraph.Nodes[j]];
                            delta_g.Add(list, Dv[graph.Nodes[k]] + Dv[subgraph.Nodes[j]] - 2 * w);
                        }
                    }
                }
            }
            int maxg = delta_g.Values.Max();
            var keys = from entry in delta_g
                       where entry.Value == maxg
                       select entry.Key;
            foreach(var item1 in keys)
            {
                foreach(var item2 in item1)
                {
                    item2.NodeFixed = true;
                }
            }
        }
        public Result FindCommunityStructure(Graph pGraph)
        {

            graph = pGraph.Clone();
            Result result;

            // начальное разбиение

            result = StartPartition(pGraph);

            while (true)
            {
                Dictionary<Node, int> Dv = D(result);
                CountGrowth(Dv, result);
            }

            return result;
        }
    }
}
