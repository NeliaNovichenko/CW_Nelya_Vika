using CW_Nelya_Vika.Models;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CW_Nelya_Vika_Algorithms
{
    public class KernighanLin : IAlgorithm
    {
        public Graph graph;
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

        public GraphList StartPartition(Graph pGraph)
        {
            GraphList r = new GraphList();
            int countVerticies = pGraph.Vertices.Count;
            int columnLength = (int)Math.Ceiling((decimal)(pGraph.Vertices.Count) / pGraph.CommunityCount);
            var list = pGraph.Vertices.Select((item, index) => new { index, item })
                       .GroupBy(x => x.index / columnLength)
                       .Select(x => x.Select(y => y.item).ToList()).ToList();
            foreach (var sublist in list)
            {
                Graph d = new Graph();
                d.Vertices = sublist;
                r.Add(d);
            }
            return r;

        }
        public Dictionary<Vertex, int> D(GraphList graphList)
        {
            Dictionary<Vertex, int> Dv = new Dictionary<Vertex, int>();
            for (int i = 0; i < graphList.Count; i++)
            {
                Graph subgraph = graphList[i];
                for (int j = 0; j < subgraph.Vertices.Count; j++)
                {
                    int innerEdge = 0, crossEdge = 0;

                    var adjNode = graph.FindNode(subgraph.Vertices[j].Label, false).GetAdjacencyVertices();
                    foreach (var node in adjNode)
                    {
                        if (node.IsFixed == false)
                        {
                            var e = graph.FindEdge(subgraph.Vertices[j], node);
                            if (e is null)
                            {
                                continue;
                            }
                            var w = e.Weight;
                            if (subgraph.FindNode(node.Label, false) != null)
                                innerEdge += w;
                            //innerEdge += w;
                            else crossEdge += w;//crossEdge += w;
                        }
                    }
                    Dv.Add(subgraph.Vertices[j], crossEdge - innerEdge);
                }
            }
            //foreach (var item in Dv)
            //{
            //    Console.WriteLine(item.Key.Label + " " + item.Value);
            //}
            return Dv;
        }
        public GraphList CountGrowth(Dictionary<Vertex, int> Dv, GraphList graphList)
        {

            List<Pair> deltag = new List<Pair>();
            for (int i = 0; i < graphList.Count; i++)
            {
                Graph subgraph = graphList[i];
                for (int j = 0; j < subgraph.Vertices.Count; j++)
                {
                    //var adjNode = graph.FindNode(subgraph.Vertices[j].Id, false).GetAdjacencyVertices();
                    //foreach (var vertex in graph.Vertices)
                    for (int k = 0; k < graph.Vertices.Count; k++)
                    {
                        var w1 = graph.FindNode(subgraph.Vertices[j].Label, false);
                        if (w1.IsFixed || graph.Vertices[k].IsFixed) { continue; }
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
            //foreach (var item in deltag)
            //{
            //    Console.WriteLine(item.v1.Label + " " + item.v2.Label + " " + item.delta_g);
            //}
            if (deltag.Count == 0)
            {
                foreach (var item in graph.Vertices)
                {
                    item.IsFixed = true;
                }
                return null;
            }
            int maxvalue = deltag[0].delta_g;
            Pair maxpair = deltag[0];
            for (int i = 0; i < deltag.Count; i++)
            {
                if (deltag[i].delta_g > maxvalue)
                {
                    maxvalue = deltag[i].delta_g;
                    maxpair = deltag[i];
                }
            }
            //Console.WriteLine();
            //Console.WriteLine(maxpair.v1.Label + " " + maxpair.v2.Label + " " + maxvalue);
            maxpair.v1.IsFixed = true;
            maxpair.v2.IsFixed = true;
            int sub1 = 0;
            int sub2 = 0;
            for (int i = 0; i < graphList.Count; i++)
            {
                Graph subgraph = graphList[i];
                var tmp1 = subgraph.FindNode(maxpair.v1.Label, false);
                if (tmp1 != null) { sub1 = i; }
            }
            for (int i = 0; i < graphList.Count; i++)
            {
                Graph subgraph = graphList[i];
                var tmp2 = subgraph.FindNode(maxpair.v2.Label, false);
                if (tmp2 != null) { sub2 = i; }
            }
            var t1 = graphList[sub1].Vertices.Find(x => x.Label == maxpair.v1.Label);
            var t2 = graphList[sub2].Vertices.Find(x => x.Label == maxpair.v2.Label);
            graphList[sub1].Vertices.Remove(t1);
            graphList[sub1].Vertices.Add(t2);
            graphList[sub1].Vertices.Find(x => x.Label == t2.Label).IsFixed = true;
            graph.FindNode(t1.Label, false).IsFixed = true;
            graphList[sub2].Vertices.Remove(t2);
            graphList[sub2].Vertices.Add(t1);
            graphList[sub2].Vertices.Find(x => x.Label == t1.Label).IsFixed = true;
            graph.FindNode(t2.Label, false).IsFixed = true;

            //foreach (var sublist in graphList)
            //{
            //    foreach (var node in sublist.Vertices)
            //    {
            //        Console.Write(node.Label);
            //        Console.Write(' ');
            //    }
            //    Console.WriteLine();
            //}
            return graphList;
            //for (int i = 0; i < graphList.Count; i++)
            //{
            //    Graph subgraph = graphList[i];
            //    var tmp = subgraph.FindNode(maxpair.v1.Label, false);
            //    subgraph.Vertices.Remove(tmp);
            //    subgraph.Vertices.Add(maxpair.v2);
            //}
        }
        public void Exchange(GraphList graphs)
        {

        }
        private bool IsAllFixed(List<Vertex> vertices)
        {
            foreach (var item in vertices)
            {
                if (item.IsFixed == false)
                    return false;
            }
            return true;
        }
        public GraphList FindCommunityStructure(Graph pGraph)
        {
            graph = pGraph.Clone();
            GraphList graphList;

            // начальное разбиение

            graphList = StartPartition(pGraph);

            while (IsAllFixed(graph.Vertices) == false)
            {
                Dictionary<Vertex, int> Dv = D(graphList);
                GraphList graphlist1 = CountGrowth(Dv, graphList);
                if (graphlist1 == null)
                    continue;
                //Exchange(graphList);

                foreach (var sublist in graphlist1)
                {
                    foreach (var node1 in sublist.Vertices)
                    {
                        Console.Write(node1.Label);
                        Console.Write(' ');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

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
    }
}
