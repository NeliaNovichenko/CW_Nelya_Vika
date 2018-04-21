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
            int countVerticies = pGraph.Vertices.Count;
            int columnLength = (int)Math.Ceiling((decimal)(pGraph.Vertices.Count) / pGraph.CommunityCount);
            var list = pGraph.Vertices.Select((item, index) => new { index, item })
                       .GroupBy(x => x.index / columnLength)
                       .Select(x => x.Select(y => y.item).ToList()).ToList();
            foreach (var sublist in list)
            {
                Graph d = new Graph();
                d.Vertices = sublist;
                //foreach (var node1 in sublist)
                //{
                //    foreach(var node2 in sublist)
                //    {
                //        var edge = pGraph.Edges.Find(e => e.VertexOut == node1 && e.VertexIn == node2);
                //        d.Edges.Add(edge);
                //    }
                //}
                r.Add(d);
            }
            foreach (var sublist in r)
            {
                foreach (var node in sublist.Vertices)
                {
                    Console.Write(node.Label);
                    Console.Write(' ');
                }
                Console.WriteLine();
                Console.WriteLine(sublist.GetType());
            }
            return r;

        }
        public Dictionary<Vertex, int> D(Result result)
        {
            Dictionary<Vertex, int> Dv = new Dictionary<Vertex, int>();
            for (int i = 0; i < result.Count; i++)
            {
                Graph subgraph = result[i];
                for (int j = 0; j < subgraph.Vertices.Count; j++)
                {
                    int innerEdge = 0, crossEdge = 0;

                    var adjNode = graph.FindNode(subgraph.Vertices[j].Label, false).GetAdjacencyVertices();
                    foreach (var node in adjNode)
                    {
                        if (node.IsFixed == false)
                        {
                            //var w = graph.FindEdge(subgraph.Vertices[j], vertex).Weight;
                            if (subgraph.FindNode(node.Label, false) != null)
                                innerEdge++;
                            //innerEdge += w;
                            else crossEdge++;//crossEdge += w;
                        }
                    }
                    Dv.Add(subgraph.Vertices[j], crossEdge - innerEdge);
                }
            }
            return Dv;
        }
        public void CountGrowth(Dictionary<Vertex, int> Dv, Result result)
        {
            Dictionary<List<Vertex>, int> delta_g = new Dictionary<List<Vertex>, int>();
            for (int i = 0; i < result.Count; i++)
            {
                Graph subgraph = result[i];
                for (int j = 0; j < subgraph.Vertices.Count; j++)
                {
                    //var adjNode = graph.FindNode(subgraph.Vertices[j].Id, false).GetAdjacencyVertices();
                    //foreach (var vertex in graph.Vertices)
                    for (int k = 0; k < graph.Vertices.Count; k++)
                    {
                        int w = 0;
                        //var w = graph.FindEdge(subgraph.Vertices[j], vertex).Weight;
                        if (subgraph.FindNode(graph.Vertices[k].Label, false) == null)
                        {
                            var e = graph.FindEdge(subgraph.Vertices[j], graph.Vertices[k]);
                            if (e != null) w = 1;

                            List<Vertex> list = new List<Vertex>();
                            list.Add(graph.Vertices[k]);
                            list.Add(subgraph.Vertices[j]);
                            int a = Dv[graph.Vertices[k]];
                            int b = Dv[subgraph.Vertices[j]];
                            delta_g.Add(list, Dv[graph.Vertices[k]] + Dv[subgraph.Vertices[j]] - 2 * w);
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
                    item2.IsFixed = true;
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
                Dictionary<Vertex, int> Dv = D(result);
                CountGrowth(Dv, result);
            }

            return result;
        }
    }
}
