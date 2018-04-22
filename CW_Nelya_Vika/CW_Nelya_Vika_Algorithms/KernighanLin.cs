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
            //foreach (var sublist in r)
            //{
            //    foreach (var node in sublist.Vertices)
            //    {
            //        Console.Write(node.Label);
            //        Console.Write(' ');
            //    }
            //    Console.WriteLine();
            //}
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
                //var tmp1 = subgraph.FindNode(maxpair.v1.Label, false);
                //var tmp2 = new Vertex();
                //for (int j = 0; j < subgraph.Vertices.Count; j++)
                //{
                //    if (j == (subgraph.Vertices.Count - 1))
                //    {
                //        tmp2 = subgraph.FindNode(maxpair.v2.Label, false);
                //    }
                //}
                //for (int k = 0; k < subgraph.Vertices.Count; k++)
                //{
                //    subgraph.Vertices[k] = tmp2;
                //}
                //for (int z = 0; z < subgraph.Vertices.Count; z++)
                //{
                //    subgraph.Vertices[z] = tmp1;
                //}
                var tmp1 = subgraph.FindNode(maxpair.v1.Label, false);
                if (tmp1 != null) { sub1 = i; }

                //if (tmp1 != null)
                //{
                //    subgraph.Vertices.Remove(tmp1);
                //    subgraph.Vertices.Add(maxpair.v2);
                //}
                //for (int j = i+1; j < graphList.Count; j++)
                //{
                //    Graph subgraph1 = graphList[i];
                //    var tmp2 = subgraph1.FindNode(maxpair.v2.Label, false);
                //    if (tmp2 != null)
                //    {
                //        subgraph.Vertices.Remove(tmp2);
                //        subgraph.Vertices.Add(maxpair.v1);
                //    }
                //}
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
            graphList[sub2].Vertices.Remove(t2);
            graphList[sub2].Vertices.Add(t1);
            graphList[sub2].Vertices.Find(x => x.Label == t1.Label).IsFixed = true;

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
        public GraphList FindCommunityStructure(Graph pGraph)
        {

            graph = pGraph.Clone();
            GraphList graphList;

            // начальное разбиение

            graphList = StartPartition(pGraph);
            GraphList graphlist1 = new GraphList();

            while (true)
            {
                //Dictionary<Vertex, int> Dv = D(graphList);
                //GraphList graphlist1 = CountGrowth(Dv, graphList);
                //Exchange(graphList);
                foreach (var item in graphList)
                {
                    foreach (var node in item.Vertices)
                    {
                        

                        Console.WriteLine(node.Label + " " + node.IsFixed);
                        while(node.IsFixed == false)
                        {
                            Dictionary<Vertex, int> Dv = D(graphList);
                            graphlist1 = CountGrowth(Dv, graphList);
                            
                        }
                        
                    }
                }
                foreach (var sublist in graphlist1)
                {
                    foreach (var node1 in sublist.Vertices)
                    {
                        Console.Write(node1.Label);
                        Console.Write(' ');
                    }
                    Console.WriteLine();
                }
                break;
            }

            return graphList;
        }
    }
}
