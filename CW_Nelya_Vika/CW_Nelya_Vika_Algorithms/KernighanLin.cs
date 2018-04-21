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
        struct Pair
        {
            public Vertex v1;
            public Vertex v2;
            public int delta_g;

            public Pair(Vertex ver1, Vertex ver2, int d)
            {
                v1 = ver1;
                v2 = ver2;
                delta_g = d;
            }
        }

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
            foreach (var item in Dv)
            {
                Console.WriteLine(item.Key.Label + " " + item.Value);
            }
            return Dv;
        }
        public void CountGrowth(Dictionary<Vertex, int> Dv, Result result)
        {
            List<Pair> deltag = new List<Pair>();
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
                            int delta_g = Dv[graph.Vertices[k]] + Dv[subgraph.Vertices[j]] - 2 * w;
                            deltag.Add(new Pair(subgraph.Vertices[j], graph.Vertices[k], delta_g));
                        }
                    }
                }
            }
            foreach (var item in deltag)
            {
                Console.WriteLine(item.v1.Label + " " + item.v2.Label + " " + item.delta_g);
            }
            //int maxg = deltag.Values.Max();
            //var keys = from entry in deltag
            //           where entry.Value == maxg
            //           select entry.Key;
            //foreach (var item1 in keys)
            //{
            //    foreach (var item2 in item1)
            //    {
            //        item2.IsFixed = true;
            //    }
            //}
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
                break;
            }

            return result;
        }
    }
}
